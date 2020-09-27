using System.Collections.Generic;
using Newtonsoft.Json;

namespace DeribitDotNet.Responses
{
    public class OrderOperationResponse : Response
    {
        [JsonProperty("result/order")]
        public Order Order;

        [JsonProperty("result/trades")]
        public UserTrade[] Trades;

        public override string ToString() => $"{base.ToString()}, {nameof(Order)}: [{Order}], {nameof(Trades)}: " +
                                             $"[{(Trades != null ? string.Join("],[", (IEnumerable<UserTrade>)Trades) : string.Empty)}]";
    }
}