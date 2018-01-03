
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
    /// The secrets related to disk job.
    /// </summary>
    [Newtonsoft.Json.JsonObject("Disk")]
    public partial class DiskJobSecrets : JobSecrets
    {
        /// <summary>
        /// Initializes a new instance of the DiskJobSecrets class.
        /// </summary>
        public DiskJobSecrets() { }

        /// <summary>
        /// Initializes a new instance of the DiskJobSecrets class.
        /// </summary>
        /// <param name="diskSecrets">Contains the list of secrets object for
        /// that device.</param>
        public DiskJobSecrets(IList<DiskSecret> diskSecrets = default(IList<DiskSecret>))
        {
            DiskSecrets = diskSecrets;
        }

        /// <summary>
        /// Gets or sets contains the list of secrets object for that device.
        /// </summary>
        [JsonProperty(PropertyName = "diskSecrets")]
        public IList<DiskSecret> DiskSecrets { get; set; }

    }
}

