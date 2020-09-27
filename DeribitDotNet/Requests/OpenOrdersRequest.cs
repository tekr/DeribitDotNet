using DeribitDotNet.Responses;
using Newtonsoft.Json;

namespace DeribitDotNet.Requests
{
    public class OpenOrdersRequest : Request<OpenOrdersResponse>
    {
        [JsonProperty("instrument_name", NullValueHandling = NullValueHandling.Ignore)]
        public string Instrument;

        [JsonProperty("kind", NullValueHandling = NullValueHandling.Ignore)]
        public InstrumentKind? InstrumentKind;

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public OrderType? OrderType;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DeribitCurrency? Currency;

        public OpenOrdersRequest(DeribitCurrency currency, InstrumentKind instrumentKind = Requests.InstrumentKind.Any,
            OrderType orderType = Requests.OrderType.Any) : base("get_open_orders_by_currency", false)
        {
            Currency = currency;
            InstrumentKind = AnyAsNull(instrumentKind);
            OrderType = AnyAsNull(orderType);
        }

        public OpenOrdersRequest(string instrument, OrderType orderType = Requests.OrderType.Any) : base("get_open_orders_by_instrument",
            false)
        {
            Instrument = instrument;
            OrderType = AnyAsNull(orderType);
        }
    }
}