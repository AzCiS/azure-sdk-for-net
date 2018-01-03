
namespace Microsoft.Azure.Management.DataBox.Models
{
    using Azure;
    using Management;
    using DataBox;
    using Rest;
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// Details for the shipping label.
    /// </summary>
    public partial class ShippingLabelDetails
    {
        /// <summary>
        /// Initializes a new instance of the ShippingLabelDetails class.
        /// </summary>
        public ShippingLabelDetails() { }

        /// <summary>
        /// Initializes a new instance of the ShippingLabelDetails class.
        /// </summary>
        /// <param name="shippingLabelSasUri">Sas uri for accessing the
        /// shipping label.</param>
        public ShippingLabelDetails(string shippingLabelSasUri)
        {
            ShippingLabelSasUri = shippingLabelSasUri;
        }

        /// <summary>
        /// Gets or sets sas uri for accessing the shipping label.
        /// </summary>
        [JsonProperty(PropertyName = "shippingLabelSasUri")]
        public string ShippingLabelSasUri { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (ShippingLabelSasUri == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "ShippingLabelSasUri");
            }
        }
    }
}

