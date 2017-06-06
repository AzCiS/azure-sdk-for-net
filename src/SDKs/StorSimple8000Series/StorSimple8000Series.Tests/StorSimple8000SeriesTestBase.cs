// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.

using Microsoft.Azure.Management.StorSimple8000Series;
using Microsoft.Azure.Management.StorSimple8000Series.Models;
using Microsoft.Azure.Test.HttpRecorder;
using Microsoft.Rest.ClientRuntime.Azure.TestFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace StorSimple8000Series.Tests
{
    public class StorSimple8000SeriesTestBase : TestBase
    {
        private const string SubIdKey = "SubId";
        private string subscriptionId { get; set; }

        public StorSimple8000SeriesManagementClient Client { get; set; }

        public string ResourceGroupName { get; set; }
        public string ManagerName { get; set; }

        public StorSimple8000SeriesTestBase(MockContext context, string resourceGroupName = TestConstants.DefaultResourceGroupName, string managerName = TestConstants.DefaultManagerName)
        {
            var testEnv = TestEnvironmentFactory.GetTestEnvironment();
            
            this.Client = context.GetServiceClient<StorSimple8000SeriesManagementClient>();

            if (HttpMockServer.Mode == HttpRecorderMode.Record)
            {
                this.subscriptionId = testEnv.SubscriptionId;
                HttpMockServer.Variables[SubIdKey] = subscriptionId;
            }
            else if (HttpMockServer.Mode == HttpRecorderMode.Playback)
            {
                subscriptionId = HttpMockServer.Variables[SubIdKey];
            } 

            Initialize(resourceGroupName, managerName);
        }

        private void Initialize(string resourceGroupName, string managerName)
        {
            this.ResourceGroupName = resourceGroupName;
            this.ManagerName = managerName;
        }
    }
}