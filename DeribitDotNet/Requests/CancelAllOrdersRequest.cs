using DeribitDotNet.Responses;
using Newtonsoft.Json;

namespace DeribitDotNet.Requests
{
    public class CancelAllOrdersRequest : Request<Response>
    {
        [JsonProperty("instrument_name", NullValueHandling = NullValueHandling.Ignore)]
        public string Instrument;

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public OrderType? OrderType;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public InstrumentKind? InstrumentKind;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DeribitCurrency? Currency;

        public CancelAllOrdersRequest() : base("cancel_all", false)
        {
        }

        public CancelAllOrdersRequest(DeribitCurrency currency, InstrumentKind? instrumentKind = null,
            OrderType orderType = Requests.OrderType.Any) : base("cancel_all_by_currency", false)
        {
            Currency = currency;
            InstrumentKind = instrumentKind;
            OrderType = AnyAsNull(orderType);
        }

        public CancelAllOrdersRequest(string instrument, OrderType orderType = Requests.OrderType.Any) : base(
            "cancel_all_by_instrument", false)
        {
            Instrument = instrument;
            OrderType = AnyAsNull(orderType);
        }
    }
}