
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
    /// Job stages.
    /// </summary>
    public partial class JobStages
    {
        /// <summary>
        /// Initializes a new instance of the JobStages class.
        /// </summary>
        public JobStages() { }

        /// <summary>
        /// Initializes a new instance of the JobStages class.
        /// </summary>
        /// <param name="stageName">Name of the job stage. Possible values
        /// include: 'DeviceOrdered', 'DevicePrepared', 'Dispatched',
        /// 'Delivered', 'PickedUp', 'AtAzureDC', 'DataCopy', 'Completed',
        /// 'CompletedWithErrors', 'Cancelled',
        /// 'Failed_IssueReportedAtCustomer', 'Failed_IssueDetectedAtAzureDC',
        /// 'Aborted'</param>
        /// <param name="stageStatus">Status of the job stage. Possible values
        /// include: 'None', 'InProgress', 'Succeeded', 'Failed', 'Cancelled',
        /// 'Cancelling', 'SucceededWithErrors'</param>
        /// <param name="stageTime">Time for the job stage in UTC ISO 8601
        /// format.</param>
        /// <param name="jobStageDetails">Job Stage Details</param>
        /// <param name="errorDetails">Error details for the stage.</param>
        public JobStages(StageName stageName, StageStatus stageStatus, System.DateTime stageTime, object jobStageDetails = default(object), IList<JobErrorDetails> errorDetails = default(IList<JobErrorDetails>))
        {
            StageName = stageName;
            StageStatus = stageStatus;
            StageTime = stageTime;
            JobStageDetails = jobStageDetails;
            ErrorDetails = errorDetails;
        }

        /// <summary>
        /// Gets or sets name of the job stage. Possible values include:
        /// 'DeviceOrdered', 'DevicePrepared', 'Dispatched', 'Delivered',
        /// 'PickedUp', 'AtAzureDC', 'DataCopy', 'Completed',
        /// 'CompletedWithErrors', 'Cancelled',
        /// 'Failed_IssueReportedAtCustomer', 'Failed_IssueDetectedAtAzureDC',
        /// 'Aborted'
        /// </summary>
        [JsonProperty(PropertyName = "stageName")]
        public StageName StageName { get; set; }

        /// <summary>
        /// Gets or sets status of the job stage. Possible values include:
        /// 'None', 'InProgress', 'Succeeded', 'Failed', 'Cancelled',
        /// 'Cancelling', 'SucceededWithErrors'
        /// </summary>
        [JsonProperty(PropertyName = "stageStatus")]
        public StageStatus StageStatus { get; set; }

        /// <summary>
        /// Gets or sets time for the job stage in UTC ISO 8601 format.
        /// </summary>
        [JsonProperty(PropertyName = "stageTime")]
        public System.DateTime StageTime { get; set; }

        /// <summary>
        /// Gets or sets job Stage Details
        /// </summary>
        [JsonProperty(PropertyName = "jobStageDetails")]
        public object JobStageDetails { get; set; }

        /// <summary>
        /// Gets or sets error details for the stage.
        /// </summary>
        [JsonProperty(PropertyName = "errorDetails")]
        public IList<JobErrorDetails> ErrorDetails { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="Rest.ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
        }
    }
}

