using System.Linq;
using DeribitDotNet.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DeribitDotNet.Requests
{
    [JsonConverter(typeof(StringEnumConverter), true)]
    public enum InstrumentKind
    {
        Any,
        Future,
        Option,
    }

    [JsonConverter(typeof(StringEnumConverter), true)]
    public enum DeribitCurrency
    {
        Btc,
        Eth
    }

    public class SubscribeRequest : Request<SubscribeInstrumentsResponse>
    {
        public string[] Channels;

        public static SubscribeRequest OrderBook(bool batched = false, params string[] symbols) =>
            new SubscribeRequest("book", batched, true, symbols);

        public static SubscribeRequest Trades(bool batched = false, params string[] symbols) =>
            new SubscribeRequest("trades", batched, true, symbols);

        public static SubscribeRequest Funding(bool batched = false, params string[] symbols) =>
            new SubscribeRequest("perpetual", batched, true, symbols);

        public static SubscribeRequest Ticker(bool batched = false, params string[] symbols) =>
            new SubscribeRequest("ticker", batched, true, symbols);

        public static SubscribeRequest Orders(bool batched = false, params string[] symbols) =>
            new SubscribeRequest("user.orders", batched, false, symbols);

        public static SubscribeRequest Orders(InstrumentKind instrumentKind = InstrumentKind.Any,
            DeribitCurrency? currency = null, bool batched = false) =>
            new SubscribeRequest("user.orders", instrumentKind, currency, batched);

        public static SubscribeRequest UserTrades(bool batched = false, params string[] symbols) =>
            new SubscribeRequest("user.trades", batched, false, symbols);

        public static SubscribeRequest UserTrades(InstrumentKind instrumentKind = InstrumentKind.Any,
            DeribitCurrency? currency = null, bool batched = false) =>
            new SubscribeRequest("user.trades", instrumentKind, currency, batched);

        private SubscribeRequest(string eventType, bool batched, bool isPublic, params string[] symbols) : base("subscribe", isPublic) =>
            Channels = symbols.Select(s => $"{eventType}.{s}.{GetBatched(batched)}").ToArray();

        private SubscribeRequest(string eventType, InstrumentKind instrumentKind, DeribitCurrency? currency, bool batched) :
            base("subscribe", false) => Channels = new[]
            {$"{eventType}.{instrumentKind}.{(currency != null ? currency.ToString() : "any")}.{GetBatched(batched)}"};

        private static string GetBatched(bool batched) => batched ? "100ms" : "raw";
    }
}