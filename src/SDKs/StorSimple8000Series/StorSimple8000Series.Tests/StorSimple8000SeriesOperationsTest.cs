using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;
using Microsoft.Azure.Management.StorSimple8000Series;
using Microsoft.Azure.Management.StorSimple8000Series.Models;
using Microsoft.Rest.ClientRuntime.Azure.TestFramework;

namespace StorSimple8000Series.Tests
{
    public class StorSimple8000SeriesTest : TestBase
    {
        public void TestServiceConfiguration()
        {
            using (MockContext context = MockContext.Start(this.GetType().FullName))
            {
                var testBase = new StorSimple8000SeriesTestBase(context);

                var deviceNames = Helpers.CheckAndGetConfiguredDevices1(testBase, minimumRequiredConfiguredDevices: 1);
                var firstDeviceName = deviceNames[0];

                try
                {
                    //Create SAC
                    var sac = Helpers.CreateStorageAccountCredential(
                        testBase,
                        TestConstants.DefaultStorageAccountName,
                        TestConstants.DefaultStorageAccountAccessKey);

                    //Create ACR
                    var acr = Helpers.CreateAccessControlRecord(
                        testBase,
                        TestUtilities.GenerateRandomName("ACRForSDKTest"),
                        TestUtilities.GenerateRandomName(TestConstants.DefaultInitiatorName));

                    //Create Bandwidth Setting
                    var bws = Helpers.CreateBandwidthSetting(
                        testBase,
                        TestUtilities.GenerateRandomName("BWSForSDKTest"));                    
                }
                catch (Exception e)
                {
                    Assert.Null(e);
                }
            }
        }

        [Fact]
        public void CreateStorSimpleManagerAndConfigureDevice()
        {
            using (MockContext context = MockContext.Start(this.GetType().FullName))
            {
                var testBase = new StorSimple8000SeriesTestBase(context);

                //create StorSimple Manager
                //var manager = Helpers.CreateManager(testBase, testBase.ManagerName);

                //Get Device Registration Key
                var registrationKey = Helpers.GetDeviceRegistrationKey(testBase);

                //Configure and Get device
                var device = Helpers.ConfigureAndGetDevice(testBase, TestConstants.FirstDeviceName, TestConstants.FirstDeviceControllerZeroIp, TestConstants.FirstDeviceControllerOneIp);

                //TODO: Deactivate device

                //TODO: Delete device

                //TODO: Delete StorSimple Manager
            }
        }

        [Fact]
        public void TestMetricOperations()
        {
            using (MockContext context = MockContext.Start(this.GetType().FullName))
            {
                var testBase = new StorSimple8000SeriesTestBase(context);

                //checking for prerequisites
                var devices = Helpers.CheckAndGetConfiguredDevices(testBase, requiredCount: 1);
                var volumeContainers = Helpers.CheckAndGetVolumeContainers(testBase, devices.First().Name, requiredCount: 1);
                var volumes = Helpers.CheckAndGetVolumes(testBase, devices.First().Name, volumeContainers.First().Name, requiredCount: 1);

                var deviceName = devices.First().Name;
                var volumeContainerName = volumeContainers.First().Name;
                var volumeName = volumes.First().Name;


                //Get MetricDefinitions for Manager
                var resourceMetricDefinition = Helpers.GetManagerMetricDefinitions(testBase);

                //Get MetricDefinitions for Device
                var deviceMetricDefinition = Helpers.GetDeviceMetricDefinition(testBase, deviceName);

                //Get MetricDefinitions for VolumeContainer
                var volumeContainerMetricDefinition = Helpers.GetVolumeContainerMetricDefinition(testBase, deviceName, volumeContainerName);

                //Get MetricDefinitions for Volume
                var volumeMetricDefinition = Helpers.GetVolumeMetricDefinition(testBase, deviceName, volumeContainerName, volumeName);

                //Get Metrics for Manager
                var resourceMetrics = Helpers.GetManagerMetrics(testBase, resourceMetricDefinition.First());

                //Get Metrics for Device
                var deviceMetrics = Helpers.GetDeviceMetrics(testBase, deviceName, deviceMetricDefinition.First());

                //Get Metrics for VolumeContainer
                var volumeContainerMetrics = Helpers.GetVolumeContainerMetrics(testBase, deviceName, volumeContainerName, volumeContainerMetricDefinition.First());

                //Get Metrics for Volume
                var volumeMetrics = Helpers.GetVolumeMetrics(testBase, deviceName, volumeContainerName, volumeName, volumeMetricDefinition.First());
            }
        }

