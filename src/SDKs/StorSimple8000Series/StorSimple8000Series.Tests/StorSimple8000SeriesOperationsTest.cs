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
        [Fact]
        public void TestStorSimpleManagerOperations()
        {
            using (MockContext context = MockContext.Start(this.GetType().FullName))
            {
                var testBase = new StorSimple8000SeriesTestBase(context, TestConstants.DefaultResourceGroupName, TestConstants.ManagerNameForManagerOperations);

                try
                {
                    //create StorSimple Manager
                    var manager = Helpers.CreateManager(testBase, testBase.ManagerName);

                    //Get Device Registration Key
                    var registrationKey = Helpers.GetDeviceRegistrationKey(testBase);

                    //regenerate activation key
                    var newActivationKey = Helpers.RegenerateActivationKey(testBase);

                    //update tag for Storsimple Manager
                    var updatedManager = Helpers.UpdateManager(
                        testBase,
                        testBase.ManagerName,
                        TestUtilities.GenerateRandomName("TagName"),
                        TestUtilities.GenerateRandomName("TagValue"));

                    //list all StorSimple managers in subscription
                    var managersInSubscriptions = Helpers.ListManagerBySubscription(testBase);

                    //list all StorSimple managers in resourceGroup
                    var managersInResourceGroup = Helpers.ListManagerByResourceGroup(testBase);

                    //get and update ExtendedInfo
                    var updatedExtendedInfo = Helpers.GetAndUpdateManagerExtendedInfo(testBase);
                }
                catch (Exception e)
                {
                    Assert.Null(e);
                }
                finally
                {
                    //delete ExtendedInfo
                    Helpers.DeleteManagerExtendedInfo(testBase, testBase.ManagerName);

                    //delete StorSimple Manager
                    Helpers.DeleteManagerAndValidate(testBase, testBase.ManagerName);
                }
            }
        }

        [Fact]
        public void TestServiceConfiguration()
        {
            using (MockContext context = MockContext.Start(this.GetType().FullName))
            {
                var testBase = new StorSimple8000SeriesTestBase(context, TestConstants.DefaultResourceGroupName, TestConstants.DefaultManagerName);

                //check prerequisite - a device exists.
                var devices = Helpers.CheckAndGetConfiguredDevices(testBase, requiredCount: 1);

                //initialize entity names
                var sacName = TestConstants.DefaultStorageAccountName;
                var acrName = TestUtilities.GenerateRandomName("ACRForSDKTest");
                var acrInitiatorName = TestUtilities.GenerateRandomName(TestConstants.DefaultInitiatorName);
                var bwsName = TestUtilities.GenerateRandomName("BWSForSDKTest");


                try
                {
                    //Create SAC
                    var sac = Helpers.CreateStorageAccountCredential(testBase, sacName, TestConstants.DefaultStorageAccountAccessKey);

                    //Create ACR
                    var acr = Helpers.CreateAccessControlRecord(testBase, acrName, acrInitiatorName);

                    //Create Bandwidth Setting
                    var bws = Helpers.CreateBandwidthSetting(testBase, bwsName);
                }
                catch (Exception e)
                {
                    Assert.Null(e);
                }
                finally
                {
                    Helpers.DeleteStorageAccountCredentialAndValidate(testBase, sacName);

                    Helpers.DeleteAccessControlRecordAndValidate(testBase, acrName);

                    Helpers.DeleteBandwidthSettingAndValidate(testBase, bwsName);
                }
            }
        }

        [Fact]
        public void TestVolumeContainerAndVolume()
        {
            using (MockContext context = MockContext.Start(this.GetType().FullName))
            {
                var testBase = new StorSimple8000SeriesTestBase(context);

                //check and get prerequisites - device, sac, acr, bws
                var devices = Helpers.CheckAndGetConfiguredDevices(testBase, requiredCount: 1);
                var sacs = Helpers.CheckAndGetStorageAccountCredentials(testBase, requiredCount: 1);
                var bandwidthSettings = Helpers.CheckAndGetBandwidthSettings(testBase, requiredCount: 1);
                var acrs = Helpers.CheckAndGetAccessControlRecords(testBase, requiredCount: 1);
                var deviceName = devices.First().Name;
                var sacName = sacs.First().Name;
                var bwsName = bandwidthSettings.First().Name;
                var acrName = acrs.First().Name;

                //initialize entity names
                var vcName = TestUtilities.GenerateRandomName("VCForSDKTest");
                var volName = TestUtilities.GenerateRandomName("VolForSDKTest");

                try
                {
                    //Create Volume Container
                    var vc = Helpers.CreateVolumeContainer(testBase, deviceName, vcName, sacName, bwsName);

                    //Create volumes
                    var vol = Helpers.CreateVolume(testBase, deviceName, vc.Name, volName, VolumeType.Tiered, acrName);
                }
                catch (Exception e)
                {
                    Assert.Null(e);
                }
                finally
                {
                    Helpers.DeleteVolumeContainersAndVolumes(testBase, deviceName, vcName);
                }
            }
        }

        [Fact]
        public void TestBackupRestoreAndClone()
        {
            using (MockContext context = MockContext.Start(this.GetType().FullName))
            {
                var testBase = new StorSimple8000SeriesTestBase(context, TestConstants.DefaultResourceGroupName, TestConstants.DefaultManagerName);

                //check and get pre-requisites - device, volumeContainer, volumes
                var devices = Helpers.CheckAndGetConfiguredDevices(testBase, requiredCount: 1);
                var deviceName = devices.First().Name;
                var volumeContainerNames = Helpers.CheckAndGetVolumeContainers(testBase, deviceName, requiredCount: 1);
                var volumeContainerName = volumeContainerNames.First().Name;
                var volumes = Helpers.CheckAndGetVolumes(testBase, deviceName, volumeContainerName, requiredCount: 2);
                var firstVolumeName = volumes.ElementAt(0).Name;
                var volumeIds = new List<String>();
                volumeIds.Add(volumes.ElementAt(0).Id);
                volumeIds.Add(volumes.ElementAt(1).Id);

                //initialize entity names
                var backupPolicyName = TestUtilities.GenerateRandomName("BkUpPolicyForSDKTest");

                try
                {
                    // Create a backup policy
                    var backupPolicy = Helpers.CreateBackupPolicy(testBase, deviceName, backupPolicyName, volumeIds);

                    // Take manual backup
                    var backup = Helpers.BackupNow(testBase, deviceName, backupPolicy.Name);

                    // Restore
                    Helpers.RestoreBackup(testBase, deviceName, backup.Name);

                    // Clone
                    Helpers.CloneVolume(testBase, deviceName, firstVolumeName);
                }
                catch (Exception e)
                {
                    Assert.Null(e);
                }
                finally
                {
                    Helpers.DeleteBackupPolicieSchedulesAndBackups(testBase, deviceName, backupPolicyName);
                }
            }
        }

        [Fact]
        public void TestFailover()
        {
            using (MockContext context = MockContext.Start(this.GetType().FullName))
            {
                var testBase = new StorSimple8000SeriesTestBase(context, TestConstants.DefaultResourceGroupName, TestConstants.DefaultManagerName);

                //check and get pre-requisites - 2 devices, volumeContainer, volumes
                var device1 = Helpers.CheckAndGetConfiguredDevices(testBase, TestConstants.FirstDeviceName);
                var device2 = Helpers.CheckAndGetConfiguredDevices(testBase, TestConstants.DeviceForFailover);
                var sourceDeviceName = device1.Name;
                var targetDeviceName = device2.Name;
                var volumeContainers = Helpers.CheckAndGetVolumeContainers(testBase, sourceDeviceName, requiredCount: 2);
                var volumeContainerNames = volumeContainers.Select(vc => vc.Name).ToList();

                try
                {
                    // Do failover
                    Helpers.Failover(testBase, sourceDeviceName, targetDeviceName, volumeContainerNames);
                }
                catch (Exception e)
                {
                    Assert.Null(e);
                }
            }
        }

        [Fact]
        public void TestMetricOperations()
        {
            using (MockContext context = MockContext.Start(this.GetType().FullName))
            {
                var testBase = new StorSimple8000SeriesTestBase(context, TestConstants.DefaultResourceGroupName, TestConstants.DefaultManagerName);

                //checking for prerequisites
                var device = Helpers.CheckAndGetConfiguredDevices(testBase, TestConstants.DeviceForMonitoringTest);
                var deviceName = device.Name;
                var volumeContainers = Helpers.CheckAndGetVolumeContainers(testBase, deviceName, requiredCount: 1);
                var volumeContainerName = volumeContainers.First().Name;
                var volumes = Helpers.CheckAndGetVolumes(testBase, deviceName, volumeContainerName, requiredCount: 1);
                var volumeName = volumes.First().Name;

                try
                {
                    //Get metric definitions and metrics for Manager
                    var resourceMetricDefinition = Helpers.GetManagerMetricDefinitions(testBase);
                    var resourceMetrics = Helpers.GetManagerMetrics(testBase, resourceMetricDefinition.First());

                    //Get metric definitions and metrics for Device
                    var deviceMetricDefinition = Helpers.GetDeviceMetricDefinition(testBase, deviceName);
                    var deviceMetrics = Helpers.GetDeviceMetrics(testBase, deviceName, deviceMetricDefinition.First());

                    //Get metric definitions and metrics for VolumeContainer
                    var volumeContainerMetricDefinition = Helpers.GetVolumeContainerMetricDefinition(testBase, deviceName, volumeContainerName);
                    var volumeContainerMetrics = Helpers.GetVolumeContainerMetrics(testBase, deviceName, volumeContainerName, volumeContainerMetricDefinition.First());

                    //Get metric definitions and metrics for Volume
                    var volumeMetricDefinition = Helpers.GetVolumeMetricDefinition(testBase, deviceName, volumeContainerName, volumeName);
                    var volumeMetrics = Helpers.GetVolumeMetrics(testBase, deviceName, volumeContainerName, volumeName, volumeMetricDefinition.First());
                }
                catch (Exception e)
                {
                    Assert.Null(e);
                }
            }
        }

        [Fact]
        public void TestOperationsAndFeaturesAPI()
        {
            using (MockContext context = MockContext.Start(this.GetType().FullName))
            {
                var testBase = new StorSimple8000SeriesTestBase(context);

                try
                {
                    //operations
                    var operations = Helpers.GetOperations(testBase);

                    //features for StorSimple Manager
                    var featuresForResource = Helpers.GetFeatures(testBase);

                    //features for device
                    var devices = Helpers.CheckAndGetConfiguredDevices(testBase, requiredCount: 1);
                    var deviceName = devices.First().Name;
                    var featuresForDevice = Helpers.GetFeatures(testBase, deviceName);
                }
                catch (Exception e)
                {
                    Assert.Null(e);
                }
            }
        }
 
        [Fact]
        public void TestDeviceSettingsOperationsOnConfiguredDevices()
        {
            using (MockContext context = MockContext.Start(this.GetType().FullName))
            {
                //checking for prerequisites
                var testBase = new StorSimple8000SeriesTestBase(context);
                var devices = Helpers.CheckAndGetConfiguredDevices(testBase, requiredCount: 1);
                var deviceName = devices.First().Name;

                try
                {
                    //Create Time Settings
                    var timeSettings = Helpers.CreateTimeSettings(testBase, deviceName);

                    //Create Alert Settings
                    var alertSettings = Helpers.CreateAlertSettings(testBase, deviceName);

                    //Create Network Settings
                    var bws = Helpers.CreateNetworkSettings(testBase, deviceName);

                    //Create Security Settings
                    var securitySettings = Helpers.CreateSecuritySettings(testBase, deviceName);
                }
                catch (Exception e)
                {
                    Assert.Null(e);
                }
            }
        }

        [Fact]
        public void TestServiceDataEncryptionKeyRolloverOnConfiguredDevices()
            {
            using (MockContext context = MockContext.Start(this.GetType().FullName))
            {
                //checking for prerequisites
                var testBase = new StorSimple8000SeriesTestBase(context);
                var device = Helpers.CheckAndGetConfiguredDevices(testBase, TestConstants.DeviceForKeyRollover);
                var firstDeviceName = device.Name;

                try
                {
                    //Authorize device for Key rollover.
                    Helpers.AuthorizeDeviceForRollover(
                        testBase,
                        firstDeviceName);
                }

                catch (Exception e)
                {
                    Assert.Null(e);
                }
            }
        }

        [Fact]
        public void TestAlerts()
        {
            using (MockContext context = MockContext.Start(this.GetType().FullName))
            {
                //checking for prerequisites
                var testBase = new StorSimple8000SeriesTestBase(context, TestConstants.DefaultResourceGroupName, TestConstants.DefaultManagerName);
                var deviceName = TestConstants.FirstDeviceName;

                try
                {
                    // Get Active Alerts with device name filter
                    var alerts = Helpers.GetActiveAlertsForDevice(testBase, deviceName);
                    var firstAlert = alerts.First();

                    // Clear Alert
                    Helpers.ClearAlert(testBase, firstAlert.Id);

                    // Get Cleared Alerts
                    Helpers.GetClearedAlerts(testBase);

                    //Test Severity Filters - Get Informational Alerts
                    Helpers.GetAlertsBySeverity(testBase, AlertSeverity.Informational);
                }
                catch (Exception e)
                {
                    Assert.Null(e);
                }
            }
        }
    }
}