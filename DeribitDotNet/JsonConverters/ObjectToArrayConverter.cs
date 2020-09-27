using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace DeribitDotNet.JsonConverters
{
    /// <summary>
    /// Adapted from https://stackoverflow.com/questions/39461518/c-sharp-json-net-deserialize-response-that-uses-an-unusual-data-structure
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ObjectToArrayConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(T) == objectType;
        }

        static bool ShouldSkip(JsonProperty property)
        {
            return property.Ignored || !property.Readable || !property.Writable;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var type = value.GetType();

            if (!(serializer.ContractResolver.ResolveContract(type) is JsonObjectContract contract))
            {
                throw new JsonSerializationException("invalid type " + type.FullName);
            }
        
            var list = contract.Properties.Where(p => !ShouldSkip(p)).Select(p => p.ValueProvider.GetValue(value));
            serializer.Serialize(writer, list);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }
        
            var token = JArray.Load(reader);

            if (!(serializer.ContractResolver.ResolveContract(objectType) is JsonObjectContract contract))
            {
                throw new JsonSerializationException("invalid type " + objectType.FullName);
            }
        
            var value = existingValue ?? contract.DefaultCreator();
        
            foreach (var pair in contract.Properties.Where(p => !ShouldSkip(p)).Zip(token, (p, v) => new { Value = v, Property = p }))
            {
                var propertyValue = pair.Value.ToObject(pair.Property.PropertyType, serializer);
                pair.Property.ValueProvider.SetValue(value, propertyValue);
            }

            return value;
        }
    }
}