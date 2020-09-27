using Newtonsoft.Json;

namespace DeribitDotNet.Responses
{
    public class OrderCancelResponse : Response
    {
        [JsonProperty("result")]
        public Order Order;
    }
}