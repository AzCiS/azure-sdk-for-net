
namespace Microsoft.Azure.Management.DataBox.Models
{
    using Azure;
    using Management;
    using DataBox;
    using Rest;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// List of service regions and storage regions
    /// </summary>
    public partial class RegionAvailabilityResponse
    {
        /// <summary>
        /// Initializes a new instance of the RegionAvailabilityResponse class.
        /// </summary>
        public RegionAvailabilityResponse() { }

        /// <summary>
        /// Initializes a new instance of the RegionAvailabilityResponse class.
        /// </summary>
        /// <param name="supportedRegions">List of supported region.</param>
        public RegionAvailabilityResponse(IList<SupportedRegions> supportedRegions)
        {
            SupportedRegions = supportedRegions;
        }

        /// <summary>
        /// Gets or sets list of supported region.
        /// </summary>
        [JsonProperty(PropertyName = "supportedRegions")]
        public IList<SupportedRegions> SupportedRegions { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (SupportedRegions == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "SupportedRegions");
            }
            if (SupportedRegions != null)
            {
                foreach (var element in SupportedRegions)
                {
                    if (element != null)
                    {
                        element.Validate();
                    }
                }
            }
        }
    }
}

