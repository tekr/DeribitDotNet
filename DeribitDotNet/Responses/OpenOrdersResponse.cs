using Newtonsoft.Json;

namespace DeribitDotNet.Responses
{
    public class OpenOrdersResponse : Response
    {
        [JsonProperty("result")]
        public Order[] Orders;
    }
}