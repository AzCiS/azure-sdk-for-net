
namespace Microsoft.Azure.Management.DataBox.Models
{
    using Azure;
    using Management;
    using DataBox;
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// The sku type.
    /// </summary>
    public partial class Sku
    {
        /// <summary>
        /// Initializes a new instance of the Sku class.
        /// </summary>
        public Sku() { }

        /// <summary>
        /// Initializes a new instance of the Sku class.
        /// </summary>
        /// <param name="name">The sku name. Optional for job resource creation
        /// and update.</param>
        /// <param name="tier">The sku tier. This is based on the SKU
        /// name.</param>
        public Sku(string name = default(string), string tier = default(string))
        {
            Name = name;
            Tier = tier;
        }

        /// <summary>
        /// Gets or sets the sku name. Optional for job resource creation and
        /// update.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the sku tier. This is based on the SKU name.
        /// </summary>
        [JsonProperty(PropertyName = "tier")]
        public string Tier { get; set; }

    }
}

