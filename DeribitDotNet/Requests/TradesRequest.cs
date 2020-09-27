using System;
using System.Runtime.Serialization;
using DeribitDotNet.JsonConverters;
using DeribitDotNet.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DeribitDotNet.Requests
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SortOrder
    {
        [EnumMember(Value = "asc")]
        Ascending,
        [EnumMember(Value = "desc")]
        Descending
    }

    public class TradesRequest : Request<TradesResponse>
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DeribitCurrency? Currency;

        [JsonProperty("instrument_name", NullValueHandling = NullValueHandling.Ignore)]
        public string Instrument;

        [JsonProperty("kind", NullValueHandling = NullValueHandling.Ignore)]
        public InstrumentKind? InstrumentKind;

        [JsonConverter(typeof(CountToDateTimeConverter), false),
         JsonProperty("start_timestamp", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? StartTime;

        [JsonConverter(typeof(CountToDateTimeConverter), false),
         JsonProperty("end_timestamp", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? EndTime;

        [JsonProperty("count", NullValueHandling = NullValueHandling.Ignore)]
        public int? MaxCount;

        [JsonProperty("include_old", NullValueHandling = NullValueHandling.Ignore)]
        public bool IncludeOld;

        [JsonProperty("sorting")]
        public SortOrder? SortOrder;

        [JsonProperty("start_id", NullValueHandling = NullValueHandling.Ignore)]
        public string StartTradeId;

        [JsonProperty("end_id", NullValueHandling = NullValueHandling.Ignore)]
        public string EndTradeId;

        [JsonProperty("start_seq", NullValueHandling = NullValueHandling.Ignore)]
        public long? StartSeq;

        [JsonProperty("end_seq", NullValueHandling = NullValueHandling.Ignore)]
        public long? EndSeq;

        public TradesRequest(DeribitCurrency currency, InstrumentKind instrumentKind = Requests.InstrumentKind.Any, string startTradeId = null,
            string endTradeId = null, int? maxCount = null, bool includeOld = true, SortOrder sortOrder = Requests.SortOrder.Ascending) : base(
            "get_user_trades_by_currency", false)
        {
            Currency = currency;
            InstrumentKind = AnyAsNull(instrumentKind);
            StartTradeId = startTradeId;
            EndTradeId = endTradeId;
            MaxCount = maxCount;
            IncludeOld = includeOld;
            SortOrder = sortOrder;
        }

        public TradesRequest(DeribitCurrency currency, DateTime startTime, DateTime endTime,
            InstrumentKind instrumentKind = Requests.InstrumentKind.Any, int? maxCount = null, bool includeOld = true,
            SortOrder sortOrder = Requests.SortOrder.Ascending) : base("get_user_trades_by_currency_and_time", false)
        {
            Currency = currency;
            StartTime = startTime;
            EndTime = endTime;
            InstrumentKind = AnyAsNull(instrumentKind);
            MaxCount = maxCount;
            IncludeOld = includeOld;
            SortOrder = sortOrder;
        }

        public TradesRequest(string instrument, long? startSeq = null, long? endSeq = null, int? maxCount = null, bool includeOld = true,
            SortOrder sortOrder = Requests.SortOrder.Ascending) : base("get_user_trades_by_instrument", false)
        {
            Instrument = instrument;
            StartSeq = startSeq;
            EndSeq = endSeq;
            MaxCount = maxCount;
            IncludeOld = includeOld;
            SortOrder = sortOrder;
        }

        public TradesRequest(string instrument, DateTime startTime, DateTime endTime, int? maxCount = null, bool includeOld = true,
            SortOrder sortOrder = Requests.SortOrder.Ascending) : base("get_user_trades_by_instrument_and_time", false)
        {
            Instrument = instrument;
            StartTime = startTime;
            EndTime = endTime;
            MaxCount = maxCount;
            IncludeOld = includeOld;
            SortOrder = sortOrder;
        }
    }
}