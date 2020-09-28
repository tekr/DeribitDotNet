using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using DeribitDotNet.JsonConverters;
using DeribitDotNet.Notifications;
using DeribitDotNet.Requests;
using DeribitDotNet.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using Timer = System.Timers.Timer;

namespace DeribitDotNet
{
    public class DeribitApi : IDisposable
    {
        private static readonly TimeSpan RequestTimeout = TimeSpan.FromSeconds(120);
        private static readonly TimeSpan MarketDataWatchdogTimeout = TimeSpan.FromSeconds(20);

        private static readonly TimeSpan HeartbeatInterval = TimeSpan.FromSeconds(15);

        private static readonly TestRequest TestRequest = new TestRequest();

        private readonly IList<SubscribeRequest> _activeSubscriptions = new List<SubscribeRequest>();

        private readonly string _key;
        private readonly string _secret;

        private readonly ConcurrentDictionary<long, Action<string, ErrorResponse, DateTime>> _responseTypesByRequestId =
            new ConcurrentDictionary<long, Action<string, ErrorResponse, DateTime>>();

        private readonly Utils.ResilientWebSocket _webSocket;
        private readonly Timer _marketDataWatchdog;

        private long _nextRequestId;

        private bool _initialised;
        private bool _resubscribing;
        private bool _disposed;

        private string _token;
        private DateTime _tokenExpiryTime;

        public bool MarketDataWatchdogEnabled { get; set; } = true;

        public IObservable<OrderBookNotification> OrderBooks => OrderBooksSubject.AsObservable();   
        public IObservable<TickerNotification> Tickers => TickersSubject.AsObservable();
        public IObservable<OrderNotification> Orders => OrdersSubject.AsObservable();
        public IObservable<TradeNotification> Trades => TradesSubject.AsObservable();
        public IObservable<UserTrade> UserTrades => UserTradesSubject.AsObservable();

        internal static readonly JsonSerializerSettings SerialiserSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Converters =
            {
                new FloatJsonConverter(),
                new NestedPathJsonConverter<OrderOperationResponse>(),
                new NestedPathJsonConverter<TradesResponse>(),
            },
        };

        internal Subject<OrderBookNotification> OrderBooksSubject = new Subject<OrderBookNotification>();
        internal Subject<TickerNotification> TickersSubject = new Subject<TickerNotification>();
        internal Subject<OrderNotification> OrdersSubject = new Subject<OrderNotification>();
        internal Subject<TradeNotification> TradesSubject = new Subject<TradeNotification>();
        internal Subject<UserTrade> UserTradesSubject = new Subject<UserTrade>();

        public DeribitApi(string key, string secret, bool testEnvironment = false)
        {
            _key = key;
            _secret = secret;

            _webSocket = new Utils.ResilientWebSocket($"wss://{(testEnvironment ? "test" : "www")}.deribit.com/ws/api/v2");

            Log.Debug($"Deribit API created with key {key}. Configured for {(testEnvironment ? "test" : "prod")} URL");

            _webSocket.Connected += OnWebsocketConnected;
            _webSocket.Closed += (s, e) => _token = null;
            _webSocket.MessageReceived += OnWebsocketMessage;

            _marketDataWatchdog = new Timer(MarketDataWatchdogTimeout.TotalMilliseconds);
            _marketDataWatchdog.Elapsed += (s, e) =>
            {
                if (MarketDataWatchdogEnabled)
                {
                    Log.Error($"No message received from Deribit for {MarketDataWatchdogTimeout.TotalSeconds} seconds. Resetting");
                    _ = _webSocket.Reset();
                }
            };
        }

        public async Task Initialise()
        {
            if (_disposed) throw new ObjectDisposedException("DeribitApi", "Object has been disposed");

            if (!_initialised)
            {
                Log.Debug("Initialising Deribit API");

                await _webSocket.Initialise();
                _initialised = true;

                await Send(new SetHeartbeatRequest((int)HeartbeatInterval.TotalSeconds));
            }
        }

        public void Reconnect()
        {
            _ = _webSocket.Reset();
        }

        public async Task<TResponse> Send<TResponse>(Request<TResponse> request) where TResponse : Response, new()
        {
            EnsureInitialised();

            await _webSocket.ConnectionReadyAsync();

            if (!request.IsPublic)
            {
                await EnsureAuthenticated();
            }

            var requestHolder = new RequestHolder<Request<TResponse>, TResponse>(Interlocked.Increment(ref _nextRequestId), request, _token);
            var message = SerialisationHelper.Serialise(requestHolder);

            var taskCompletionSource = new TaskCompletionSource<TResponse>();
            _responseTypesByRequestId[requestHolder.Id] = (m, s, t) => HandleResponse(taskCompletionSource, m, s, t, request);

            _ = _webSocket.Send(message);

            var completedTask = await Task.WhenAny(taskCompletionSource.Task, Task.Delay(RequestTimeout));

            if (completedTask != taskCompletionSource.Task)
            {
                taskCompletionSource.SetException(new TimeoutException("The operation timed out"));
                _responseTypesByRequestId.TryRemove(requestHolder.Id, out _);
            }

            if (request is SubscribeRequest subscriptionRequest && !_resubscribing)
            {
                _marketDataWatchdog.Start();
                Log.Information($"Recording subscription for {string.Join(",", subscriptionRequest.Channels)}");
                _activeSubscriptions.Add(subscriptionRequest);
            }

            return await taskCompletionSource.Task;
        }

