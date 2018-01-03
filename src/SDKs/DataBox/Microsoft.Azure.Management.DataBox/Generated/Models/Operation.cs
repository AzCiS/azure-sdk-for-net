
namespace Microsoft.Azure.Management.DataBox.Models
{
    using Azure;
    using Management;
    using DataBox;
    using Rest;
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// Operation entity.
    /// </summary>
    public partial class Operation
    {
        /// <summary>
        /// Initializes a new instance of the Operation class.
        /// </summary>
        public Operation() { }

        /// <summary>
        /// Initializes a new instance of the Operation class.
        /// </summary>
        /// <param name="name">Name of the operation. Format:
        /// {resourceProviderNamespace}/{resourceType}/{read|write|delete|action}</param>
        /// <param name="display">Operation display values.</param>
        /// <param name="properties">Operation properties.</param>
        /// <param name="origin">Origin of the operation. Can be :
        /// user|system|user,system</param>
        public Operation(string name, OperationDisplay display, object properties, string origin)
        {
            Name = name;
            Display = display;
            Properties = properties;
            Origin = origin;
        }

        /// <summary>
        /// Gets or sets name of the operation. Format:
        /// {resourceProviderNamespace}/{resourceType}/{read|write|delete|action}
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets operation display values.
        /// </summary>
        [JsonProperty(PropertyName = "display")]
        public OperationDisplay Display { get; set; }

        /// <summary>
        /// Gets or sets operation properties.
        /// </summary>
        [JsonProperty(PropertyName = "properties")]
        public object Properties { get; set; }

        /// <summary>
        /// Gets or sets origin of the operation. Can be :
        /// user|system|user,system
        /// </summary>
        [JsonProperty(PropertyName = "origin")]
        public string Origin { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (Name == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Name");
            }
            if (Display == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Display");
            }
            if (Properties == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Properties");
            }
            if (Origin == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Origin");
            }
        }
    }
}

