using System;
using DeribitDotNet.JsonConverters;
using Newtonsoft.Json;

namespace DeribitDotNet.Responses
{
    public class MinResponse
    {
        [JsonIgnore]
        public bool Succeeded => Error == null;

        public string Method;

        public int Id;

        public ErrorResponse Error;
    }

    public class Response : MinResponse
    {
        public DateTime ArrivalTime { get; internal set; }

        [JsonProperty("usIn")]
        [JsonConverter(typeof(CountToDateTimeConverter), true)]
        public DateTime ExchangeTimeIn;

        [JsonProperty("usDiff")]
        public long ProcessingTimeUs;

        public override string ToString() =>
            $@"{nameof(Id)}: {Id}, {(Error != null ? string.Format($"{nameof(Error)}: {Error}") : $"{nameof(ExchangeTimeIn)}: " +
            $"{ExchangeTimeIn:O}, {nameof(ProcessingTimeUs)}: {ProcessingTimeUs}")}, {nameof(ArrivalTime)}: {ArrivalTime:O}";
    }
}