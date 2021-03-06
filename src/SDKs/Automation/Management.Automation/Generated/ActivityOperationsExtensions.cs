// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

// Warning: This code was generated by a tool.
// 
// Changes to this file may cause incorrect behavior and will be lost if the
// code is regenerated.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Automation;
using Microsoft.Azure.Management.Automation.Models;

namespace Microsoft.Azure.Management.Automation
{
    public static partial class ActivityOperationsExtensions
    {
        /// <summary>
        /// Retrieve the activity in the module identified by module name and
        /// activity name.  (see
        /// http://aka.ms/azureautomationsdk/activityoperations for more
        /// information)
        /// </summary>
        /// <param name='operations'>
        /// Reference to the
        /// Microsoft.Azure.Management.Automation.IActivityOperations.
        /// </param>
        /// <param name='resourceGroupName'>
        /// Required. The name of the resource group
        /// </param>
        /// <param name='automationAccount'>
        /// Required. The automation account name.
        /// </param>
        /// <param name='moduleName'>
        /// Required. The name of module.
        /// </param>
        /// <param name='activityName'>
        /// Required. The name of activity.
        /// </param>
        /// <returns>
        /// The response model for the get activity operation.
        /// </returns>
        public static ActivityGetResponse Get(this IActivityOperations operations, string resourceGroupName, string automationAccount, string moduleName, string activityName)
        {
            return Task.Factory.StartNew((object s) => 
            {
                return ((IActivityOperations)s).GetAsync(resourceGroupName, automationAccount, moduleName, activityName);
            }
            , operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
        }
        
        /// <summary>
        /// Retrieve the activity in the module identified by module name and
        /// activity name.  (see
        /// http://aka.ms/azureautomationsdk/activityoperations for more
        /// information)
        /// </summary>
        /// <param name='operations'>
        /// Reference to the
        /// Microsoft.Azure.Management.Automation.IActivityOperations.
        /// </param>
        /// <param name='resourceGroupName'>
        /// Required. The name of the resource group
        /// </param>
        /// <param name='automationAccount'>
        /// Required. The automation account name.
        /// </param>
        /// <param name='moduleName'>
        /// Required. The name of module.
        /// </param>
        /// <param name='activityName'>
        /// Required. The name of activity.
        /// </param>
        /// <returns>
        /// The response model for the get activity operation.
        /// </returns>
        public static Task<ActivityGetResponse> GetAsync(this IActivityOperations operations, string resourceGroupName, string automationAccount, string moduleName, string activityName)
        {
            return operations.GetAsync(resourceGroupName, automationAccount, moduleName, activityName, CancellationToken.None);
        }
        
        /// <summary>
        /// Retrieve a list of activities in the module identified by module
        /// name.  (see http://aka.ms/azureautomationsdk/activityoperations
        /// for more information)
        /// </summary>
        /// <param name='operations'>
        /// Reference to the
        /// Microsoft.Azure.Management.Automation.IActivityOperations.
        /// </param>
        /// <param name='resourceGroupName'>
        /// Required. The name of the resource group
        /// </param>
        /// <param name='automationAccount'>
        /// Required. The automation account name.
        /// </param>
        /// <param name='moduleName'>
        /// Required. The name of module.
        /// </param>
        /// <returns>
        /// The response model for the list activity operation.
        /// </returns>
        public static ActivityListResponse List(this IActivityOperations operations, string resourceGroupName, string automationAccount, string moduleName)
        {
            return Task.Factory.StartNew((object s) => 
            {
                return ((IActivityOperations)s).ListAsync(resourceGroupName, automationAccount, moduleName);
            }
            , operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
        }
        
        /// <summary>
        /// Retrieve a list of activities in the module identified by module
        /// name.  (see http://aka.ms/azureautomationsdk/activityoperations
        /// for more information)
        /// </summary>
        /// <param name='operations'>
        /// Reference to the
        /// Microsoft.Azure.Management.Automation.IActivityOperations.
        /// </param>
        /// <param name='resourceGroupName'>
        /// Required. The name of the resource group
        /// </param>
        /// <param name='automationAccount'>
        /// Required. The automation account name.
        /// </param>
        /// <param name='moduleName'>
        /// Required. The name of module.
        /// </param>
        /// <returns>
        /// The response model for the list activity operation.
        /// </returns>
        public static Task<ActivityListResponse> ListAsync(this IActivityOperations operations, string resourceGroupName, string automationAccount, string moduleName)
        {
            return operations.ListAsync(resourceGroupName, automationAccount, moduleName, CancellationToken.None);
        }
        
        /// <summary>
        /// Retrieve next list of activities in the module identified by module
        /// name.  (see http://aka.ms/azureautomationsdk/activityoperations
        /// for more information)
        /// </summary>
        /// <param name='operations'>
        /// Reference to the
        /// Microsoft.Azure.Management.Automation.IActivityOperations.
        /// </param>
        /// <param name='nextLink'>
        /// Required. The link to retrieve next set of items.
        /// </param>
        /// <returns>
        /// The response model for the list activity operation.
        /// </returns>
        public static ActivityListResponse ListNext(this IActivityOperations operations, string nextLink)
        {
            return Task.Factory.StartNew((object s) => 
            {
                return ((IActivityOperations)s).ListNextAsync(nextLink);
            }
            , operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
        }
        
        /// <summary>
        /// Retrieve next list of activities in the module identified by module
        /// name.  (see http://aka.ms/azureautomationsdk/activityoperations
        /// for more information)
        /// </summary>
        /// <param name='operations'>
        /// Reference to the
        /// Microsoft.Azure.Management.Automation.IActivityOperations.
        /// </param>
        /// <param name='nextLink'>
        /// Required. The link to retrieve next set of items.
        /// </param>
        /// <returns>
        /// The response model for the list activity operation.
        /// </returns>
        public static Task<ActivityListResponse> ListNextAsync(this IActivityOperations operations, string nextLink)
        {
            return operations.ListNextAsync(nextLink, CancellationToken.None);
        }
    }
}
