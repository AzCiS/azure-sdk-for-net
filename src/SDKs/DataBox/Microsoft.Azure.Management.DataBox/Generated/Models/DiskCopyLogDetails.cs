
namespace Microsoft.Azure.Management.DataBox.Models
{
    using Azure;
    using Management;
    using DataBox;
    using Rest;
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// Copy Log Details for a disk
    /// </summary>
    [Newtonsoft.Json.JsonObject("Disk")]
    public partial class DiskCopyLogDetails : CopyLogDetails
    {
        /// <summary>
        /// Initializes a new instance of the DiskCopyLogDetails class.
        /// </summary>
        public DiskCopyLogDetails() { }

        /// <summary>
        /// Initializes a new instance of the DiskCopyLogDetails class.
        /// </summary>
        /// <param name="diskSerialNumber">Disk Serial Number.</param>
        /// <param name="errorLogLink">Link for copy error logs.</param>
        /// <param name="verboseLogLink">Link for copy verbose logs.</param>
        public DiskCopyLogDetails(string diskSerialNumber, string errorLogLink, string verboseLogLink)
        {
            DiskSerialNumber = diskSerialNumber;
            ErrorLogLink = errorLogLink;
            VerboseLogLink = verboseLogLink;
        }

        /// <summary>
        /// Gets or sets disk Serial Number.
        /// </summary>
        [JsonProperty(PropertyName = "diskSerialNumber")]
        public string DiskSerialNumber { get; set; }

        /// <summary>
        /// Gets or sets link for copy error logs.
        /// </summary>
        [JsonProperty(PropertyName = "errorLogLink")]
        public string ErrorLogLink { get; set; }

        /// <summary>
        /// Gets or sets link for copy verbose logs.
        /// </summary>
        [JsonProperty(PropertyName = "verboseLogLink")]
        public string VerboseLogLink { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (DiskSerialNumber == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "DiskSerialNumber");
            }
            if (ErrorLogLink == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "ErrorLogLink");
            }
            if (VerboseLogLink == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "VerboseLogLink");
            }
        }
    }
}

