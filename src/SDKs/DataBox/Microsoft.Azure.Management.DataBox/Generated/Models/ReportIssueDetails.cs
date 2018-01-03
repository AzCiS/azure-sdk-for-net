
namespace Microsoft.Azure.Management.DataBox.Models
{
    using Azure;
    using Management;
    using DataBox;
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// Details of the reported issue.
    /// </summary>
    public partial class ReportIssueDetails
    {
        /// <summary>
        /// Initializes a new instance of the ReportIssueDetails class.
        /// </summary>
        public ReportIssueDetails() { }

        /// <summary>
        /// Initializes a new instance of the ReportIssueDetails class.
        /// </summary>
        /// <param name="issueType">Issue Type. Possible values include:
        /// 'DeviceMismatch', 'ValidationStringMismatch',
        /// 'CredentialNotWorking', 'DeviceFailure'</param>
        /// <param name="deviceIssueType">Device Issue Type. Only used for
        /// Device failure issue. Possible values include: 'DeviceTampering',
        /// 'DeviceNotBootingUp', 'DeviceHealthCheckShowFailures',
        /// 'NICsAreNotWorking', 'Misc'</param>
        public ReportIssueDetails(IssueType? issueType = default(IssueType?), DeviceIssueType? deviceIssueType = default(DeviceIssueType?))
        {
            IssueType = issueType;
            DeviceIssueType = deviceIssueType;
        }

        /// <summary>
        /// Gets or sets issue Type. Possible values include: 'DeviceMismatch',
        /// 'ValidationStringMismatch', 'CredentialNotWorking', 'DeviceFailure'
        /// </summary>
        [JsonProperty(PropertyName = "issueType")]
        public IssueType? IssueType { get; set; }

        /// <summary>
        /// Gets or sets device Issue Type. Only used for Device failure issue.
        /// Possible values include: 'DeviceTampering', 'DeviceNotBootingUp',
        /// 'DeviceHealthCheckShowFailures', 'NICsAreNotWorking', 'Misc'
        /// </summary>
        [JsonProperty(PropertyName = "deviceIssueType")]
        public DeviceIssueType? DeviceIssueType { get; set; }

    }
}

