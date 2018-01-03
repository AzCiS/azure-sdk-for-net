
namespace Microsoft.Azure.Management.DataBox.Models
{
    using Azure;
    using Management;
    using DataBox;
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// Shipping details.
    /// </summary>
    public partial class PackageShippingDetails
    {
        /// <summary>
        /// Initializes a new instance of the PackageShippingDetails class.
        /// </summary>
        public PackageShippingDetails() { }

        /// <summary>
        /// Initializes a new instance of the PackageShippingDetails class.
        /// </summary>
        /// <param name="carrierName">Name of the carrier.</param>
        /// <param name="trackingId">Tracking Id of shipment.</param>
        /// <param name="trackingUrl">Url where shipment can be
        /// tracked.</param>
        public PackageShippingDetails(string carrierName = default(string), string trackingId = default(string), string trackingUrl = default(string))
        {
            CarrierName = carrierName;
            TrackingId = trackingId;
            TrackingUrl = trackingUrl;
        }

        /// <summary>
        /// Gets or sets name of the carrier.
        /// </summary>
        [JsonProperty(PropertyName = "carrierName")]
        public string CarrierName { get; set; }

        /// <summary>
        /// Gets or sets tracking Id of shipment.
        /// </summary>
        [JsonProperty(PropertyName = "trackingId")]
        public string TrackingId { get; set; }

        /// <summary>
        /// Gets or sets url where shipment can be tracked.
        /// </summary>
        [JsonProperty(PropertyName = "trackingUrl")]
        public string TrackingUrl { get; set; }

    }
}

