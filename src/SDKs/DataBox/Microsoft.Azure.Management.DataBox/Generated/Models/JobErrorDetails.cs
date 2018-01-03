
namespace Microsoft.Azure.Management.DataBox.Models
{
    using Azure;
    using Management;
    using DataBox;
    using Newtonsoft.Json;
    using System.Linq;

    /// <summary>
    /// Job Error Details for providing the information and recommended action.
    /// </summary>
    public partial class JobErrorDetails
    {
        /// <summary>
        /// Initializes a new instance of the JobErrorDetails class.
        /// </summary>
        public JobErrorDetails() { }

        /// <summary>
        /// Initializes a new instance of the JobErrorDetails class.
        /// </summary>
        /// <param name="errorMessage">Message for the error.</param>
        /// <param name="errorCode">Code for the error.</param>
        /// <param name="recommendedAction">Recommended action for the
        /// error.</param>
        /// <param name="exceptionMessage">Contains the non localized exception
        /// message</param>
        public JobErrorDetails(string errorMessage = default(string), int? errorCode = default(int?), string recommendedAction = default(string), string exceptionMessage = default(string))
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
            RecommendedAction = recommendedAction;
            ExceptionMessage = exceptionMessage;
        }

        /// <summary>
        /// Gets or sets message for the error.
        /// </summary>
        [JsonProperty(PropertyName = "errorMessage")]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets code for the error.
        /// </summary>
        [JsonProperty(PropertyName = "errorCode")]
        public int? ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets recommended action for the error.
        /// </summary>
        [JsonProperty(PropertyName = "recommendedAction")]
        public string RecommendedAction { get; set; }

        /// <summary>
        /// Gets or sets contains the non localized exception message
        /// </summary>
        [JsonProperty(PropertyName = "exceptionMessage")]
        public string ExceptionMessage { get; set; }

    }
}

