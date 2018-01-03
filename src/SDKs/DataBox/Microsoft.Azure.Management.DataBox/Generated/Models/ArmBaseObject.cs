
namespace Microsoft.Azure.Management.DataBox.Models
{
    using Azure;
    using Management;
    using DataBox;
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// Base class for all objects under resource.
    /// </summary>
    public partial class ArmBaseObject
    {
        /// <summary>
        /// Initializes a new instance of the ArmBaseObject class.
        /// </summary>
        public ArmBaseObject() { }

        /// <summary>
        /// Initializes a new instance of the ArmBaseObject class.
        /// </summary>
        /// <param name="name">Name of the object.</param>
        /// <param name="id">Id of the object.</param>
        /// <param name="type">Type of the object.</param>
        public ArmBaseObject(string name = default(string), string id = default(string), string type = default(string))
        {
            Name = name;
            Id = id;
            Type = type;
        }

        /// <summary>
        /// Gets name of the object.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; protected set; }

        /// <summary>
        /// Gets id of the object.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; protected set; }

        /// <summary>
        /// Gets type of the object.
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; protected set; }

    }
}

