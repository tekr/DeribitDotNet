using System.Runtime.Serialization;
using DeribitDotNet.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DeribitDotNet.Requests
{
    [JsonConverter(typeof(StringEnumConverter), true)]
    public enum OrderType
    {
        // Limit (default) must be first
        Limit,
        Market,
        [EnumMember(Value = "stop_limit")]
        StopLimit,
        [EnumMember(Value = "stop_market")]
        StopMarket,
        Any
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum TimeInForce
    {
        // GTC (default) must be first
        [EnumMember(Value = "good_til_cancelled")]
        GoodTillCancelled,
        [EnumMember(Value = "fill_or_kill")]
        FillOrKill,
        [EnumMember(Value = "immediate_or_cancel")]
        ImmediateOrCancel,
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum StopTrigger
    {
        [EnumMember(Value = "last_price")]
        LastPrice,
        [EnumMember(Value = "mark_price")]
        MarkPrice,
        [EnumMember(Value = "index_price")]
        IndexPrice,
    }

    public class NewOrderRequest : Request<OrderOperationResponse>
    {
        [JsonProperty("instrument_name")]
        public readonly string Instrument;

        [JsonProperty("amount")]
        public double Quantity;

        [JsonProperty("type", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public OrderType OrderType;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public double? Price;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Label; // Max 32 chars

        [JsonProperty("time_in_force", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public TimeInForce TimeInForce = TimeInForce.GoodTillCancelled;

        [JsonProperty("max_show", NullValueHandling = NullValueHandling.Ignore)]
        public int? ShowSize;

        [JsonProperty("post_only", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool PostOnly;

        [JsonProperty("reduce_only", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool ReduceOnly;

        [JsonProperty("stop_price", NullValueHandling = NullValueHandling.Ignore)]
        public double? StopPrice;

        [JsonProperty("trigger", NullValueHandling = NullValueHandling.Ignore)]
        public StopTrigger? StopTrigger;

        public NewOrderRequest(string instrument, Direction direction) : base(direction == Direction.Buy ? "buy" : "sell", false) =>
            Instrument = instrument;

        public override string ToString() =>
            $"NEW: {nameof(Instrument)}: {Instrument}, {nameof(Quantity)}: {Quantity}, {nameof(OrderType)}: {OrderType}, {nameof(Price)}: " +
            $"{Price}, {nameof(Label)}: {Label}, {nameof(TimeInForce)}: {TimeInForce}, {nameof(ShowSize)}: {ShowSize}, " +
            $"{nameof(PostOnly)}: {PostOnly}, {nameof(ReduceOnly)}: {ReduceOnly}, {nameof(StopPrice)}: {StopPrice}, " +
            $"{nameof(StopTrigger)}: {StopTrigger}";
    }
}