using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace FirebaseREST.Auth
{
    public class ProviderUserInfo
    {
        [JsonProperty("providerId", ItemConverterType = typeof(StringEnumConverter))]
        public FirebaseAuthType Provider { get; set; }

        public string DisplayName { get; set; }
        public string PhotoUrl { get; set; }
        public string FederatedId { get; set; }
        public string Email { get; set; }
        public string RawId { get; set; }
        public string ScreenName { get; set; }
    }
}
