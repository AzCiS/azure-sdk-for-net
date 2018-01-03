
namespace Microsoft.Azure.Management.DataBox.Models
{
    using Azure;
    using Management;
    using DataBox;
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// Disk Copy Progress
    /// </summary>
    public partial class DiskCopyProgress
    {
        /// <summary>
        /// Initializes a new instance of the DiskCopyProgress class.
        /// </summary>
        public DiskCopyProgress() { }

        /// <summary>
        /// Initializes a new instance of the DiskCopyProgress class.
        /// </summary>
        /// <param name="serialNumber">The serial number of the disk</param>
        /// <param name="percentComplete">Indicates the percentage completed
        /// for the copy of the disk.</param>
        /// <param name="status">The Status of the copy. Possible values
        /// include: 'NotStarted', 'InProgress', 'Completed',
        /// 'CompletedWithErrors', 'Failed'</param>
        public DiskCopyProgress(string serialNumber = default(string), int? percentComplete = default(int?), CopyStatus? status = default(CopyStatus?))
        {
            SerialNumber = serialNumber;
            PercentComplete = percentComplete;
            Status = status;
        }

        /// <summary>
        /// Gets or sets the serial number of the disk
        /// </summary>
        [JsonProperty(PropertyName = "serialNumber")]
        public string SerialNumber { get; set; }

        /// <summary>
        /// Gets or sets indicates the percentage completed for the copy of the
        /// disk.
        /// </summary>
        [JsonProperty(PropertyName = "percentComplete")]
        public int? PercentComplete { get; set; }

        /// <summary>
        /// Gets or sets the Status of the copy. Possible values include:
        /// 'NotStarted', 'InProgress', 'Completed', 'CompletedWithErrors',
        /// 'Failed'
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public CopyStatus? Status { get; set; }

    }
}

