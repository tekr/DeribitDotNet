using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using Serilog.Events;

namespace DeribitDotNet.Utils
{
    public class ResilientWebSocket : IDisposable
    {
        private const int CloseTimeoutMs = 5000;
        private const int OpenTimeoutMs = 5000;
        private const int SendTimeoutMs = 5000;

        private const int RetryOpenDelayMs = 5000;

        private readonly AsyncManualResetEvent _connectionWaitHandle = new AsyncManualResetEvent();

        private ClientWebSocket _webSocket;
        private int _reconnecting;
        private bool _disposed;

        private readonly Uri _url;

        private byte[] _writeBuffer = new byte[4096];
        private byte[] _readBuffer = new byte[262144];

        public bool IsConnected => _connectionWaitHandle.WaitTask().IsCompleted;

        public event EventHandler Connected;
        public event EventHandler Closed;
        public event EventHandler<ValueTuple<string, DateTime>> MessageReceived;

        public ResilientWebSocket(string url) => _url = new Uri(url);

        public async ValueTask Initialise()
        {
            if (_disposed) throw new ObjectDisposedException("ResilientWebSocket", "Object has been disposed");
            await Reconnect().ConfigureAwait(false);
        }

        public async Task Send(string message)
        {
            if (Log.IsEnabled(LogEventLevel.Verbose))
            {
                Log.Verbose($"Sending message: {message}");
            }

            var errorCounter = 0;

            do
            {
                if (_disposed) throw new ObjectDisposedException("ResilientWebSocket", "Object has been disposed");

                try
                {
                    int length;

                    unsafe
                    {
                        fixed (char* pMessage = message)
                        fixed (byte* pBuffer = _writeBuffer)
                        {
                            // Will throw if buffer is too small
                            length = Encoding.Default.GetBytes(pMessage, message.Length, pBuffer, _writeBuffer.Length);
                        }
                    }

                    if (errorCounter > 0)
                    {
                        Log.Debug($"About to send length: {length} Message: {message}");
                    }

                    var sendTask = _webSocket.SendAsync(new ArraySegment<byte>(_writeBuffer, 0, length), WebSocketMessageType.Text, true,
                        CancellationToken.None);

                    if (errorCounter > 0)
                    {
                        Log.Debug($"Send task: {sendTask}. About to await with timeout");
                    }

                    await sendTask.TimeoutAfter(SendTimeoutMs).ConfigureAwait(false);

                    if (errorCounter > 0)
                    {
                        Log.Debug($"Awaited Send successfully");
                    }

                    break;
                }
                catch (ArgumentException ae) when (ae.ParamName == "bytes")
                {
                    Log.Warning($"Write buffer size ({_writeBuffer.Length}) too small for message. Doubling..");
                    _writeBuffer = new byte[_writeBuffer.Length * 2];
                }
                catch (Exception e)
                {
                    Log.Error(e, $"Error writing message {message} to web socket");

                    if (_webSocket == null)
                    {
                        Log.Debug("Websocket was null. Initiating reconnection");
                        await Reconnect().ConfigureAwait(false);
                    }

                    if (errorCounter++ >= 10)
                    {
                        Log.Debug($"Error counter has reached 10. Reconnecting..");
                        await Reconnect().ConfigureAwait(false);
                    }

                    await Task.Delay(200).ConfigureAwait(false);
                }

            } while (true);
        }

        public Task Reset()
        {
            if (_disposed) return Task.CompletedTask;

            if (!IsConnected)
            {
                Log.Debug("Reset() called when web socket is not connected. Ignoring.");
                return _connectionWaitHandle.WaitTask();
            }

            Log.Information("Resetting websocket connection");
            return Reconnect();
        }

        public Task ConnectionReadyAsync() => _connectionWaitHandle.WaitTask();

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;

                Log.Debug("Disposing web socket");
                EnsureClosed().Wait();
            }
        }

        private async Task Reconnect()
        {
            if (_disposed) return;

            if (Interlocked.CompareExchange(ref _reconnecting, 1, 0) == 0)
            {
                Log.Debug($"Initiating web socket reconnection to {_url}");

                try
                {
                    _connectionWaitHandle.Reset();

                    while (!IsConnected)
                    {
                        try
                        {
                            await EnsureClosed().ConfigureAwait(false);

                            _webSocket = new ClientWebSocket
                            {
                                Options = {KeepAliveInterval = TimeSpan.FromSeconds(30)}
                            };

                            Log.Information($"Opening connection to {_url}");

                            await _webSocket.ConnectAsync(_url, CancellationToken.None).TimeoutAfter(OpenTimeoutMs).ConfigureAwait(false);

                            Log.Debug("Web socket opened");
                            _connectionWaitHandle.Set();

                            _ = Task.Run(() => ReceiveLoop(_webSocket));
                        }
                        catch (Exception e)
                        {
                            Log.Error(e, $"Error reconnecting to {_url}");
                        }

                        if (!IsConnected)
                        {
                            Log.Information("Connection unsuccessful. Sleeping for 5 seconds before retrying..");
                            await Task.Delay(RetryOpenDelayMs).ConfigureAwait(false);
                        }
                    }
                }
                finally
                {
                    _reconnecting = 0;
                }

                try
                {
                    Connected?.Invoke(this, EventArgs.Empty);
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Error in connection opened event handler");
                }
            }
            else
            {
                Log.Debug("Reconnection already in progress. Ignoring Reconnect request");
            }
        }

        private async void ReceiveLoop(WebSocket webSocket)
        {
            try
            {
                var offset = 0;

                while (webSocket != null && webSocket.State != WebSocketState.Closed && webSocket.State != WebSocketState.Aborted && !_disposed)
                {
                    var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(_readBuffer, offset, _readBuffer.Length - offset),
                        CancellationToken.None).ConfigureAwait(false);

                    var dateTime = DateTime.UtcNow;

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await EnsureClosed().ConfigureAwait(false);
                    }
                    else if (result.CloseStatus == null)
                    {
                        if (result.EndOfMessage)
                        {
                            var message = Encoding.Default.GetString(_readBuffer, 0, offset + result.Count);

                            if (Log.IsEnabled(LogEventLevel.Verbose))
                            {
                                Log.Verbose($"Message received: {message}");
                            }

                            try
                            {
                                MessageReceived?.Invoke(this, (message, dateTime));
                            }
                            catch (Exception e)
                            {
                                Log.Error(e, "Error in message received event handler");
                            }

                            offset = 0;
                        }
                        else
                        {
                            offset += result.Count;

                            if (offset >= _readBuffer.Length - 1)
                            {
                                Log.Warning($"Read buffer size ({_readBuffer.Length}) too small for message. Doubling..");
                                var newReadBuffer = new byte[_readBuffer.Length * 2];
                                Array.Copy(_readBuffer, newReadBuffer, offset);

                                _readBuffer = newReadBuffer;
                            }
                        }
                    }
                    else
                    {
                        Log.Debug($"Web socket closed. Status: {result.CloseStatus} Reason: {result.CloseStatusDescription}");

                        Closed?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "Web socket error during receive");
            }

            await Reconnect().ConfigureAwait(false);
        }

        private async Task EnsureClosed()
        {
            try
            {
                if (_webSocket != null && _webSocket.State != WebSocketState.Aborted && _webSocket.State != WebSocketState.CloseReceived && _webSocket.State != WebSocketState.Closed)
                {
                    Log.Debug($"Closing web socket in state {_webSocket.State}");
                    await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None).TimeoutAfter(CloseTimeoutMs);
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "Error closing web socket");
            }

            _webSocket = null;
        }
    }
}
