using DeribitDotNet.Responses;
using Newtonsoft.Json;

namespace DeribitDotNet.Requests
{
    public class AuthenticateRequest : Request<AuthenticateResponse>
    {
        [JsonProperty("grant_type")]
        public readonly string GrantType;

        [JsonProperty("client_id", NullValueHandling = NullValueHandling.Ignore)]
        public readonly string ClientId;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public readonly long Timestamp;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public readonly string Signature;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public readonly string Nonce;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public readonly string Username;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public readonly string Password;

        [JsonProperty("client_secret", NullValueHandling = NullValueHandling.Ignore)]
        public readonly string ClientSecret;

        [JsonProperty("refresh_token", NullValueHandling = NullValueHandling.Ignore)]
        public readonly string RefreshToken;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public readonly string State;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public readonly string Scope;

        public static AuthenticateRequest ClientSignature(string clientId, long timestamp, string signature, string nonce) =>
            new AuthenticateRequest("client_signature", clientId, timestamp, signature, nonce);

        public static AuthenticateRequest ClientPassword(string username, string password) =>
            new AuthenticateRequest("password", username: username, password: password);

        public static AuthenticateRequest ClientCredentials(string clientId, string secret) =>
            new AuthenticateRequest("client_credentials", clientId, clientSecret: secret);

        public static AuthenticateRequest NewToken(string refreshToken) =>
            new AuthenticateRequest("refresh_token", refreshToken: refreshToken);


        private AuthenticateRequest(string grantType = null, string clientId = null, long timestamp = default, string signature = null,
            string nonce = null, string username = null, string password = null, string clientSecret = null, string refreshToken = null,
            string state = null, string scope = null) : base("auth", true)
        {
            GrantType = grantType;
            ClientId = clientId;
            Timestamp = timestamp;
            Signature = signature;
            Nonce = nonce;
            Username = username;
            Password = password;
            ClientSecret = clientSecret;
            RefreshToken = refreshToken;
            State = state;
            Scope = scope;
        }
    }
}