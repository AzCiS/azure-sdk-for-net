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

namespace StorSimple8000Series.Tests
{
    public static partial class Helpers
    {
        #region GetMetrics Calls
        public static IEnumerable<Metrics> GetManagerMetrics(StorSimple8000SeriesTestBase testBase, MetricDefinition metricDefinition)
        {
            var managerMetrics = testBase.Client.Managers.ListMetrics(
                GenerateOdataFilterForMetric(metricDefinition),
                testBase.ResourceGroupName,
                testBase.ManagerName);

            return managerMetrics;
        }

        public static IEnumerable<Metrics> GetDeviceMetrics(StorSimple8000SeriesTestBase testBase, string deviceName, MetricDefinition metricDefinition)
        {
            var deviceMetrics = testBase.Client.Devices.ListMetrics(
                GenerateOdataFilterForMetric(metricDefinition),
                deviceName.GetDoubleEncoded(),
                testBase.ResourceGroupName,
                testBase.ManagerName);

            return deviceMetrics;
        }

        public static IEnumerable<Metrics> GetVolumeContainerMetrics(StorSimple8000SeriesTestBase testBase, string deviceName, string volumeContainerName, MetricDefinition metricDefinition)
        {
            var vcMetrics = testBase.Client.VolumeContainers.ListMetrics(
                GenerateOdataFilterForMetric(metricDefinition),
                deviceName.GetDoubleEncoded(),
                volumeContainerName.GetDoubleEncoded(),
                testBase.ResourceGroupName,
                testBase.ManagerName);

            return vcMetrics;
        }

        public static IEnumerable<Metrics> GetVolumeMetrics(StorSimple8000SeriesTestBase testBase, string deviceName, string volumeContainerName, string volumeName, MetricDefinition metricDefinition)
        {
            var volMetrics = testBase.Client.Volumes.ListMetrics(
                GenerateOdataFilterForMetric(metricDefinition),
                deviceName.GetDoubleEncoded(),
                volumeContainerName.GetDoubleEncoded(),
                volumeName.GetDoubleEncoded(),
                testBase.ResourceGroupName,
                testBase.ManagerName);

            return volMetrics;

        }

        private static ODataQuery<MetricFilter> GenerateOdataFilterForMetric(MetricDefinition metricDefinition)
        {
            Expression<Func<MetricFilter, bool>> filter =
                metricFilter => (metricFilter.Name.Value == metricDefinition.Name.Value
                && metricFilter.TimeGrain == metricDefinition.MetricAvailabilities.First().TimeGrain
                && metricFilter.StartTime >= TestConstants.MetricsStartTime
                && metricFilter.EndTime <= TestConstants.MetricsEndTime
                && metricFilter.Category == metricDefinition.Category);

            ODataQuery<MetricFilter> odataQuery = new ODataQuery<MetricFilter>(filter);
            odataQuery.Expand = "name";

            return odataQuery;
        }
        #endregion

        #region GetMetricDefinition Calls 
        public static IEnumerable<MetricDefinition> GetManagerMetricDefinitions(StorSimple8000SeriesTestBase testBase)
        {
            var managerMetrics = testBase.Client.Managers.ListMetricDefinition(
                testBase.ResourceGroupName,
                testBase.ManagerName);

            return managerMetrics;
        }
        public static IEnumerable<MetricDefinition> GetDeviceMetricDefinition(StorSimple8000SeriesTestBase testBase, string deviceName)
        {
            var deviceMetrics = testBase.Client.Devices.ListMetricDefinition(
                deviceName.GetDoubleEncoded(),
                testBase.ResourceGroupName,
                testBase.ManagerName);

            return deviceMetrics;
        }

        public static IEnumerable<MetricDefinition> GetVolumeContainerMetricDefinition(StorSimple8000SeriesTestBase testBase, string deviceName, string volumeContainerName)
        {
            var volumeContainerMetrics = testBase.Client.VolumeContainers.ListMetricDefinition(
                deviceName.GetDoubleEncoded(),
                volumeContainerName.GetDoubleEncoded(),
                testBase.ResourceGroupName,
                testBase.ManagerName);

            return volumeContainerMetrics;
        }

        public static IEnumerable<MetricDefinition> GetVolumeMetricDefinition(StorSimple8000SeriesTestBase testBase, string deviceName, string volumeContainerName, string volumeName)
        {
            return testBase.Client.Volumes.ListMetricDefinition(
                deviceName.GetDoubleEncoded(),
                volumeContainerName.GetDoubleEncoded(),
                volumeName.GetDoubleEncoded(),
                testBase.ResourceGroupName,
                testBase.ManagerName);
        }
        #endregion
    }
}