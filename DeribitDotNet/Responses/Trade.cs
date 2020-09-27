using System;
using System.Runtime.Serialization;
using DeribitDotNet.JsonConverters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DeribitDotNet.Responses
{
    [JsonConverter(typeof(StringEnumConverter), true)]
    public enum Direction
    {
        Buy,
        Sell
    }

    [JsonConverter(typeof(StringEnumConverter), true)]
    public enum TradeDirection
    {
        Buy,
        Sell,
        Liquidation
    }

    [JsonConverter(typeof(StringEnumConverter), true)]
    public enum MakerTaker
    {
        [EnumMember(Value = "M")] Maker,
        [EnumMember(Value = "T")] Taker,
        [EnumMember(Value = "MT")] MakerAndTaker,
    }

    public enum TickDirection
    {
        PlusTick = 0,
        ZeroPlusTick = 1,
        MinusTick = 2,
        ZeroMinusTick = 3
    }

    [JsonConverter(typeof(StringEnumConverter), true)]
    public enum TradeOrderType
    {
        None,
        Limit,
        Market,
        Liquidation
    }

    [JsonConverter(typeof(StringEnumConverter), true)]
    public enum TradeOrderState
    {
        None,
        Open,
        Filled,
        Rejected,
        Cancelled,
        Untriggered,
        Archive
    }

    public class Trade
    {
        [JsonProperty("amount")]
        public double Quantity;

        public TradeDirection Direction;

        [JsonProperty("index_price")]
        public double IndexPrice;

        [JsonProperty("instrument_name")]
        public string Instrument;

        [JsonProperty("liquidation")]
        public MakerTaker? LiquidationSide;

        public double Price;

        [JsonProperty("tick_direction")]
        public TickDirection TickDirection;

        [JsonProperty("timestamp")]
        [JsonConverter(typeof(CountToDateTimeConverter), false)]
        public DateTime DateTime;

        [JsonProperty("trade_id")]
        public string TradeId;

        [JsonProperty("trade_seq")]
        public long TradeSequenceNumber;

        public override string ToString() =>
            $"{nameof(Quantity)}: {Quantity}, {nameof(Direction)}: {Direction}, {nameof(IndexPrice)}: {IndexPrice}, {nameof(Instrument)}: " +
            $"{Instrument}, {nameof(LiquidationSide)}: {LiquidationSide}, {nameof(Price)}: {Price}, {nameof(TickDirection)}: " +
            $"{TickDirection}, {nameof(DateTime)}: {DateTime}, {nameof(TradeId)}: {TradeId}, {nameof(TradeSequenceNumber)}: " +
            $"{TradeSequenceNumber}";
    }
}