using DeribitDotNet.Responses;
using Newtonsoft.Json;

namespace DeribitDotNet.Requests
{
    public class CancelOrderRequest : Request<CancelOrderResponse>
    {
        [JsonProperty("order_id")]
        public readonly string OrderId;

        public CancelOrderRequest(string orderId) : base("cancel", false) => OrderId = orderId;

        public override string ToString() => $"CANCEL: {base.ToString()}, {nameof(OrderId)}: {OrderId}";
    }
}