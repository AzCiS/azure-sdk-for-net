
namespace Microsoft.Azure.Management.DataBox.Models
{
    using Azure;
    using Management;
    using DataBox;
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// Response of the GetServiceHealth api.
    /// </summary>
    public partial class ServiceHealthResponse
    {
        /// <summary>
        /// Initializes a new instance of the ServiceHealthResponse class.
        /// </summary>
        public ServiceHealthResponse() { }

        /// <summary>
        /// Initializes a new instance of the ServiceHealthResponse class.
        /// </summary>
        public ServiceHealthResponse(string connectorType = default(string), System.DateTime? startTime = default(System.DateTime?), System.DateTime? endTime = default(System.DateTime?), bool? status = default(bool?))
        {
            ConnectorType = connectorType;
            StartTime = startTime;
            EndTime = endTime;
            Status = status;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "connectorType")]
        public string ConnectorType { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "startTime")]
        public System.DateTime? StartTime { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "endTime")]
        public System.DateTime? EndTime { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public bool? Status { get; set; }

    }
}

