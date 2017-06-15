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
        /// <summary>
        /// Helper method to trigger failover
        /// </summary>
        /// <param name="testBase"></param>
        /// <param name="sourceDeviceName"></param>
        /// <param name="targetDeviceName"></param>
        /// <param name="volumeContainerNames"></param>
        public static void Failover(
                StorSimple8000SeriesTestBase testBase,
                string sourceDeviceName,
                string targetDeviceName,
                IList<string> volumeContainerNames)
        {
            Assert.False(sourceDeviceName.Equals(
                targetDeviceName,
                StringComparison.CurrentCultureIgnoreCase));

            var devices = testBase.Client.Devices.ListByManager(
                                testBase.ResourceGroupName,
                                testBase.ManagerName);

            Assert.NotNull(devices);
            Assert.NotEmpty(devices);

            var sourceDevice = devices.FirstOrDefault(
                        d => d.Name.Equals(
                            sourceDeviceName,
                            StringComparison.CurrentCultureIgnoreCase));

            Assert.NotNull(sourceDevice);

            var targetDevice = devices.FirstOrDefault(
                        d => d.Name.Equals(
                            targetDeviceName,
                            StringComparison.CurrentCultureIgnoreCase));

            Assert.NotNull(targetDevice);

            var volumeContainers = testBase.Client.VolumeContainers.ListByDevice(
                                sourceDevice.Name.GetDoubleEncoded(),
                                testBase.ResourceGroupName,
                                testBase.ManagerName);

            Assert.NotNull(volumeContainers);
            Assert.NotEmpty(volumeContainers);

            var volumeContainersToFailover = volumeContainers.Where(v =>
                        volumeContainerNames.Contains(v.Name, new StringIgnoreCaseEqualityComparer()));

            var volumeContainerIds = new List<string>();
            foreach (var vc in volumeContainersToFailover)
            {
                volumeContainerIds.Add(vc.Id);
            }

            // Assert that volume containers are not already on target device
            var volumeContainersOnTargetDevice = testBase.Client.VolumeContainers.ListByDevice(
                                                targetDevice.Name.GetDoubleEncoded(),
                                                testBase.ResourceGroupName,
                                                testBase.ManagerName);

            Assert.NotNull(volumeContainersOnTargetDevice);

            var volumeContainersAlreadyOnTargetDevice = volumeContainersOnTargetDevice.Where(v =>
                                    volumeContainerNames.Contains(v.Name, new StringIgnoreCaseEqualityComparer()));

            Assert.NotNull(volumeContainersAlreadyOnTargetDevice);
            Assert.Empty(volumeContainersAlreadyOnTargetDevice);

            // Create a failover request
            FailoverRequest failoverRequest = new FailoverRequest()
            {
                TargetDeviceId = targetDevice.Id,
                VolumeContainers = volumeContainerIds
            };

            // Trigger failover
            testBase.Client.Devices.Failover(
                                sourceDevice.Name.GetDoubleEncoded(),
                                failoverRequest,
                                testBase.ResourceGroupName,
                                testBase.ManagerName);

            // Query volume containers from target device after failover
            var volumeContainersAfterFailover = testBase.Client.VolumeContainers.ListByDevice(
                                 targetDevice.Name.GetDoubleEncoded(),
                                 testBase.ResourceGroupName,
                                 testBase.ManagerName);

            Assert.NotNull(volumeContainersAfterFailover);

            // Assuming the volume container names are the same on the target device
            var failedOverVolumeContainers = volumeContainersAfterFailover.Where(v =>
                                    volumeContainerNames.Contains(v.Name, new StringIgnoreCaseEqualityComparer()));

            Assert.NotNull(failedOverVolumeContainers);
            Assert.NotEmpty(failedOverVolumeContainers);
        }
    }
}