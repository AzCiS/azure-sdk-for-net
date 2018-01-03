
namespace Microsoft.Azure.Management.DataBox.Models
{
    using Azure;
    using Management;
    using DataBox;
    using Rest;
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// Unencrypted secrets for accessing device.
    /// </summary>
    public partial class UnencryptedSecrets
    {
        /// <summary>
        /// Initializes a new instance of the UnencryptedSecrets class.
        /// </summary>
        public UnencryptedSecrets() { }

        /// <summary>
        /// Initializes a new instance of the UnencryptedSecrets class.
        /// </summary>
        /// <param name="jobName">Name of the job.</param>
        /// <param name="deviceType">The Device Type used in the job. Possible
        /// values include: 'Pod', 'Disk', 'Cabinet'</param>
        /// <param name="jobSecrets">Secrets related to this job.</param>
        public UnencryptedSecrets(string jobName, DeviceType? deviceType = default(DeviceType?), JobSecrets jobSecrets = default(JobSecrets))
        {
            JobName = jobName;
            DeviceType = deviceType;
            JobSecrets = jobSecrets;
        }

        /// <summary>
        /// Gets or sets name of the job.
        /// </summary>
        [JsonProperty(PropertyName = "jobName")]
        public string JobName { get; set; }

        /// <summary>
        /// Gets or sets the Device Type used in the job. Possible values
        /// include: 'Pod', 'Disk', 'Cabinet'
        /// </summary>
        [JsonProperty(PropertyName = "deviceType")]
        public DeviceType? DeviceType { get; set; }

        /// <summary>
        /// Gets or sets secrets related to this job.
        /// </summary>
        [JsonProperty(PropertyName = "jobSecrets")]
        public JobSecrets JobSecrets { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (JobName == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "JobName");
            }
        }
    }
}

