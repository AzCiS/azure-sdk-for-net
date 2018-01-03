
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
    /// Output for the GetCopyLogsUri.
    /// </summary>
    public partial class GetCopyLogsUriOutput
    {
        /// <summary>
        /// Initializes a new instance of the GetCopyLogsUriOutput class.
        /// </summary>
        public GetCopyLogsUriOutput() { }

        /// <summary>
        /// Initializes a new instance of the GetCopyLogsUriOutput class.
        /// </summary>
        /// <param name="logType">Type/Level of the log.</param>
        /// <param name="copyLogDetails">List of copy log details.</param>
        public GetCopyLogsUriOutput(string logType = default(string), IList<CopyLogDetails> copyLogDetails = default(IList<CopyLogDetails>))
        {
            LogType = logType;
            CopyLogDetails = copyLogDetails;
        }

        /// <summary>
        /// Gets or sets type/Level of the log.
        /// </summary>
        [JsonProperty(PropertyName = "logType")]
        public string LogType { get; set; }

        /// <summary>
        /// Gets or sets list of copy log details.
        /// </summary>
        [JsonProperty(PropertyName = "copyLogDetails")]
        public IList<CopyLogDetails> CopyLogDetails { get; set; }

    }
}

