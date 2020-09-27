using DeribitDotNet.Responses;
using Newtonsoft.Json;

namespace DeribitDotNet.Requests
{
    public class PositionsRequest : Request<PositionsResponse>
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DeribitCurrency Currency;

        [JsonProperty("kind", NullValueHandling = NullValueHandling.Ignore)]
        public InstrumentKind? InstrumentKind;

        public PositionsRequest(DeribitCurrency currency, InstrumentKind instrumentKind = Requests.InstrumentKind.Any) : base("get_positions", false)
        {
            Currency = currency;
            InstrumentKind = AnyAsNull(instrumentKind);
        }
    }
}