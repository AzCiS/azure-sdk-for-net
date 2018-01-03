
namespace Microsoft.Azure.Management.DataBox.Models
{
    using Azure;
    using Management;
    using DataBox;
    using Rest;
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// Details for the destination account.
    /// </summary>
    public partial class DestinationAccountDetails
    {
        /// <summary>
        /// Initializes a new instance of the DestinationAccountDetails class.
        /// </summary>
        public DestinationAccountDetails() { }

        /// <summary>
        /// Initializes a new instance of the DestinationAccountDetails class.
        /// </summary>
        /// <param name="accountId">Destination storage account id.</param>
        /// <param name="accountType">Destination account type. Possible values
        /// include: 'UnknownType', 'GeneralPurposeStorage',
        /// 'BlobStorage'</param>
        public DestinationAccountDetails(string accountId, AccountType accountType)
        {
            AccountId = accountId;
            AccountType = accountType;
        }

        /// <summary>
        /// Gets or sets destination storage account id.
        /// </summary>
        [JsonProperty(PropertyName = "accountId")]
        public string AccountId { get; set; }

        /// <summary>
        /// Gets or sets destination account type. Possible values include:
        /// 'UnknownType', 'GeneralPurposeStorage', 'BlobStorage'
        /// </summary>
        [JsonProperty(PropertyName = "accountType")]
        public AccountType AccountType { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (AccountId == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "AccountId");
            }
        }
    }
}

