
namespace Microsoft.Azure.Management.DataBox.Models
{
    using Azure;
    using Management;
    using DataBox;
    using Rest;
    using Rest.Serialization;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Output of the address validation api.
    /// </summary>
    [JsonTransformation]
    public partial class AddressValidationOutput
    {
        /// <summary>
        /// Initializes a new instance of the AddressValidationOutput class.
        /// </summary>
        public AddressValidationOutput() { }

        /// <summary>
        /// Initializes a new instance of the AddressValidationOutput class.
        /// </summary>
        /// <param name="validationStatus">The address validation status.
        /// Possible values include: 'Valid', 'Invalid', 'Ambiguous'</param>
        /// <param name="alternateAddresses">List of alternate
        /// addresses.</param>
        public AddressValidationOutput(AddressValidationStatus? validationStatus = default(AddressValidationStatus?), IList<ShippingAddress> alternateAddresses = default(IList<ShippingAddress>))
        {
            ValidationStatus = validationStatus;
            AlternateAddresses = alternateAddresses;
        }

        /// <summary>
        /// Gets or sets the address validation status. Possible values
        /// include: 'Valid', 'Invalid', 'Ambiguous'
        /// </summary>
        [JsonProperty(PropertyName = "properties.validationStatus")]
        public AddressValidationStatus? ValidationStatus { get; set; }

        /// <summary>
        /// Gets or sets list of alternate addresses.
        /// </summary>
        [JsonProperty(PropertyName = "properties.alternateAddresses")]
        public IList<ShippingAddress> AlternateAddresses { get; set; }

    }
}

