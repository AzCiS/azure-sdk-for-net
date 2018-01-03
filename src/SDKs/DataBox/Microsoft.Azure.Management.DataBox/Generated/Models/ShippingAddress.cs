
namespace Microsoft.Azure.Management.DataBox.Models
{
    using Azure;
    using Management;
    using DataBox;
    using Rest;
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// Shipping address where customer wishes to receive the device.
    /// </summary>
    public partial class ShippingAddress
    {
        /// <summary>
        /// Initializes a new instance of the ShippingAddress class.
        /// </summary>
        public ShippingAddress() { }

        /// <summary>
        /// Initializes a new instance of the ShippingAddress class.
        /// </summary>
        /// <param name="streetAddress1">Street Address line 1.</param>
        /// <param name="city">Name of the City.</param>
        /// <param name="stateOrProvince">Name of the State or
        /// Province.</param>
        /// <param name="country">Name of the Country.</param>
        /// <param name="postalCode">Postal code.</param>
        /// <param name="streetAddress2">Street Address line 2.</param>
        /// <param name="streetAddress3">Street Address line 3.</param>
        /// <param name="zipExtendedCode">Extended Zip Code.</param>
        /// <param name="companyName">Name of the company.</param>
        /// <param name="addressType">Type of address. Possible values include:
        /// 'None', 'Residential', 'Commercial'</param>
        public ShippingAddress(string streetAddress1, string city, string stateOrProvince, string country, string postalCode, string streetAddress2 = default(string), string streetAddress3 = default(string), string zipExtendedCode = default(string), string companyName = default(string), AddressType? addressType = default(AddressType?))
        {
            StreetAddress1 = streetAddress1;
            StreetAddress2 = streetAddress2;
            StreetAddress3 = streetAddress3;
            City = city;
            StateOrProvince = stateOrProvince;
            Country = country;
            PostalCode = postalCode;
            ZipExtendedCode = zipExtendedCode;
            CompanyName = companyName;
            AddressType = addressType;
        }

        /// <summary>
        /// Gets or sets street Address line 1.
        /// </summary>
        [JsonProperty(PropertyName = "streetAddress1")]
        public string StreetAddress1 { get; set; }

        /// <summary>
        /// Gets or sets street Address line 2.
        /// </summary>
        [JsonProperty(PropertyName = "streetAddress2")]
        public string StreetAddress2 { get; set; }

        /// <summary>
        /// Gets or sets street Address line 3.
        /// </summary>
        [JsonProperty(PropertyName = "streetAddress3")]
        public string StreetAddress3 { get; set; }

        /// <summary>
        /// Gets or sets name of the City.
        /// </summary>
        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets name of the State or Province.
        /// </summary>
        [JsonProperty(PropertyName = "stateOrProvince")]
        public string StateOrProvince { get; set; }

        /// <summary>
        /// Gets or sets name of the Country.
        /// </summary>
        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets postal code.
        /// </summary>
        [JsonProperty(PropertyName = "postalCode")]
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets extended Zip Code.
        /// </summary>
        [JsonProperty(PropertyName = "zipExtendedCode")]
        public string ZipExtendedCode { get; set; }

        /// <summary>
        /// Gets or sets name of the company.
        /// </summary>
        [JsonProperty(PropertyName = "companyName")]
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets type of address. Possible values include: 'None',
        /// 'Residential', 'Commercial'
        /// </summary>
        [JsonProperty(PropertyName = "addressType")]
        public AddressType? AddressType { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (StreetAddress1 == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "StreetAddress1");
            }
            if (City == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "City");
            }
            if (StateOrProvince == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "StateOrProvince");
            }
            if (Country == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Country");
            }
            if (PostalCode == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "PostalCode");
            }
        }
    }
}

