
namespace Microsoft.Azure.Management.DataBox.Models
{
    using Azure;
    using Management;
    using DataBox;
    using Rest;
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// Top level error for the job.
    /// </summary>
    public partial class Error
    {
        /// <summary>
        /// Initializes a new instance of the Error class.
        /// </summary>
        public Error() { }

        /// <summary>
        /// Initializes a new instance of the Error class.
        /// </summary>
        /// <param name="code">Error code that can be used to programmatically
        /// identify the error.</param>
        /// <param name="message">Describes the error in detail and provides
        /// debugging information.</param>
        public Error(string code, string message = default(string))
        {
            Code = code;
            Message = message;
        }

        /// <summary>
        /// Gets or sets error code that can be used to programmatically
        /// identify the error.
        /// </summary>
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets describes the error in detail and provides debugging
        /// information.
        /// </summary>
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (Code == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Code");
            }
        }
    }
}

