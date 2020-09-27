using DeribitDotNet.Responses;
using Newtonsoft.Json;

namespace DeribitDotNet.Requests
{
    public class AmendOrderRequest : Request<OrderOperationResponse>
    {
        [JsonProperty("order_id")]
        public readonly string OrderId;

        [JsonProperty("amount")]
        public double Quantity;

        public double Price;

        [JsonProperty("post_only")]
        public bool PostOnly;

        [JsonProperty("stop_price", NullValueHandling = NullValueHandling.Ignore)]
        public double? StopPrice;

        public AmendOrderRequest(string orderId) : base("edit", false) => OrderId = orderId;

        public override string ToString() => $"AMEND: {base.ToString()}, {nameof(OrderId)}: {OrderId}, {nameof(Quantity)}: {Quantity}, " +
                                             $"{nameof(Price)}: {Price}, {nameof(PostOnly)}: {PostOnly}, {nameof(StopPrice)}: {StopPrice}";
    }
}