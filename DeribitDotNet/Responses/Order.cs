using System;
using DeribitDotNet.JsonConverters;
using DeribitDotNet.Requests;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DeribitDotNet.Responses
{
    [JsonConverter(typeof(StringEnumConverter), true)]
    public enum OrderStatus
    {
        Open,
        Filled,
        Cancelled
    }

    public class Order
    {
        [JsonProperty("amount")]
        public double Quantity;

        [JsonProperty("api")]
        public bool IsApiOrder;

        [JsonProperty("average_price")]
        public double AvgPrice;

        public double Commission;

        [JsonProperty("creation_timestamp")]
        [JsonConverter(typeof(CountToDateTimeConverter), false)]
        public DateTime CreatedTime;

        public Direction Direction;

        [JsonProperty("filled_amount")]
        public double FilledQuantity;

        [JsonProperty("instrument_name")]
        public string Instrument;

        [JsonProperty("is_liquidation")]
        public bool IsLiquidation;

        public string Label;

        [JsonProperty("last_update_timestamp")]
        [JsonConverter(typeof(CountToDateTimeConverter), false)]
        public DateTime LastUpdateTime;

        [JsonProperty("max_show")]
        public int ShowSize;

        [JsonProperty("order_id")]
        public string OrderId;

        [JsonProperty("order_state")]
        public OrderStatus Status;

        [JsonProperty("order_type")]
        public OrderType OrderType;

        [JsonProperty("original_order_type")]
        public OrderType OriginalOrderType;

        [JsonProperty("post_only")]
        public bool PostOnly;

        public double Price;

        [JsonProperty("profit_loss")]
        public double Pnl;

        [JsonProperty("reduce_only")]
        public bool ReduceOnly;

        [JsonProperty("stop_price")]
        public double? StopPrice;

        [JsonProperty("time_in_force")]
        public TimeInForce TimeInForce;

        [JsonProperty("trigger")]
        public StopTrigger? StopTrigger;

        [JsonProperty("triggered")]
        public bool HasTriggered;

        public override string ToString() =>
            $"{nameof(Quantity)}: {Quantity}, {nameof(IsApiOrder)}: {IsApiOrder}, {nameof(AvgPrice)}: {AvgPrice}, " +
            $"{nameof(Commission)}: {Commission}, {nameof(CreatedTime)}: {CreatedTime}, {nameof(Direction)}: " +
            $"{Direction}, {nameof(FilledQuantity)}: {FilledQuantity}, {nameof(Instrument)}: {Instrument}, {nameof(IsLiquidation)}:" +
            $"{IsLiquidation}, {nameof(Label)}: {Label}, {nameof(LastUpdateTime)}: {LastUpdateTime}, {nameof(ShowSize)}: " +
            $"{ShowSize}, {nameof(OrderId)}: {OrderId}, {nameof(Status)}: {Status}, {nameof(OrderType)}: {OrderType}, " +
            $"{nameof(OriginalOrderType)}: {OriginalOrderType}, {nameof(PostOnly)}: {PostOnly}, {nameof(Price)}: {Price}, " +
            $"{nameof(Pnl)}: {Pnl}, {nameof(ReduceOnly)}: {ReduceOnly}, {nameof(StopPrice)}: {StopPrice}, {nameof(TimeInForce)}: " +
            $"{TimeInForce}, {nameof(StopTrigger)}: {StopTrigger}, {nameof(HasTriggered)}: {HasTriggered}";
    }
}