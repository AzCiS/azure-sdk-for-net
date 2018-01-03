
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
    /// Extension methods for ServiceOperations.
    /// </summary>
    public static partial class ServiceOperationsExtensions
    {
            /// <summary>
            /// This method returns the list of supported service regions and regions for
            /// destination storage accounts
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='regionAvailabilityInput'>
            /// Country Code and Device Type.
            /// </param>
            public static RegionAvailabilityResponse RegionAvailability(this IServiceOperations operations, RegionAvailabilityInput regionAvailabilityInput)
            {
                return operations.RegionAvailabilityAsync(regionAvailabilityInput).GetAwaiter().GetResult();
            }

            /// <summary>
            /// This method returns the list of supported service regions and regions for
            /// destination storage accounts
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='regionAvailabilityInput'>
            /// Country Code and Device Type.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<RegionAvailabilityResponse> RegionAvailabilityAsync(this IServiceOperations operations, RegionAvailabilityInput regionAvailabilityInput, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.RegionAvailabilityWithHttpMessagesAsync(regionAvailabilityInput, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// This method returns the health of partner services.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            public static ServiceHealthResponseList GetServiceHealth(this IServiceOperations operations)
            {
                return operations.GetServiceHealthAsync().GetAwaiter().GetResult();
            }

            /// <summary>
            /// This method returns the health of partner services.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<ServiceHealthResponseList> GetServiceHealthAsync(this IServiceOperations operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetServiceHealthWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// This method validates the customer shipping address and provide alternate
            /// addresses if any.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='validateAddress'>
            /// Shipping address of the customer.
            /// </param>
            public static AddressValidationOutput ValidateAddressMethod(this IServiceOperations operations, ValidateAddress validateAddress)
            {
                return operations.ValidateAddressMethodAsync(validateAddress).GetAwaiter().GetResult();
            }

            /// <summary>
            /// This method validates the customer shipping address and provide alternate
            /// addresses if any.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='validateAddress'>
            /// Shipping address of the customer.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<AddressValidationOutput> ValidateAddressMethodAsync(this IServiceOperations operations, ValidateAddress validateAddress, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.ValidateAddressMethodWithHttpMessagesAsync(validateAddress, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

    }
}

