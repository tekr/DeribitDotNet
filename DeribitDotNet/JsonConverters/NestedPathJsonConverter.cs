using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DeribitDotNet.JsonConverters
{
    // Modified from examples at https://gist.github.com/lucd/cdd57a2602bd975ec0a6
    public class NestedPathJsonConverter<T> : JsonConverter where T : new()
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(T);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serialiser)
        {
            var result = new T();
            var jObject = JObject.Load(reader);

            foreach (var field in result.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                var propertyName = GetJsonPropertyName(field);
                var pathSteps = propertyName.Split('/');

                var token = pathSteps.Aggregate<string, JToken>(jObject, (current, step) => current?[step]);
                object newFieldValue;

                if (field.GetCustomAttributes(false).FirstOrDefault(a => a is JsonConverterAttribute) is JsonConverterAttribute
                    jsonConverterAttribute)
                {
                    var converter = (JsonConverter)Activator.CreateInstance(jsonConverterAttribute.ConverterType,
                        jsonConverterAttribute.ConverterParameters);

                    var reader2 = token.CreateReader();
                    reader2.Read();
                    newFieldValue = converter.ReadJson(reader2, field.FieldType, null, serialiser);
                }
                else
                {
                    newFieldValue = token?.ToObject(field.FieldType);
                }

                if (newFieldValue != null)
                {
                    field.SetValue(result, newFieldValue is long && field.FieldType == typeof(int)
                        ? Convert.ToInt32(newFieldValue)
                        : newFieldValue);
                }
            }

            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serialiser) =>
            throw new NotImplementedException();

        private static string GetJsonPropertyName(MemberInfo memberInfo)
        {
            string name;

            var jsonPropertyAttribute = memberInfo.GetCustomAttributes(false).FirstOrDefault(a => a is JsonPropertyAttribute);
            if (jsonPropertyAttribute != null)
            {
                name = ((JsonPropertyAttribute)jsonPropertyAttribute).PropertyName;
            }
            else
            {
                // Lower-case first letter
                var charArray = memberInfo.Name.ToCharArray();
                charArray[0] = char.ToLower(charArray[0]);
                name = new string(charArray);
            }

            return name;
        }
    }
}