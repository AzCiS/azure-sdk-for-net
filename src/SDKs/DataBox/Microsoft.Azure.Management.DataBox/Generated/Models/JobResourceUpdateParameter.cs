
namespace Microsoft.Azure.Management.DataBox.Models
{
    using Azure;
    using Management;
    using DataBox;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// The JobResourceUpdateParameter.
    /// </summary>
    public partial class JobResourceUpdateParameter
    {
        /// <summary>
        /// Initializes a new instance of the JobResourceUpdateParameter class.
        /// </summary>
        public JobResourceUpdateParameter() { }

        /// <summary>
        /// Initializes a new instance of the JobResourceUpdateParameter class.
        /// </summary>
        /// <param name="sku">The sku type.</param>
        /// <param name="tags">The list of key value pairs that describe the
        /// resource. These tags can be used in viewing and grouping this
        /// resource (across resource groups).</param>
        public JobResourceUpdateParameter(Sku sku = default(Sku), IDictionary<string, string> tags = default(IDictionary<string, string>))
        {
            Sku = sku;
            Tags = tags;
        }

        /// <summary>
        /// Gets or sets the sku type.
        /// </summary>
        [JsonProperty(PropertyName = "sku")]
        public Sku Sku { get; set; }

        /// <summary>
        /// Gets or sets the list of key value pairs that describe the
        /// resource. These tags can be used in viewing and grouping this
        /// resource (across resource groups).
        /// </summary>
        [JsonProperty(PropertyName = "tags")]
        public IDictionary<string, string> Tags { get; set; }

    }
}

