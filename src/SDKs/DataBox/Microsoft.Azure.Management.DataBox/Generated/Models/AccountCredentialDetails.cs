
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
    /// Credential details of the account.
    /// </summary>
    public partial class AccountCredentialDetails
    {
        /// <summary>
        /// Initializes a new instance of the AccountCredentialDetails class.
        /// </summary>
        public AccountCredentialDetails() { }

        /// <summary>
        /// Initializes a new instance of the AccountCredentialDetails class.
        /// </summary>
        /// <param name="shareCredentialDetails">Per share level unencrypted
        /// access credentials.</param>
        /// <param name="accountName">Name of the account.</param>
        public AccountCredentialDetails(IList<ShareCredentialDetails> shareCredentialDetails, string accountName = default(string))
        {
            AccountName = accountName;
            ShareCredentialDetails = shareCredentialDetails;
        }

        /// <summary>
        /// Gets or sets name of the account.
        /// </summary>
        [JsonProperty(PropertyName = "accountName")]
        public string AccountName { get; set; }

        /// <summary>
        /// Gets or sets per share level unencrypted access credentials.
        /// </summary>
        [JsonProperty(PropertyName = "shareCredentialDetails")]
        public IList<ShareCredentialDetails> ShareCredentialDetails { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (ShareCredentialDetails == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "ShareCredentialDetails");
            }
        }
    }
}

