
namespace Microsoft.Azure.Management.DataBox.Models
{
    using Azure;
    using Management;
    using DataBox;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The secrets related to a Pod.
    /// </summary>
    public partial class PodSecret
    {
        /// <summary>
        /// Initializes a new instance of the PodSecret class.
        /// </summary>
        public PodSecret() { }

        /// <summary>
        /// Initializes a new instance of the PodSecret class.
        /// </summary>
        /// <param name="deviceSerialNumber">Serial number of the assigned
        /// device.</param>
        /// <param name="devicePassword">Password for out of the box experience
        /// on device.</param>
        /// <param name="accountCredentialDetails">Per account level access
        /// credentials.</param>
        public PodSecret(string deviceSerialNumber = default(string), string devicePassword = default(string), IList<AccountCredentialDetails> accountCredentialDetails = default(IList<AccountCredentialDetails>))
        {
            DeviceSerialNumber = deviceSerialNumber;
            DevicePassword = devicePassword;
            AccountCredentialDetails = accountCredentialDetails;
        }

        /// <summary>
        /// Gets or sets serial number of the assigned device.
        /// </summary>
        [JsonProperty(PropertyName = "deviceSerialNumber")]
        public string DeviceSerialNumber { get; set; }

        /// <summary>
        /// Gets or sets password for out of the box experience on device.
        /// </summary>
        [JsonProperty(PropertyName = "devicePassword")]
        public string DevicePassword { get; set; }

        /// <summary>
        /// Gets or sets per account level access credentials.
        /// </summary>
        [JsonProperty(PropertyName = "accountCredentialDetails")]
        public IList<AccountCredentialDetails> AccountCredentialDetails { get; set; }

    }
}

