
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
    /// The secrets related to a pod job.
    /// </summary>
    [Newtonsoft.Json.JsonObject("Pod")]
    public partial class PodJobSecrets : JobSecrets
    {
        /// <summary>
        /// Initializes a new instance of the PodJobSecrets class.
        /// </summary>
        public PodJobSecrets() { }

        /// <summary>
        /// Initializes a new instance of the PodJobSecrets class.
        /// </summary>
        /// <param name="podSecrets">Contains the list of secret objects for a
        /// job.</param>
        public PodJobSecrets(IList<PodSecret> podSecrets = default(IList<PodSecret>))
        {
            PodSecrets = podSecrets;
        }

        /// <summary>
        /// Gets or sets contains the list of secret objects for a job.
        /// </summary>
        [JsonProperty(PropertyName = "podSecrets")]
        public IList<PodSecret> PodSecrets { get; set; }

    }
}

