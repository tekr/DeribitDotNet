using Newtonsoft.Json;

namespace DeribitDotNet.Responses
{
    public class TradesResponse : Response
    {
        [JsonProperty("result/trades")]
        public UserTrade[] Trades;
    }
}