        private async Task EnsureAuthenticated()
        {
            if (_token == null || (_tokenExpiryTime - DateTime.UtcNow).TotalSeconds < 30)
            {
                while (true)
                {
                    var response = await Send(AuthenticateRequest.ClientCredentials(_key, _secret));

                    if (response.Error != null)
                    {
                        Log.Error($"Error requesting authentication token: {response.Error}");
                    } else if (string.IsNullOrWhiteSpace(response.Result.AccessToken))
                    {
                        Log.Error("Returned authentication token was null or empty!");
                    }
                    else
                    {
                        _token = response.Result.AccessToken;
                        _tokenExpiryTime = response.ArrivalTime.AddSeconds(response.Result.ExpiresInSec);

                        Log.Information($"Authentication token received. Expires {_tokenExpiryTime}");
                        break;
                    }

                    Log.Information("Pausing for 5 seconds and retrying");
                    await Task.Delay(5000);
                }
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                _webSocket?.Dispose();
                _marketDataWatchdog.Dispose();
            }
        }

        private static void HandleResponse<TResponse>(TaskCompletionSource<TResponse> taskCompletionSource,
            string message, ErrorResponse error, DateTime arrivalTime, Request request) where TResponse : Response, new()
        {
            try
            {
                TResponse response;

                if (error == null)
                {
                    response = SerialisationHelper.Deserialise<TResponse>(message);
                }
                else
                {
                    // Ignore order overlap and order not open errors (can occur in the normal course of trading due to in-flight operations)
                    if (error.Code != 10003 && error.Code != 11044 && error.Code != 10010)
                    {
                        Log.Error($"Received error response from Deribit for request {request}: {error}");
                    }

                    response = new TResponse {Error = error};
                }

                response.ArrivalTime = arrivalTime;
                taskCompletionSource.SetResult(response);
            }
            catch (Exception e)
            {
                Log.Error(e, $"Error handling message response {message}");
                taskCompletionSource.SetException(new DeribitSendException(error, message, e));
            }
        }

        private void OnWebsocketMessage(object s, (string, DateTime) valueTuple)
        {
            var (message, arrivalTime) = valueTuple;

            try
            {
                var minResponse = SerialisationHelper.Deserialise<Response>(message);

                if (minResponse == null)
                {
                    Log.Error($"Unable to deserialise valid response from message - {message}");
                }
                // Notifications (market data, trades, etc.) are most time-sensitive, so try that first
                else if (minResponse.Method == "subscription")
                {
                    var notificationHolder = SerialisationHelper.Deserialise<NotificationHolder>(message);

                    switch (notificationHolder.Details.Channel.Substring(0, 2))
                    {
                        case "bo":
                            SendNotification(notificationHolder.Details.Data.ToObject<OrderBookNotification>(), arrivalTime);
                            break;

                        case "ti":
                            SendNotification(notificationHolder.Details.Data.ToObject<TickerNotification>(), arrivalTime);

                            break;

                        case "tr":
                            foreach (var notification in notificationHolder.Details.Data.ToObject<IList<TradeNotification>>())
                            {
                                SendNotification(notification, arrivalTime);
                            }

                            break;

                        case "us":
                            switch (notificationHolder.Details.Channel[5])
                            {
                                case 't':
                                    foreach (var notification in notificationHolder.Details.Data.ToObject<IList<UserTradeNotification>>())
                                    {
                                        SendNotification(notification, arrivalTime);
                                    }

                                    break;

                                case 'o':
                                    SendNotification(notificationHolder.Details.Data.ToObject<OrderNotification>(), arrivalTime);

                                    break;
                            }
                            break;

                        default:
                            Log.Error($"Unknown notification channel: {notificationHolder.Details.Channel}");
                            break;
                    }

                    _marketDataWatchdog.Stop();
                    _marketDataWatchdog.Start();
                }
                else if (_responseTypesByRequestId.TryRemove(minResponse.Id, out var handler))
                {
                    handler(message, minResponse.Error, arrivalTime);
                } else if (minResponse.Method == "heartbeat")
                {
                    Log.Verbose("Received test request. Responding");
                    _ = Send(TestRequest);
                }
                else
                {
                    Log.Error($"No handler registered for request with id: {minResponse.Id}, method {minResponse.Method}");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"Error processing message {message}");
            }
        }

        private void SendNotification(INotification notification, DateTime arrivalTime)
        {
            notification.ArrivalTime = arrivalTime;
            notification.Notify(this);
        }

        private void EnsureInitialised()
        {
            if (!_initialised) throw new InvalidOperationException("API is not initialised. Please call Initialise() first");
            if (_disposed) throw new ObjectDisposedException("DeribitApi", "Object has been disposed");
        }

        private async void OnWebsocketConnected(object sender, EventArgs eventArgs)
        {
            if (_activeSubscriptions.Count > 0 && !_disposed)
            {
                Log.Information($"Resubscribing to {_activeSubscriptions.Count} active subscription(s)");

                try
                {
                    _resubscribing = true;
                    foreach (var subscription in _activeSubscriptions.ToList())
                    {
                        try
                        {
                            Log.Debug($"Resubscribing to {string.Join(",", subscription.Channels)}");
                            await Send(subscription);
                        }
                        catch (Exception e)
                        {
                            Log.Error($"Error sending resubscription {subscription}", e);
                        }
                    }

                    Log.Information("Resubscription finished");
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error resubscribing");
                    _ = _webSocket.Reset();
                }
                finally
                {
                    _resubscribing = false;
                }
            }

            if (_initialised && !_disposed)
            {
                await Send(new SetHeartbeatRequest((int)HeartbeatInterval.TotalSeconds));
            }
        }
    }
}
