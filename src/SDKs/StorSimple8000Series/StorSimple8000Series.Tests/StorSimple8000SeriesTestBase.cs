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
    class StorSimple8000SeriesTestBase : TestBase
    {
        private const string SubIdKey = "SubId";
        private const string DefaultResourceGroupName = "ResourceGroupForSDKTest";
        private const string DefaultManagerName = "ManagerForSDKTest";
        private const string DefaultStorageAccountName = "StorageAccountForSDKTest";
        private const string DefaultStorageAccountAccessKey = "<accessKey>";
        public string subscriptionId { get; set; }

        public StorSimple8000SeriesManagementClient client { get; set; }

        public string ResourceGroupName { get; set; }
        public string ManagerName { get; set; }
        public string StorageAccountName { get; set; }
        public string StorageAccountAccessKey { get; set; }

        public StorSimple8000SeriesTestBase(MockContext context, string resourceGroupName = DefaultResourceGroupName, string resourceName = DefaultManagerName)
        {
            var testEnv = TestEnvironmentFactory.GetTestEnvironment();
            
            this.client = context.GetServiceClient<StorSimple8000SeriesManagementClient>();

            if (HttpMockServer.Mode == HttpRecorderMode.Record)
            {
                this.subscriptionId = testEnv.SubscriptionId;
                HttpMockServer.Variables[SubIdKey] = subscriptionId;
            }
            else if (HttpMockServer.Mode == HttpRecorderMode.Playback)
            {
                subscriptionId = HttpMockServer.Variables[SubIdKey];
            } 

            Initialize(resourceGroupName, resourceName);
        }

        private void Initialize(string resourceGroupName, string resourceName)
        {
            this.ResourceGroupName = resourceGroupName;
            this.ManagerName = resourceName;
            this.StorageAccountName = DefaultStorageAccountName;
            this.StorageAccountAccessKey = DefaultStorageAccountAccessKey;
        }
    }
}