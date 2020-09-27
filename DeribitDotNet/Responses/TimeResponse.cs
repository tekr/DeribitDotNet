using System;
using DeribitDotNet.JsonConverters;
using Newtonsoft.Json;

namespace DeribitDotNet.Responses
{
    public class TimeResponse : Response
    {
        [JsonConverter(typeof(CountToDateTimeConverter), false)]
        [JsonProperty("result")]
        public DateTime ExchangeTime;

        public override string ToString() => $"{base.ToString()}, {nameof(ExchangeTime)}: {ExchangeTime:O}";
    }
}