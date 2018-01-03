
namespace Microsoft.Azure.Management.DataBox.Models
{
    using Azure;
    using Management;
    using DataBox;
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// Inputs to get list of supported storage regions and service regions for
    /// job creation.
    /// </summary>
    public partial class RegionAvailabilityInput
    {
        /// <summary>
        /// Initializes a new instance of the RegionAvailabilityInput class.
        /// </summary>
        public RegionAvailabilityInput() { }

        /// <summary>
        /// Initializes a new instance of the RegionAvailabilityInput class.
        /// </summary>
        /// <param name="countryCode">Country for which the supported regions
        /// are requested. Possible values include: 'US', 'NL', 'IE', 'AT',
        /// 'IT', 'BE', 'LV', 'BG', 'LT', 'HR', 'LU', 'CY', 'MT', 'CZ', 'DK',
        /// 'PL', 'EE', 'PT', 'FI', 'RO', 'FR', 'SK', 'DE', 'SI', 'GR', 'ES',
        /// 'HU', 'SE', 'GB'</param>
        /// <param name="deviceType">Device type for which the supported
        /// regions have to be fetched. Possible values include: 'Pod', 'Disk',
        /// 'Cabinet'</param>
        public RegionAvailabilityInput(CountryCode countryCode, DeviceType deviceType)
        {
            CountryCode = countryCode;
            DeviceType = deviceType;
        }

        /// <summary>
        /// Gets or sets country for which the supported regions are requested.
        /// Possible values include: 'US', 'NL', 'IE', 'AT', 'IT', 'BE', 'LV',
        /// 'BG', 'LT', 'HR', 'LU', 'CY', 'MT', 'CZ', 'DK', 'PL', 'EE', 'PT',
        /// 'FI', 'RO', 'FR', 'SK', 'DE', 'SI', 'GR', 'ES', 'HU', 'SE', 'GB'
        /// </summary>
        [JsonProperty(PropertyName = "countryCode")]
        public CountryCode CountryCode { get; set; }

        /// <summary>
        /// Gets or sets device type for which the supported regions have to be
        /// fetched. Possible values include: 'Pod', 'Disk', 'Cabinet'
        /// </summary>
        [JsonProperty(PropertyName = "deviceType")]
        public DeviceType DeviceType { get; set; }

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

