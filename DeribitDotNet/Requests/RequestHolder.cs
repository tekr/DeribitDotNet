using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using DeribitDotNet.Responses;
using Newtonsoft.Json;

[assembly:InternalsVisibleTo("DeribitDotNet.UnitTests")]

namespace DeribitDotNet.Requests
{
    internal class RequestHolder<TRequest, TResponse> where TResponse : Response where TRequest : Request<TResponse>
    {
        [JsonProperty("jsonrpc")]
        public static readonly string JsonRpcVersion = "2.0";

        [JsonProperty("access_token", NullValueHandling = NullValueHandling.Ignore)]
        public string AccessToken;

        public readonly long Id;

        public readonly string Method;

        [JsonProperty("params", NullValueHandling = NullValueHandling.Ignore)]
        public readonly TRequest Parameters;

        public RequestHolder(long id, TRequest request, string token)
        {
            Id = id;
            Method = $"{(request.IsPublic ? "public" : "private")}/{request.Method}";

            Parameters = request;

            if (!Parameters.IsPublic)
            {
                AccessToken = token;
            }
        }
    }

    internal static class SerialisationHelper
    {
        internal static readonly JsonSerializer Serialiser = JsonSerializer.CreateDefault(DeribitApi.SerialiserSettings);

        private static readonly StringBuilder Buffer = new StringBuilder(1024);
        private static readonly JsonTextWriter JsonWriter = new JsonTextWriter(new StringWriter(Buffer));

        static SerialisationHelper()
        {
            JsonWriter.Formatting = Serialiser.Formatting;
            Serialiser.CheckAdditionalContent = false;
        }

        public static string Serialise(object obj)
        {
            lock (JsonWriter)
            {
                Serialiser.Serialize(JsonWriter, obj, null);

                var str = Buffer.ToString();
                Buffer.Clear();

                return str;
            }
        }

        public static T Deserialise<T>(string text)
        {
            using var jsonTextReader = new JsonTextReader(new StringReader(text));
            return (T)Serialiser.Deserialize(jsonTextReader, typeof(T));
        }
    }
}