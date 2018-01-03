
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
    /// List of service health response.
    /// </summary>
    public partial class ServiceHealthResponseList
    {
        /// <summary>
        /// Initializes a new instance of the ServiceHealthResponseList class.
        /// </summary>
        public ServiceHealthResponseList() { }

        /// <summary>
        /// Initializes a new instance of the ServiceHealthResponseList class.
        /// </summary>
        /// <param name="dependencies">List of ServiceHealthResponse.</param>
        public ServiceHealthResponseList(string serviceVersion = default(string), IList<ServiceHealthResponse> dependencies = default(IList<ServiceHealthResponse>))
        {
            ServiceVersion = serviceVersion;
            Dependencies = dependencies;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "serviceVersion")]
        public string ServiceVersion { get; set; }

        /// <summary>
        /// Gets or sets list of ServiceHealthResponse.
        /// </summary>
        [JsonProperty(PropertyName = "dependencies")]
        public IList<ServiceHealthResponse> Dependencies { get; set; }

    }
}

