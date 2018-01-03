
namespace Microsoft.Azure.Management.DataBox.Models
{
    using Azure;
    using Management;
    using DataBox;
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// Contains all the secrets of a Disk.
    /// </summary>
    public partial class DiskSecret
    {
        /// <summary>
        /// Initializes a new instance of the DiskSecret class.
        /// </summary>
        public DiskSecret() { }

        /// <summary>
        /// Initializes a new instance of the DiskSecret class.
        /// </summary>
        /// <param name="diskSerialNumber">Serial number of the assigned
        /// disk.</param>
        /// <param name="bitLockerKey">Bit Locker key of the disk which can be
        /// used to unlock the disk to copy data.</param>
        public DiskSecret(string diskSerialNumber = default(string), string bitLockerKey = default(string))
        {
            DiskSerialNumber = diskSerialNumber;
            BitLockerKey = bitLockerKey;
        }

        /// <summary>
        /// Gets or sets serial number of the assigned disk.
        /// </summary>
        [JsonProperty(PropertyName = "diskSerialNumber")]
        public string DiskSerialNumber { get; set; }

        /// <summary>
        /// Gets or sets bit Locker key of the disk which can be used to unlock
        /// the disk to copy data.
        /// </summary>
        [JsonProperty(PropertyName = "bitLockerKey")]
        public string BitLockerKey { get; set; }

    }
}

