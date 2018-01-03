
namespace Microsoft.Azure.Management.DataBox.Models
{
    using Azure;
    using Management;
    using DataBox;
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// Shipment pick up request details.
    /// </summary>
    public partial class ShipmentPickUpRequest
    {
        /// <summary>
        /// Initializes a new instance of the ShipmentPickUpRequest class.
        /// </summary>
        public ShipmentPickUpRequest() { }

        /// <summary>
        /// Initializes a new instance of the ShipmentPickUpRequest class.
        /// </summary>
        /// <param name="startTime">Minimum date after which the pick up should
        /// commence, this must be in local time of pick up area.</param>
        /// <param name="endTime">Maximum date before which the pick up should
        /// commence, this must be in local time of pick up area.</param>
        /// <param name="shipmentLocation">Shipment Location in the pickup
        /// place. Eg.front desk</param>
        public ShipmentPickUpRequest(System.DateTime? startTime = default(System.DateTime?), System.DateTime? endTime = default(System.DateTime?), string shipmentLocation = default(string))
        {
            StartTime = startTime;
            EndTime = endTime;
            ShipmentLocation = shipmentLocation;
        }

        /// <summary>
        /// Gets or sets minimum date after which the pick up should commence,
        /// this must be in local time of pick up area.
        /// </summary>
        [JsonProperty(PropertyName = "startTime")]
        public System.DateTime? StartTime { get; set; }

        /// <summary>
        /// Gets or sets maximum date before which the pick up should commence,
        /// this must be in local time of pick up area.
        /// </summary>
        [JsonProperty(PropertyName = "endTime")]
        public System.DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets shipment Location in the pickup place. Eg.front desk
        /// </summary>
        [JsonProperty(PropertyName = "shipmentLocation")]
        public string ShipmentLocation { get; set; }

    }
}

