
namespace Microsoft.Azure.Management.DataBox.Models
{
    using Azure;
    using Management;
    using DataBox;
    using Rest;
    using Rest.Azure;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Model of the Resource.
    /// </summary>
    public partial class Resource : IResource
    {
        /// <summary>
        /// Initializes a new instance of the Resource class.
        /// </summary>
        public Resource() { }

        /// <summary>
        /// Initializes a new instance of the Resource class.
        /// </summary>
        /// <param name="location">The location of the resource. This will be
        /// one of the supported and registered Azure Regions (e.g. West US,
        /// East US, Southeast Asia, etc.). The region of a resource cannot be
        /// changed once it is created, but if an identical region is specified
        /// on update the request will succeed.</param>
        /// <param name="tags">The list of key value pairs that describe the
        /// resource. These tags can be used in viewing and grouping this
        /// resource (across resource groups).</param>
        /// <param name="sku">The sku type.</param>
        public Resource(string location, IDictionary<string, string> tags = default(IDictionary<string, string>), Sku sku = default(Sku))
        {
            Location = location;
            Tags = tags;
            Sku = sku;
        }

        /// <summary>
        /// Gets or sets the location of the resource. This will be one of the
        /// supported and registered Azure Regions (e.g. West US, East US,
        /// Southeast Asia, etc.). The region of a resource cannot be changed
        /// once it is created, but if an identical region is specified on
        /// update the request will succeed.
        /// </summary>
        [JsonProperty(PropertyName = "location")]
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the list of key value pairs that describe the
        /// resource. These tags can be used in viewing and grouping this
        /// resource (across resource groups).
        /// </summary>
        [JsonProperty(PropertyName = "tags")]
        public IDictionary<string, string> Tags { get; set; }

        /// <summary>
        /// Gets or sets the sku type.
        /// </summary>
        [JsonProperty(PropertyName = "sku")]
        public Sku Sku { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (Location == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Location");
            }
        }
    }
}

