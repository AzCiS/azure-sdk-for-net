
namespace Microsoft.Azure.Management.DataBox.Models
{
    using Azure;
    using Management;
    using DataBox;
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// Shipment pick up response.
    /// </summary>
    public partial class ShipmentPickUpResponse
    {
        /// <summary>
        /// Initializes a new instance of the ShipmentPickUpResponse class.
        /// </summary>
        public ShipmentPickUpResponse() { }

        /// <summary>
        /// Initializes a new instance of the ShipmentPickUpResponse class.
        /// </summary>
        /// <param name="confirmationNumber">Confirmation number for the pick
        /// up request.</param>
        /// <param name="readyByTime">Time by which shipment should be ready
        /// for pick up, this is in local time of pick up area.</param>
        public ShipmentPickUpResponse(string confirmationNumber = default(string), System.DateTime? readyByTime = default(System.DateTime?))
        {
            ConfirmationNumber = confirmationNumber;
            ReadyByTime = readyByTime;
        }

        /// <summary>
        /// Gets or sets confirmation number for the pick up request.
        /// </summary>
        [JsonProperty(PropertyName = "confirmationNumber")]
        public string ConfirmationNumber { get; set; }

        /// <summary>
        /// Gets or sets time by which shipment should be ready for pick up,
        /// this is in local time of pick up area.
        /// </summary>
        [JsonProperty(PropertyName = "readyByTime")]
        public System.DateTime? ReadyByTime { get; set; }

    }
}

