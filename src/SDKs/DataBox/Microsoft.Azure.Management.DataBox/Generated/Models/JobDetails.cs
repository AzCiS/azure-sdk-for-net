
namespace Microsoft.Azure.Management.DataBox.Models
{
    using Azure;
    using Management;
    using DataBox;
    using Rest;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Job details.
    /// </summary>
    public partial class JobDetails
    {
        /// <summary>
        /// Initializes a new instance of the JobDetails class.
        /// </summary>
        public JobDetails() { }

        /// <summary>
        /// Initializes a new instance of the JobDetails class.
        /// </summary>
        /// <param name="contactDetails">Contact details for shipping.</param>
        /// <param name="shippingAddress">Shipping address of the
        /// customer.</param>
        /// <param name="expectedDataSizeInTeraBytes">The expected size of the
        /// data, which needs to be transfered in this job, in tera
        /// bytes.</param>
        /// <param name="jobStages">List of stages that run in the job.</param>
        /// <param name="errorDetails">Error details for failure. This is
        /// optional.</param>
        public JobDetails(ContactDetails contactDetails, ShippingAddress shippingAddress, int? expectedDataSizeInTeraBytes = default(int?), IList<JobStages> jobStages = default(IList<JobStages>), IList<JobErrorDetails> errorDetails = default(IList<JobErrorDetails>))
        {
            ExpectedDataSizeInTeraBytes = expectedDataSizeInTeraBytes;
            JobStages = jobStages;
            ContactDetails = contactDetails;
            ShippingAddress = shippingAddress;
            ErrorDetails = errorDetails;
        }

        /// <summary>
        /// Gets or sets the expected size of the data, which needs to be
        /// transfered in this job, in tera bytes.
        /// </summary>
        [JsonProperty(PropertyName = "expectedDataSizeInTeraBytes")]
        public int? ExpectedDataSizeInTeraBytes { get; set; }

        /// <summary>
        /// Gets or sets list of stages that run in the job.
        /// </summary>
        [JsonProperty(PropertyName = "jobStages")]
        public IList<JobStages> JobStages { get; set; }

        /// <summary>
        /// Gets or sets contact details for shipping.
        /// </summary>
        [JsonProperty(PropertyName = "contactDetails")]
        public ContactDetails ContactDetails { get; set; }

        /// <summary>
        /// Gets or sets shipping address of the customer.
        /// </summary>
        [JsonProperty(PropertyName = "shippingAddress")]
        public ShippingAddress ShippingAddress { get; set; }

        /// <summary>
        /// Gets or sets error details for failure. This is optional.
        /// </summary>
        [JsonProperty(PropertyName = "errorDetails")]
        public IList<JobErrorDetails> ErrorDetails { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (ContactDetails == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "ContactDetails");
            }
            if (ShippingAddress == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "ShippingAddress");
            }
            if (JobStages != null)
            {
                foreach (var element in JobStages)
                {
                    if (element != null)
                    {
                        element.Validate();
                    }
                }
            }
            if (ContactDetails != null)
            {
                ContactDetails.Validate();
            }
            if (ShippingAddress != null)
            {
                ShippingAddress.Validate();
            }
        }
    }
}

