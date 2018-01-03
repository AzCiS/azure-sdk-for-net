
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
    /// The secrets related to a cabinet job.
    /// </summary>
    [Newtonsoft.Json.JsonObject("Cabinet")]
    public partial class CabinetJobSecrets : JobSecrets
    {
        /// <summary>
        /// Initializes a new instance of the CabinetJobSecrets class.
        /// </summary>
        public CabinetJobSecrets() { }

        /// <summary>
        /// Initializes a new instance of the CabinetJobSecrets class.
        /// </summary>
        /// <param name="cabinetPodSecrets">Contains the list of secret objects
        /// for a cabinet job.</param>
        public CabinetJobSecrets(IList<CabinetPodSecret> cabinetPodSecrets = default(IList<CabinetPodSecret>))
        {
            CabinetPodSecrets = cabinetPodSecrets;
        }

        /// <summary>
        /// Gets or sets contains the list of secret objects for a cabinet job.
        /// </summary>
        [JsonProperty(PropertyName = "cabinetPodSecrets")]
        public IList<CabinetPodSecret> CabinetPodSecrets { get; set; }

    }
}

