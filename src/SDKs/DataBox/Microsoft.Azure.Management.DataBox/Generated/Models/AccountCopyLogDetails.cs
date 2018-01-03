
namespace Microsoft.Azure.Management.DataBox.Models
{
    using Azure;
    using Management;
    using DataBox;
    using Rest;
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// Copy log details for an storage account
    /// </summary>
    [Newtonsoft.Json.JsonObject("Pod")]
    public partial class AccountCopyLogDetails : CopyLogDetails
    {
        /// <summary>
        /// Initializes a new instance of the AccountCopyLogDetails class.
        /// </summary>
        public AccountCopyLogDetails() { }

        /// <summary>
        /// Initializes a new instance of the AccountCopyLogDetails class.
        /// </summary>
        /// <param name="accountName">Destination account name.</param>
        /// <param name="copyLogLink">Link for copy logs.</param>
        public AccountCopyLogDetails(string accountName, string copyLogLink)
        {
            AccountName = accountName;
            CopyLogLink = copyLogLink;
        }

        /// <summary>
        /// Gets or sets destination account name.
        /// </summary>
        [JsonProperty(PropertyName = "accountName")]
        public string AccountName { get; set; }

        /// <summary>
        /// Gets or sets link for copy logs.
        /// </summary>
        [JsonProperty(PropertyName = "copyLogLink")]
        public string CopyLogLink { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (AccountName == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "AccountName");
            }
            if (CopyLogLink == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "CopyLogLink");
            }
        }
    }
}

