using DeribitDotNet.Responses;
using Newtonsoft.Json;

namespace DeribitDotNet.Requests
{
    public class InstrumentsRequest : Request<InstrumentsResponse>
    {
        public string Currency;

        [JsonProperty("kind", NullValueHandling = NullValueHandling.Ignore)]
        public InstrumentType? InstrumentType;

        public readonly bool Expired;

        public InstrumentsRequest(string currency, InstrumentType? instrumentType = null, bool expired = false) : base("get_instruments",
            true)
        {
            Currency = currency;
            InstrumentType = instrumentType;
            Expired = expired;
        }
    }
}