using System;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.Threading;
using System.Linq.Expressions;
using Xunit;
using Xunit.Sdk;
using Xunit.Abstractions;
using Microsoft.Rest.Azure;
using Microsoft.Rest.ClientRuntime.Azure.TestFramework;
using Microsoft.Azure.Management.StorSimple8000Series;
using Microsoft.Azure.Management.StorSimple8000Series.Models;
using Microsoft.Azure.Test.HttpRecorder;
using Microsoft.Azure.Management.Compute;
using Microsoft.Azure.Management.Network;
using Microsoft.Rest.Azure.OData;

namespace StorSimple8000Series.Tests
{
    public class FailoverTests : StorSimpleTestBase
    {
        public FailoverTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper) { }

        [Fact]
        public void TestFailover()
        {
            //check and get pre-requisites - 2 devices, volumeContainer, volumes
            var device1 = Helpers.CheckAndGetConfiguredDevice(this, TestConstants.DefaultDeviceName);
            var device2 = Helpers.CheckAndGetConfiguredDevice(this, TestConstants.DeviceForFailover);
            var sourceDeviceName = device1.Name;
            var targetDeviceName = device2.Name;
            var volumeContainers = Helpers.CheckAndGetVolumeContainers(this, sourceDeviceName, requiredCount: 1);
            var volumeContainerNames = volumeContainers.Select(vc => vc.Name).ToList();

            try
            {
                // Do failover
                Failover(sourceDeviceName, targetDeviceName, volumeContainerNames);
            }
            catch (Exception e)
            {
                Assert.Null(e);
            }
        }

        /// <summary>
        /// Helper method to trigger failover
        /// </summary>
        private void Failover(string sourceDeviceName, string targetDeviceName, IList<string> volumeContainerNames)
        {
            Assert.False(sourceDeviceName.Equals(
                targetDeviceName,
                StringComparison.CurrentCultureIgnoreCase));

            var devices = this.Client.Devices.ListByManager(
                                this.ResourceGroupName,
                                this.ManagerName);

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

            var volumeContainers = this.Client.VolumeContainers.ListByDevice(
                                sourceDevice.Name.GetDoubleEncoded(),
                                this.ResourceGroupName,
                                this.ManagerName);

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
            var volumeContainersOnTargetDevice = this.Client.VolumeContainers.ListByDevice(
                                                targetDevice.Name.GetDoubleEncoded(),
                                                this.ResourceGroupName,
                                                this.ManagerName);

            Assert.NotNull(volumeContainersOnTargetDevice);

            var volumeContainersAlreadyOnTargetDevice = volumeContainersOnTargetDevice.Where(v =>
                                    volumeContainerNames.Contains(v.Name, new StringIgnoreCaseEqualityComparer()));

            Assert.NotNull(volumeContainersAlreadyOnTargetDevice);
            Assert.Empty(volumeContainersAlreadyOnTargetDevice);

            // Get failover sets
            var failoverSets = this.Client.Devices.ListFailoverSets(
                sourceDeviceName,
                this.ResourceGroupName,
                this.ManagerName);

            Assert.NotNull(failoverSets);
            Assert.NotEmpty(failoverSets);

            // Get failover targets
            ListFailoverTargetsRequest failoverTargetsRequest = new ListFailoverTargetsRequest()
            {
                VolumeContainers = volumeContainerIds
            };

            var failoverTargets = this.Client.Devices.ListFailoverTargets(
                sourceDeviceName,
                failoverTargetsRequest,
                this.ResourceGroupName,
                this.ManagerName);

            Assert.NotNull(failoverTargets);
            Assert.NotEmpty(failoverTargets);

            // Create a failover request
            FailoverRequest failoverRequest = new FailoverRequest()
            {
                TargetDeviceId = targetDevice.Id,
                VolumeContainers = volumeContainerIds
            };

            // Trigger failover
            this.Client.Devices.Failover(
                                sourceDevice.Name.GetDoubleEncoded(),
                                failoverRequest,
                                this.ResourceGroupName,
                                this.ManagerName);

            // Query volume containers from target device after failover
            var volumeContainersAfterFailover = this.Client.VolumeContainers.ListByDevice(
                                 targetDevice.Name.GetDoubleEncoded(),
                                 this.ResourceGroupName,
                                 this.ManagerName);

            Assert.NotNull(volumeContainersAfterFailover);

            // Assuming the volume container names are the same on the target device
            var failedOverVolumeContainers = volumeContainersAfterFailover.Where(v =>
                                    volumeContainerNames.Contains(v.Name, new StringIgnoreCaseEqualityComparer()));

            Assert.NotNull(failedOverVolumeContainers);
            Assert.NotEmpty(failedOverVolumeContainers);
        }
    }
}
