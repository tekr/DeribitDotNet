using System;
using Newtonsoft.Json;

namespace DeribitDotNet.JsonConverters
{
    internal class CountToDateTimeConverter : JsonConverter
    {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1);

        private readonly bool _isMicroseconds;

        public CountToDateTimeConverter(bool isMicroseconds)
        {
            _isMicroseconds = isMicroseconds;
        }

        public override bool CanConvert(Type objectType) => objectType == typeof(long) || objectType == typeof(int) ||
                                                            objectType == typeof(DateTime) || objectType == typeof(DateTime?);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serialiser) =>
            Epoch.AddTicks(Convert.ToInt64(reader.Value) * (TimeSpan.TicksPerMillisecond / (_isMicroseconds ? 1000 : 1)));

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serialiser)
        {
            if (value == null) writer.WriteNull();
            else writer.WriteValue(Convert.ToInt64(((DateTime)value - Epoch).TotalMilliseconds * (_isMicroseconds ? 1000 : 1)));
        }
    }
}