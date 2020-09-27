using Newtonsoft.Json;

namespace DeribitDotNet.Responses
{
    public class CancelOrderResponse : Response
    {
        [JsonProperty("result")]
        public Order Order;
    }
}