        [Fact]
        public void TestVolumeContainerAndVolume()
        {
            using (MockContext context = MockContext.Start(this.GetType().FullName))
            {
                var testBase = new StorSimple8000SeriesTestBase(context);

                //check and get prerequisites - device, sac, acr, bws
                var deviceNames = Helpers.CheckAndGetConfiguredDevices1(testBase, minimumRequiredConfiguredDevices: 1);
                var sacs = Helpers.CheckAndGetStorageAccountCredentials(testBase, requiredCount: 1);
                var bandwidthSettings = Helpers.CheckAndGetBandwidthSettings(testBase, requiredCount: 1);
                var acrs = Helpers.CheckAndGetAccessControlRecords(testBase, requiredCount: 1);
                var firstDeviceName = deviceNames[0];
                var sacName = sacs.First().Name;
                var bwsName = bandwidthSettings.First().Name;
                var acrName = acrs.First().Name;

                try
                {
                    //Create Volume Container
                    var vc = Helpers.CreateVolumeContainerWithBWS(
                        testBase,
                        firstDeviceName,
                        TestUtilities.GenerateRandomName("VCForSDKTest"),
                        sacName, bwsName);

                    //Create volumes
                    var vol = Helpers.CreateVolume(testBase,
                        firstDeviceName,
                        vc.Name,
                        TestUtilities.GenerateRandomName("VolForSDKTest"),
                        VolumeType.Tiered,
                        acrName);
                }
                catch (Exception e)
                {
                    Assert.Null(e);
                }
            }
        }

        [Fact]
        public void TestBackupRestoreAndClone()
        {
            using (MockContext context = MockContext.Start(this.GetType().FullName))
            {
                var testBase = new StorSimple8000SeriesTestBase(context);

                //check and get pre-requisites - device, volumeContainer, volumes
                var deviceNames = Helpers.CheckAndGetConfiguredDevices1(testBase, minimumRequiredConfiguredDevices: 1);
                var firstDeviceName = deviceNames.First();
                var volumeContainerNames = Helpers.CheckAndGetVolumeContainers(testBase, firstDeviceName, requiredCount: 1);
                var volumeContainerName = volumeContainerNames.First().Name;
                var volumes = Helpers.CheckAndGetVolumes(testBase, firstDeviceName, volumeContainerName, requiredCount: 2);
                var firstVolumeName = volumes.ElementAt(0).Name;
                var secondVolumeName = volumes.ElementAt(1).Name;

                try
                {
                    // Create a backup policy
                    var backupPolicy = Helpers.CreateBackupPolicy(
                        testBase,
                        firstDeviceName,
                        TestUtilities.GenerateRandomName("BkUpPolicyForSDKTest"),
                        new List<string>()
                        {
                            firstVolumeName,
                            secondVolumeName
                        });

                    // Take manual backup
                    var backup = Helpers.BackupNow(testBase, firstDeviceName, backupPolicy.Name);

                    // Restore
                    Helpers.RestoreBackup(testBase, firstDeviceName, backup.Name);

                    // Clone
                    Helpers.CloneVolume(testBase, firstDeviceName, firstVolumeName);
                }
                catch (Exception e)
                {
                    Assert.Null(e);
                }
            }
        }
    }
}