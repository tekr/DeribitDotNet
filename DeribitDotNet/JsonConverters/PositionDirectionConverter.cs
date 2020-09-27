using System;
using DeribitDotNet.Responses;
using Newtonsoft.Json;

namespace DeribitDotNet.JsonConverters
{
    /// <summary>
    /// Handles conversion of the direction string returned from Deribit, since API has undocumented behaviour of returning 'zero'
    /// when position size is 0.
    /// </summary>
    internal class PositionDirectionConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(Direction);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serialiser)
        {
            var stringValue = reader.Value.ToString();
            return stringValue == "zero" ? Direction.Buy : Enum.Parse<Direction>(stringValue, true);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serialiser) => throw new NotImplementedException();
    }
}