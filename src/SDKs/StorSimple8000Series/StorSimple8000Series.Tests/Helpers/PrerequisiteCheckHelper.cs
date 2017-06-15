// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.

using System.Collections.Generic;
using System.Linq;
using Xunit;
using Microsoft.Azure.Management.StorSimple8000Series;
using Microsoft.Azure.Management.StorSimple8000Series.Models;

namespace StorSimple8000Series.Tests
{
    public static partial class Helpers
    {
        public static IEnumerable<StorageAccountCredential> CheckAndGetStorageAccountCredentials(StorSimple8000SeriesTestBase testBase, int requiredCount)
        {
            var sacs = testBase.Client.StorageAccountCredentials.ListByManager(
                testBase.ResourceGroupName,
                testBase.ManagerName);

            Assert.True(sacs.Count() >= requiredCount, string.Format("Could not found minimum Storage Account Credentials: Required={0}, ActuallyFound={1}", requiredCount, sacs.Count()));

            return sacs;
        }

        public static IEnumerable<AccessControlRecord> CheckAndGetAccessControlRecords(StorSimple8000SeriesTestBase testBase, int requiredCount)
        {
            var accessControlRecords = testBase.Client.AccessControlRecords.ListByManager(
                testBase.ResourceGroupName,
                testBase.ManagerName);

            Assert.True(accessControlRecords.Count() >= requiredCount, string.Format("Could not found minimum access control records: Required={0}, ActuallyFound={1}", requiredCount, accessControlRecords.Count()));

            return accessControlRecords;
        }

        public static IEnumerable<BandwidthSetting> CheckAndGetBandwidthSettings(StorSimple8000SeriesTestBase testBase, int requiredCount)
        {
            var bandwidthSettings = testBase.Client.BandwidthSettings.ListByManager(
                testBase.ResourceGroupName,
                testBase.ManagerName);

            Assert.True(bandwidthSettings.Count() >= requiredCount, string.Format("Could not found minimum Bandwidth settings: Required={0}, ActuallyFound={1}", requiredCount, bandwidthSettings.Count()));

            return bandwidthSettings;
        }

        /// <summary>
        /// Checks if minimum number of configured devices required for the testcase exists. If yes, returns the devices.
        /// </summary>
        public static IEnumerable<Device> CheckAndGetConfiguredDevices(StorSimple8000SeriesTestBase testBase, int requiredCount)
        {
            var devices = testBase.Client.Devices.ListByManager(testBase.ResourceGroupName, testBase.ManagerName);

            var configuredDeviceCount = 0;
            var configuredDeviceNames = new List<Device>();

            foreach (var device in devices)
            {
                if (device.Status == DeviceStatus.Online)
                {
                    configuredDeviceCount++;
                    configuredDeviceNames.Add(device);
                }
            }

            Assert.True(configuredDeviceCount >= requiredCount, string.Format("Could not found minimum configured devices: Required={0}, ActuallyFound={1}", requiredCount, configuredDeviceCount));

            return configuredDeviceNames;
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
    }
}