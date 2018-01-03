
namespace Microsoft.Azure.Management.DataBox.Models
{
    using Azure;
    using Management;
    using DataBox;
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// Copy progress.
    /// </summary>
    public partial class CopyProgress
    {
        /// <summary>
        /// Initializes a new instance of the CopyProgress class.
        /// </summary>
        public CopyProgress() { }

        /// <summary>
        /// Initializes a new instance of the CopyProgress class.
        /// </summary>
        /// <param name="storageAccountName">Name of the storage account where
        /// the data needs to be uploaded.</param>
        /// <param name="bytesSentToCloud">Amount of data uploaded by the job
        /// as of now.</param>
        /// <param name="totalBytesToProcess">Total amount of data to be
        /// processed by the job.</param>
        public CopyProgress(string storageAccountName = default(string), long? bytesSentToCloud = default(long?), long? totalBytesToProcess = default(long?))
        {
            StorageAccountName = storageAccountName;
            BytesSentToCloud = bytesSentToCloud;
            TotalBytesToProcess = totalBytesToProcess;
        }

        /// <summary>
        /// Gets or sets name of the storage account where the data needs to be
        /// uploaded.
        /// </summary>
        [JsonProperty(PropertyName = "storageAccountName")]
        public string StorageAccountName { get; set; }

        /// <summary>
        /// Gets or sets amount of data uploaded by the job as of now.
        /// </summary>
        [JsonProperty(PropertyName = "bytesSentToCloud")]
        public long? BytesSentToCloud { get; set; }

        /// <summary>
        /// Gets or sets total amount of data to be processed by the job.
        /// </summary>
        [JsonProperty(PropertyName = "totalBytesToProcess")]
        public long? TotalBytesToProcess { get; set; }

    }
}

