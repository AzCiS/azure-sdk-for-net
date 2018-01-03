
namespace Microsoft.Azure.Management.DataBox.Models
{
    using Azure;
    using Management;
    using DataBox;
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// Storage region and service region mapping
    /// </summary>
    public partial class SupportedRegions
    {
        /// <summary>
        /// Initializes a new instance of the SupportedRegions class.
        /// </summary>
        public SupportedRegions() { }

        /// <summary>
        /// Initializes a new instance of the SupportedRegions class.
        /// </summary>
        /// <param name="storageRegion">Storage Region. Possible values
        /// include: 'westus', 'centralus', 'eastus', 'northcentralus',
        /// 'southcentralus', 'eastus2', 'westus2', 'westcentralus',
        /// 'westeurope', 'northeurope'</param>
        /// <param name="serviceRegion">Service Region. Possible values
        /// include: 'westus', 'westeurope', 'northeurope',
        /// 'centraluseuap'</param>
        public SupportedRegions(StorageRegion storageRegion, ServiceRegion serviceRegion)
        {
            StorageRegion = storageRegion;
            ServiceRegion = serviceRegion;
        }

        /// <summary>
        /// Gets or sets storage Region. Possible values include: 'westus',
        /// 'centralus', 'eastus', 'northcentralus', 'southcentralus',
        /// 'eastus2', 'westus2', 'westcentralus', 'westeurope', 'northeurope'
        /// </summary>
        [JsonProperty(PropertyName = "storageRegion")]
        public StorageRegion StorageRegion { get; set; }

        /// <summary>
        /// Gets or sets service Region. Possible values include: 'westus',
        /// 'westeurope', 'northeurope', 'centraluseuap'
        /// </summary>
        [JsonProperty(PropertyName = "serviceRegion")]
        public ServiceRegion ServiceRegion { get; set; }

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

