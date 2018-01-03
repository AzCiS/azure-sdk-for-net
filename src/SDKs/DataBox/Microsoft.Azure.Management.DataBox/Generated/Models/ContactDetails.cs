
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
    /// Contact Details.
    /// </summary>
    public partial class ContactDetails
    {
        /// <summary>
        /// Initializes a new instance of the ContactDetails class.
        /// </summary>
        public ContactDetails() { }

        /// <summary>
        /// Initializes a new instance of the ContactDetails class.
        /// </summary>
        /// <param name="firstName">First name of the contact person.</param>
        /// <param name="lastName">Last name of the contact person.</param>
        /// <param name="phone">Phone number of the contact person.</param>
        /// <param name="emailList">List of Email-ids to be notified about job
        /// progress.</param>
        /// <param name="phoneExtension">Phone extension number of the contact
        /// person.</param>
        /// <param name="mobile">Mobile number of the contact person.</param>
        public ContactDetails(string firstName, string lastName, string phone, IList<string> emailList, string phoneExtension = default(string), string mobile = default(string))
        {
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            PhoneExtension = phoneExtension;
            Mobile = mobile;
            EmailList = emailList;
        }

        /// <summary>
        /// Gets or sets first name of the contact person.
        /// </summary>
        [JsonProperty(PropertyName = "firstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets last name of the contact person.
        /// </summary>
        [JsonProperty(PropertyName = "lastName")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets phone number of the contact person.
        /// </summary>
        [JsonProperty(PropertyName = "phone")]
        public string Phone { get; set; }

        /// <summary>
        /// Gets or sets phone extension number of the contact person.
        /// </summary>
        [JsonProperty(PropertyName = "phoneExtension")]
        public string PhoneExtension { get; set; }

        /// <summary>
        /// Gets or sets mobile number of the contact person.
        /// </summary>
        [JsonProperty(PropertyName = "mobile")]
        public string Mobile { get; set; }

        /// <summary>
        /// Gets or sets list of Email-ids to be notified about job progress.
        /// </summary>
        [JsonProperty(PropertyName = "emailList")]
        public IList<string> EmailList { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (FirstName == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "FirstName");
            }
            if (LastName == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "LastName");
            }
            if (Phone == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Phone");
            }
            if (EmailList == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "EmailList");
            }
        }
    }
}

