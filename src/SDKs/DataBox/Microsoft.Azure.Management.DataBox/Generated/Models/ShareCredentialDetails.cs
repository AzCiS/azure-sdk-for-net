
namespace Microsoft.Azure.Management.DataBox.Models
{
    using Azure;
    using Management;
    using DataBox;
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// Credential details of the shares in account.
    /// </summary>
    public partial class ShareCredentialDetails
    {
        /// <summary>
        /// Initializes a new instance of the ShareCredentialDetails class.
        /// </summary>
        public ShareCredentialDetails() { }

        /// <summary>
        /// Initializes a new instance of the ShareCredentialDetails class.
        /// </summary>
        /// <param name="shareName">Name of the share.</param>
        /// <param name="userName">User name for the share.</param>
        /// <param name="password">Password for the share.</param>
        public ShareCredentialDetails(string shareName = default(string), string userName = default(string), string password = default(string))
        {
            ShareName = shareName;
            UserName = userName;
            Password = password;
        }

        /// <summary>
        /// Gets or sets name of the share.
        /// </summary>
        [JsonProperty(PropertyName = "shareName")]
        public string ShareName { get; set; }

        /// <summary>
        /// Gets or sets user name for the share.
        /// </summary>
        [JsonProperty(PropertyName = "userName")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets password for the share.
        /// </summary>
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

    }
}

