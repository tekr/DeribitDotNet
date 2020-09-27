using System;
using Newtonsoft.Json;

namespace DeribitDotNet.JsonConverters
{
    // Converter that does not append ".0" to the end of whole numbers stored in float values
    // Deribit complains about an incorrect signature on private API calls otherwise
    internal class FloatJsonConverter : JsonConverter
    {
        public override bool CanRead => false;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) =>
            throw new NotImplementedException();

        public override bool CanConvert(Type objectType) =>
            objectType == typeof(double) || objectType == typeof(float) || objectType == typeof(decimal)
            || objectType == typeof(double?) || objectType == typeof(float?) || objectType == typeof(decimal?);

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null) writer.WriteNull();
            else writer.WriteRawValue(IsWholeValue(value)
                ? JsonConvert.ToString(Convert.ToInt64(value))
                : JsonConvert.ToString(value));
        }

        private static bool IsWholeValue(object value)
        {
            switch (value)
            {
                case decimal decimalValue:
                    return decimalValue == Math.Truncate(decimalValue);
                case float _:
                case double _:
                    var doubleValue = (double)value;
                    return doubleValue == Math.Truncate(doubleValue);
            }

            return false;
        }
    }
}