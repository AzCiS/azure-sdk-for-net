// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.

using Microsoft.Azure.Management.StorSimple8000Series;
using Microsoft.Azure.Management.StorSimple8000Series.Models;
using Microsoft.Azure.Test.HttpRecorder;
using Microsoft.Rest.Azure.OData;
using Microsoft.Rest.ClientRuntime.Azure.TestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Xunit;


namespace StorSimple8000Series.Tests
{
    public static class Helpers
    {
        public static string GetDoubleEncoded(this string input)
        {
            return Uri.EscapeDataString(Uri.EscapeDataString(input));
        }

        public static string GenerateRandomName(string prefix)
        {
            var random = new Random();
            return prefix + random.Next();
        }


        #region get entities

        public static IEnumerable<Volume> CheckAndGetVolumes(StorSimple8000SeriesTestBase testBase, string deviceName, string volumeContainerName, int requiredCount)
        {
            var volumes = testBase.Client.Volumes.ListByVolumeContainer(
                                     deviceName,
                                     volumeContainerName,
                                     testBase.ResourceGroupName,
                                     testBase.ManagerName);
            Assert.True(volumes.Count() >= requiredCount, string.Format("Minimum configured volumes: Required={0}, ActuallyFound={1}", requiredCount, volumes.Count()));

            return volumes;
        }

        public static IEnumerable<VolumeContainer> CheckAndGetVolumeContainers(StorSimple8000SeriesTestBase testBase, string deviceName, int requiredCount)
        {
            var volumeContainers = testBase.Client.VolumeContainers.ListByDevice(
                                    deviceName,
                                    testBase.ResourceGroupName,
                                    testBase.ManagerName);
            Assert.True(volumeContainers.Count() >= requiredCount, string.Format("Minimum configured volumeContainers: Required={0}, ActuallyFound={1}", requiredCount, volumeContainers.Count()));

            return volumeContainers;
        }

        public static IEnumerable<Device> CheckAndGetConfiguredDevices(StorSimple8000SeriesTestBase testBase, int requiredCount)
        {
            var devices = testBase.Client.Devices.ListByManager(
                                    testBase.ResourceGroupName,
                                    testBase.ManagerName);
            Assert.True(devices.Count() >= requiredCount, string.Format("Minimum configured devices: Required={0}, ActuallyFound={1}", requiredCount, devices.Count()));

            return devices;
        }

        #endregion

        #region GetMetrics Calls

        public static IEnumerable<Metrics> GetManagerMetrics(StorSimple8000SeriesTestBase testBase, MetricDefinition metricDefinition)
        {
            var managerMetrics = testBase.Client.Managers.ListMetrics(
                testBase.ResourceGroupName, 
                testBase.ManagerName,
                GenerateOdataFiler(metricDefinition));

            return managerMetrics;
        }

        public static IEnumerable<Metrics> GetDeviceMetrics(StorSimple8000SeriesTestBase testBase, string deviceName, MetricDefinition metricDefinition)
        {
            var deviceMetrics =  testBase.Client.Devices.ListMetrics(
                GenerateOdataFiler(metricDefinition),
                deviceName, 
                testBase.ResourceGroupName, 
                testBase.ManagerName);

            return deviceMetrics;
        }

        public static IEnumerable<Metrics> GetVolumeMetrics(StorSimple8000SeriesTestBase testBase, string deviceName, string volumeContainerName, string volumeName, MetricDefinition metricDefinition)
        {
            var volMetrics = testBase.Client.Volumes.ListMetrics(
                GenerateOdataFiler(metricDefinition), 
                deviceName, 
                volumeContainerName, 
                volumeName, 
                testBase.ResourceGroupName, 
                testBase.ManagerName);

            return volMetrics;

        }
        
        public static IEnumerable<Metrics> GetVolumeContainerMetrics(StorSimple8000SeriesTestBase testBase, string deviceName, string volumeContainerName, MetricDefinition metricDefinition)
        {
            var vcMetrics = testBase.Client.VolumeContainers.ListMetrics(
                GenerateOdataFiler(metricDefinition),
                testBase.ResourceGroupName,
                testBase.ManagerName,
                deviceName,
                volumeContainerName);

            return vcMetrics.Value;
        }

        private static ODataQuery<MetricFilter> GenerateOdataFiler(MetricDefinition metricDefinition)
        {
            Expression<Func<MetricFilter, bool>> filter =
                metricFilter => (metricFilter.Name.Value == metricDefinition.Name.Value
                && metricFilter.TimeGrain == metricDefinition.MetricAvailabilities.First().TimeGrain
                && metricFilter.StartTime >= TestConstants.MetricsStartTime
                && metricFilter.EndTime <= TestConstants.MetricsEndTime
                && metricFilter.Category == metricDefinition.Category);

            ODataQuery<MetricFilter> odataQuery = new ODataQuery<MetricFilter>(filter);

            return odataQuery;
        }
        #endregion

        #region GetMetricDefinition Calls 
        public static IEnumerable<MetricDefinition> GetVolumeMetricDefinition(StorSimple8000SeriesTestBase testBase, string deviceName, string volumeContainerName, string volumeName)
        {
            return testBase.Client.Volumes.ListMetricDefinition(
                deviceName, 
                volumeContainerName, 
                volumeName, 
                testBase.ResourceGroupName, 
                testBase.ManagerName);
        }

        public static IEnumerable<MetricDefinition> GetVolumeContainerMetricDefinition(StorSimple8000SeriesTestBase testBase, string deviceName, string volumeContainerName)
        {
            var volumeContainerMetrics = testBase.Client.VolumeContainers.ListMetricDefinition(
                deviceName, 
                volumeContainerName, 
                testBase.ResourceGroupName, 
                testBase.ManagerName);

            return volumeContainerMetrics;
        }

        public static IEnumerable<MetricDefinition> GetDeviceMetricDefinition(StorSimple8000SeriesTestBase testBase, string deviceName)
        {
            var deviceMetrics = testBase.Client.Devices.ListMetricDefinition(
                deviceName, 
                testBase.ResourceGroupName, 
                testBase.ManagerName);

            return deviceMetrics;
        }

        public static IEnumerable<MetricDefinition> GetManagerMetricDefinitions(StorSimple8000SeriesTestBase testBase)
        {
            var managerMetrics = testBase.Client.Managers.ListMetricDefinition(
                testBase.ResourceGroupName, 
                testBase.ManagerName);

            return managerMetrics;
        }

        #endregion  
    }
}
