using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Firebase.Auth.REST
{
    /// <summary>
    ///     Basic information about the logged in user.
    /// </summary>
    public class User
    {
        /// <summary>
        ///     Gets or sets the local id.
        /// </summary>
        [JsonProperty("localId", DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue("")]
        public string LocalId { get; set; }

        /// <summary>
        ///     Gets or sets the display name.
        /// </summary>
        [JsonProperty("displayName", DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue("")]
        public string DisplayName { get; set; }

        /// <summary>
        ///     Gets or sets the email.
        /// </summary>
        [JsonProperty("email", DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue("")]
        public string Email { get; set; }

        /// <summary>
        ///     Gets or sets the email verfication status.
        /// </summary>
        [JsonProperty("emailVerified", DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue(false)]
        public bool IsEmailVerified { get; set; }

        /// <summary>
        ///     Gets or sets the photo url.
        /// </summary>
        [JsonProperty("photoUrl", DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue("")]
        public string PhotoUrl { get; set; }

        /// <summary>
        ///     Gets or sets the phone number.
        /// </summary>
        [JsonProperty("phoneNumber", DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue("")]
        public string PhoneNumber { get; set; }

        /// <summary>
        ///     Gets or sets the auth providers linked to the user.
        ///     List of all linked provider objects which contain "providerId" and "federatedId".
        /// </summary>
        [JsonProperty("phoneNumber", DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue("")]
        public List<ProviderUserInfo> Providers { get; set; }

        /// <summary>
        ///     Hash version of password.
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        ///     The timestamp, in milliseconds, that the account password was last changed.
        /// </summary>
        public long PasswordUpdatedAt { get; set; }

        /// <summary>
        ///     The timestamp, in seconds, which marks a boundary, before which Firebase ID token are considered revoked.
        /// </summary>
        public long ValidSince { get; set; }

        /// <summary>
        ///     Whether the account is disabled or not.
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        ///     The timestamp, in milliseconds, that the account last logged in at.
        /// </summary>
        public long LastLoginAt { get; set; }

        /// <summary>
        ///     The timestamp, in milliseconds, that the account was created at.
        /// </summary>
        public long CreatedAt { get; set; }

        /// <summary>
        ///     Whether the account is authenticated by the developer.
        /// </summary>
        public bool CustomAuth { get; set; }

        /// <summary>
        ///     Gets or sets the first name. Only available when logging in with Facebook or Google.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        ///     Gets or sets the last name. Only available when logging in with Facebook or Google.
        /// </summary>
        public string LastName { get; set; }
    }
}
