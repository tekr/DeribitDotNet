using Newtonsoft.Json;

namespace DeribitDotNet.Responses
{
    public class AuthenticateResponse : Response
    {
        public ResponseData Result;

        public class ResponseData
        {
            [JsonProperty("access_token")]
            public string AccessToken;

            [JsonProperty("refresh_token")]
            public string RefreshToken;

            [JsonProperty("expires_in")]
            public int ExpiresInSec;

            public string Scope;

            public string State;

            [JsonProperty("token_type")]
            public string TokenType;
        }
    }
}