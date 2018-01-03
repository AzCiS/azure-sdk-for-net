
namespace Microsoft.Azure.Management.DataBox
{
    using Azure;
    using Management;
    using Rest;
    using Rest.Azure;
    using Models;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension methods for ListSecretsOperations.
    /// </summary>
    public static partial class ListSecretsOperationsExtensions
    {
            /// <summary>
            /// This method gets the unencrypted secrets related to the job.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The Resource Group Name
            /// </param>
            /// <param name='jobName'>
            /// The name of the job Resource within the specified resource group. job names
            /// must be between 3 and 24 characters in length and use any alphanumeric and
            /// underscore only
            /// </param>
            public static UnencryptedSecrets ListByJobs(this IListSecretsOperations operations, string resourceGroupName, string jobName)
            {
                return operations.ListByJobsAsync(resourceGroupName, jobName).GetAwaiter().GetResult();
            }

            /// <summary>
            /// This method gets the unencrypted secrets related to the job.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='resourceGroupName'>
            /// The Resource Group Name
            /// </param>
            /// <param name='jobName'>
            /// The name of the job Resource within the specified resource group. job names
            /// must be between 3 and 24 characters in length and use any alphanumeric and
            /// underscore only
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<UnencryptedSecrets> ListByJobsAsync(this IListSecretsOperations operations, string resourceGroupName, string jobName, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ListByJobsWithHttpMessagesAsync(resourceGroupName, jobName, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

    }
}

