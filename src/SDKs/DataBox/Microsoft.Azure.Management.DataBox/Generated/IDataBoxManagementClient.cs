
namespace Microsoft.Azure.Management.DataBox
{
    using Azure;
    using Management;
    using Rest;
    using Rest.Azure;
    using Models;
    using Newtonsoft.Json;

    /// <summary>
    /// </summary>
    public partial interface IDataBoxManagementClient : System.IDisposable
    {
        /// <summary>
        /// The base URI of the service.
        /// </summary>
        System.Uri BaseUri { get; set; }

        /// <summary>
        /// Gets or sets json serialization settings.
        /// </summary>
        Newtonsoft.Json.JsonSerializerSettings SerializationSettings { get; }

        /// <summary>
        /// Gets or sets json deserialization settings.
        /// </summary>
        Newtonsoft.Json.JsonSerializerSettings DeserializationSettings { get; }

        /// <summary>
        /// Credentials needed for the client to connect to Azure.
        /// </summary>
        ServiceClientCredentials Credentials { get; }

        /// <summary>
        /// The API Version
        /// </summary>
        string ApiVersion { get; }

        /// <summary>
        /// The location of the resource
        /// </summary>
        string Location { get; set; }

        /// <summary>
        /// The Subscription Id
        /// </summary>
        string SubscriptionId { get; set; }

        /// <summary>
        /// Gets or sets the preferred language for the response.
        /// </summary>
        string AcceptLanguage { get; set; }

        /// <summary>
        /// Gets or sets the retry timeout in seconds for Long Running
        /// Operations. Default value is 30.
        /// </summary>
        int? LongRunningOperationRetryTimeout { get; set; }

        /// <summary>
        /// When set to true a unique x-ms-client-request-id value is generated
        /// and included in each request. Default is true.
        /// </summary>
        bool? GenerateClientRequestId { get; set; }


        /// <summary>
        /// Gets the IOperations.
        /// </summary>
        IOperations Operations { get; }

        /// <summary>
        /// Gets the IJobsOperations.
        /// </summary>
        IJobsOperations Jobs { get; }

        /// <summary>
        /// Gets the IServiceOperations.
        /// </summary>
        IServiceOperations Service { get; }

        /// <summary>
        /// Gets the IListSecretsOperations.
        /// </summary>
        IListSecretsOperations ListSecrets { get; }

    }
}

