using Newtonsoft.Json;

namespace DeribitDotNet.Responses
{
    public class UserTrade : Trade
    {
        public double Fee;

        [JsonProperty("fee_currency")]
        public string FeeCurrency;

        public string Label;

        [JsonProperty("liquidity")]
        public MakerTaker Liquidity;

        [JsonProperty("matching_id")]
        public string SelfTradeMatchingId;

        [JsonProperty("order_id")]
        public string OrderId;

        [JsonProperty("order_type")]
        public TradeOrderType OrderType;

        [JsonProperty("self_trade")]
        public bool IsSelfTrade;

        [JsonProperty("state")]
        public TradeOrderState OrderState;

        public override string ToString() =>
            $"{base.ToString()}, {nameof(Fee)}: {Fee}, {nameof(FeeCurrency)}: {FeeCurrency}, {nameof(Label)}: {Label}, " +
            $"{nameof(Liquidity)}: {Liquidity}, {nameof(SelfTradeMatchingId)}: {SelfTradeMatchingId}, {nameof(OrderId)}: " +
            $"{OrderId}, {nameof(OrderType)}: {OrderType}, {nameof(IsSelfTrade)}: {IsSelfTrade}, {nameof(OrderState)}: " +
            $"{OrderState}";
    }
}