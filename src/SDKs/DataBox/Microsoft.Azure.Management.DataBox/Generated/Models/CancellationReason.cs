
namespace Microsoft.Azure.Management.DataBox.Models
{
    using Azure;
    using Management;
    using DataBox;
    using Rest;
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// Reason for cancellation.
    /// </summary>
    public partial class CancellationReason
    {
        /// <summary>
        /// Initializes a new instance of the CancellationReason class.
        /// </summary>
        public CancellationReason() { }

        /// <summary>
        /// Initializes a new instance of the CancellationReason class.
        /// </summary>
        /// <param name="reason">Reason for cancellation.</param>
        public CancellationReason(string reason)
        {
            Reason = reason;
        }

        /// <summary>
        /// Gets or sets reason for cancellation.
        /// </summary>
        [JsonProperty(PropertyName = "reason")]
        public string Reason { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (Reason == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Reason");
            }
        }
    }
}

