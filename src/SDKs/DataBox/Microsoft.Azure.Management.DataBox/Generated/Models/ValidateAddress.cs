
namespace Microsoft.Azure.Management.DataBox.Models
{
    using Azure;
    using Management;
    using DataBox;
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// The requirements to validate customer address where the device needs to
    /// be shipped.
    /// </summary>
    public partial class ValidateAddress
    {
        /// <summary>
        /// Initializes a new instance of the ValidateAddress class.
        /// </summary>
        public ValidateAddress() { }

        /// <summary>
        /// Initializes a new instance of the ValidateAddress class.
        /// </summary>
        /// <param name="shippingAddress">Shipping address of the
        /// customer.</param>
        /// <param name="deviceType">Device type to be used for the job.
        /// Possible values include: 'Pod', 'Disk', 'Cabinet'</param>
        public ValidateAddress(ShippingAddress shippingAddress = default(ShippingAddress), DeviceType? deviceType = default(DeviceType?))
        {
            ShippingAddress = shippingAddress;
            DeviceType = deviceType;
        }

        /// <summary>
        /// Gets or sets shipping address of the customer.
        /// </summary>
        [JsonProperty(PropertyName = "shippingAddress")]
        public ShippingAddress ShippingAddress { get; set; }

        /// <summary>
        /// Gets or sets device type to be used for the job. Possible values
        /// include: 'Pod', 'Disk', 'Cabinet'
        /// </summary>
        [JsonProperty(PropertyName = "deviceType")]
        public DeviceType? DeviceType { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="Rest.ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (ShippingAddress != null)
            {
                ShippingAddress.Validate();
            }
        }
    }
}

