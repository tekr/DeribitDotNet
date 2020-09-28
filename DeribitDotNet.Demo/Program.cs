using System;
using System.Linq;
using System.Threading.Tasks;
using DeribitDotNet.Requests;
using DeribitDotNet.Responses;
using Serilog;
using Serilog.Events;

namespace DeribitDotNet.Demo
{
    public class Program
    {
        /// <summary>
        /// Specify [API Key] and [API secret] as command-line parameters in order to demo private API functions (including executing
        /// orders, account details, etc.). Otherwise only public API functionality will be shown
        /// </summary>
        static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration().MinimumLevel.Is(LogEventLevel.Information).WriteTo.Console().CreateLogger();

            if (args.Length < 2)
            {
                Console.WriteLine("API key and secret not specified on commandline. Only public API calls will be demonstrated.");
                Console.WriteLine();
                
                Run(null, null).Wait();
            }
            else
            {
                Run(args[0], args[1]).Wait();
            }

        }

        private static async Task Run(string key, string secret)
        {
            var deribitApi = new DeribitApi(key, secret, true);
         
            await deribitApi.Initialise();

            Console.WriteLine();

            // Send test request and show parsed response
            Console.WriteLine("Test response: " + await deribitApi.Send(new TestRequest()));
            Console.WriteLine();

            // Send time request and show parsed response
            Console.WriteLine("Time response: " + await deribitApi.Send(new TimeRequest()));
            Console.WriteLine();

            // Get all active instruments settled in BTC
            var instrumentsResponse = await deribitApi.Send(new InstrumentsRequest("BTC", InstrumentType.Future));

            Console.WriteLine(@$"Active BTC futures instruments: {string.Join(", ",
                instrumentsResponse.Instruments.Select(i => i.InstrumentName))}");

            Console.WriteLine();

            Console.WriteLine("Top 3 levels of the BTC-PERPETUAL order book will begin streaming when you press <ENTER>. Press <ENTER> again to stop.");

            Console.ReadLine();

            // Order book for processing updates received from Deribit
            var orderBook = new OrderBook(3);

            using (deribitApi.OrderBooks.Subscribe(orderBook.Update))
            {
                // Print upper order book levels on stream update
                orderBook.Quotes.Subscribe(Console.WriteLine);

                // Subscribe to BTC-PERPETUAL order book updates
                await deribitApi.Send(SubscribeRequest.OrderBook(false, "BTC-PERPETUAL"));

                // Get off the websocket thread since we're going to block
                await Task.Delay(1);

                Console.ReadLine();
            }

            Console.WriteLine("BTC & ETH perpetual futures trades will begin streaming when you press <ENTER> (note that trade frequency may be" +
                              " quite low in the test environment). Press <ENTER> again to stop.");

            Console.ReadLine();

            // Print received trades
            using (deribitApi.Trades.Subscribe(Console.WriteLine))
            {
                // Subscribe to trades for BTC and ETH perpetual contracts
                await deribitApi.Send(SubscribeRequest.Trades(false, "BTC-PERPETUAL", "ETH-PERPETUAL"));

                // Get off the websocket thread since we're going to block
                await Task.Delay(1);

                Console.ReadLine();
            }

            if (key == null)
            {
                Console.WriteLine("Private API examples are not available as no API key was specified. Press <ENTER> to complete.");
                Console.ReadLine();

                return;
            }

            Console.WriteLine("Press <ENTER> to continue.");

            // TODO Private API examples
        }
    }
}
