// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;
using Microsoft.Azure.Management.StorSimple8000Series;
using Microsoft.Azure.Management.StorSimple8000Series.Models;
using Microsoft.Rest.Azure.OData;
using SSModels = Microsoft.Azure.Management.StorSimple8000Series.Models;
using Microsoft.Rest.Azure;

namespace StorSimple8000Series.Tests
{
    public static partial class Helpers
    {
        public static IPage<AvailableProviderOperation> GetOperations(StorSimple8000SeriesTestBase testBase)
        {
            var operations = testBase.Client.Operations.List();

            Assert.True(operations != null, "List call for Operations was not successful.");

            return operations;
        }

        public static IEnumerable<Feature> GetFeatures(StorSimple8000SeriesTestBase testBase, string deviceName = null)
        {
            var oDataQuery = string.IsNullOrEmpty(deviceName) ? null : GetODataQueryForFeatures(testBase, deviceName);
            var features = testBase.Client.Managers.ListFeatureSupportStatus(testBase.ResourceGroupName, testBase.ManagerName, oDataQuery);

            Assert.True(features != null && features.Count() != 0, "Features call was not successful.");

            return features;
        }

        private static ODataQuery<FeatureFilter> GetODataQueryForFeatures(StorSimple8000SeriesTestBase testBase, string deviceName)
        {
            var device = testBase.Client.Devices.Get(deviceName.GetDoubleEncoded(), testBase.ResourceGroupName, testBase.ManagerName);

            Expression<Func<FeatureFilter, bool>> filter = 
                f => (f.DeviceId == device.Id);

            ODataQuery<FeatureFilter> odataQuery = new ODataQuery<FeatureFilter>(filter);

            return odataQuery;
        }
    }
}