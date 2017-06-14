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
    public static class Helpers
    {
        #region Manager Helper
        public static Manager CreateManager(StorSimple8000SeriesTestBase testBase, string managerName)
        {
            Manager resourceToCreate = new Manager()
            {
                Location = "westus",
                CisIntrinsicSettings = new ManagerIntrinsicSettings()
                {
                    Type = ManagerType.GardaV1
                }
            };

            Manager manager = testBase.Client.Managers.CreateOrUpdate(
                                    resourceToCreate,
                                    testBase.ResourceGroupName,
                                    managerName);

            return manager;
        }
        #endregion

        #region Device registration Key - Secrets flow
        /// <summary>
        /// Get the device registration key.
        /// </summary>
        public static string GetDeviceRegistrationKey(StorSimple8000SeriesTestBase testBase)
        {
            return testBase.Client.Managers.GetDeviceRegistrationKey(testBase.ResourceGroupName, testBase.ManagerName);
        }
        #endregion

        #region Device
        /// <summary>
        /// Configure device and get the device.
        /// </summary>
        public static Device ConfigureAndGetDevice(StorSimple8000SeriesTestBase testBase, string deviceNameWithoutDoubleEncoding, string controllerZeroIp, string controllerOneIp)
        {
            Device device = testBase.Client.Devices.Get(
                deviceNameWithoutDoubleEncoding.GetDoubleEncoded(),
                testBase.ResourceGroupName,
                testBase.ManagerName);

            if (device.Status == DeviceStatus.ReadyToSetup)
            {
                var secondaryDnsServers = TestConstants.SecondaryDnsServers.Split(';');
                var configureDeviceRequest = new ConfigureDeviceRequest()
                {
                    FriendlyName = deviceNameWithoutDoubleEncoding.GetDoubleEncoded(),
                    CurrentDeviceName = deviceNameWithoutDoubleEncoding.GetDoubleEncoded(),
                    TimeZone = "India Standard Time",
                    NetworkInterfaceData0Settings = new NetworkInterfaceData0Settings()
                    {
                        ControllerZeroIp = controllerZeroIp,
                        ControllerOneIp = controllerOneIp
                    },
                    DnsSettings = new SecondaryDNSSettings()
                    {
                        SecondaryDnsServers = new List<string>(secondaryDnsServers)
                    }

                };

                testBase.Client.Devices.Configure(
                    configureDeviceRequest,
                    testBase.ResourceGroupName,
                    testBase.ManagerName);

                //need to add details related to odata filter
                device = testBase.Client.Devices.Get(
                    deviceNameWithoutDoubleEncoding.GetDoubleEncoded(),
                    testBase.ResourceGroupName,
                    testBase.ManagerName);
            }

            return device;
        }
        #endregion

        #region Storage account credential
        /// <summary>
        /// Create storage account credential.
        /// </summary>
        public static StorageAccountCredential CreateStorageAccountCredential(StorSimple8000SeriesTestBase testBase, string sacNameWithoutDoubleEncoding, string sacAccessKeyInPlainText)
        {
            StorageAccountCredential sacToCreate = new StorageAccountCredential()
            {
                EndPoint = TestConstants.DefaultStorageAccountEndPoint,
                SslStatus = SslStatus.Enabled,
                AccessKey = testBase.Client.Managers.GetAsymmetricEncryptedSecret(
                    testBase.ResourceGroupName,
                    testBase.ManagerName,
                    sacAccessKeyInPlainText)
            };

            testBase.Client.StorageAccountCredentials.CreateOrUpdate(
                sacNameWithoutDoubleEncoding.GetDoubleEncoded(),
                sacToCreate,
                testBase.ResourceGroupName,
                testBase.ManagerName);

            var sac = testBase.Client.StorageAccountCredentials.Get(
                sacNameWithoutDoubleEncoding.GetDoubleEncoded(),
                testBase.ResourceGroupName,
                testBase.ManagerName);

            Assert.True(sac != null && sac.Name.Equals(sacNameWithoutDoubleEncoding) &&
                sac.SslStatus.Equals(SslStatus.Enabled) &&
                sac.EndPoint.Equals(TestConstants.DefaultStorageAccountEndPoint),
                "Creation of SAC was not successful.");

            return sac;
        }
        #endregion

        #region Bandwidth Setting
        public static BandwidthSetting CreateBandwidthSetting(StorSimple8000SeriesTestBase testBase, string bwsName)
        {
            //bandwidth schedule
            var rateInMbps = 10;
            var days = new List<SSModels.DayOfWeek?>() { SSModels.DayOfWeek.Saturday, SSModels.DayOfWeek.Sunday };
            var bandwidthSchedule1 = new BandwidthSchedule()
            {
                Start = new Time(10, 0, 0),
                Stop = new Time(20, 0, 0),
                RateInMbps = rateInMbps,
                Days = days
            };

            //bandwidth Setting
            var bwsToCreate = new BandwidthSetting()
            {
                Schedules = new List<BandwidthSchedule>() { bandwidthSchedule1 }
            };

            testBase.Client.BandwidthSettings.CreateOrUpdate(
                bwsName.GetDoubleEncoded(),
                bwsToCreate,
                testBase.ResourceGroupName,
                testBase.ManagerName);

            var bws = testBase.Client.BandwidthSettings.Get(
                bwsName.GetDoubleEncoded(),
                testBase.ResourceGroupName,
                testBase.ManagerName);

            //validation
            Assert.True(bws != null && bws.Name.Equals(bwsName) &&
                bws.Schedules != null && bws.Schedules.Count != 0, "Creation of Bandwidth Setting was not successful.");

            return bws;
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

        internal static object CheckAndGetVolumeContainers()
        {
            throw new NotImplementedException();
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

        #region Device Settings Calls

        public static AlertSettings CheckAndGetDeviceAlertSettings(StorSimple8000SeriesTestBase testBase, string deviceName)
        {
            var alertSettings = testBase.Client.DeviceSettings.GetAlertSettings(
                                    deviceName,
                                    testBase.ResourceGroupName,
                                    testBase.ManagerName);

            Assert.True(alertSettings != null, string.Format("Alert Settings is not present on device"));
            return alertSettings;
        }

        public static TimeSettings CheckAndGetDeviceTimeSettings(StorSimple8000SeriesTestBase testBase, string deviceName)
        {
            var timeSettings = testBase.Client.DeviceSettings.GetTimeSettings(
                                    deviceName,
                                    testBase.ResourceGroupName,
                                    testBase.ManagerName);

            Assert.True(timeSettings != null, string.Format("Time Settings is not present on device"));
            return timeSettings;
        }

        public static NetworkSettings CheckAndGetDeviceNetworkSettings(StorSimple8000SeriesTestBase testBase, string deviceName)
        {
            var networkSettings = testBase.Client.DeviceSettings.GetNetworkSettings(
                                    deviceName,
                                    testBase.ResourceGroupName,
                                    testBase.ManagerName);

            Assert.True(networkSettings != null, string.Format("Network Settings is not present on device"));
            return networkSettings;
        }

        public static SecuritySettings CheckAndGetDeviceSecuritySettings(StorSimple8000SeriesTestBase testBase, string deviceName)
        {
            var securitySettings = testBase.Client.DeviceSettings.GetSecuritySettings(
                                    deviceName,
                                    testBase.ResourceGroupName,
                                    testBase.ManagerName);

            Assert.True(securitySettings != null, string.Format("Security Settings is not present on device"));
            return securitySettings;
        }

        /// <summary>
        /// Create TimeSettings on the Device.
        /// </summary>
        /// <param name="testBase"></param>
        /// <param name="deviceName"></param>
        public static TimeSettings CreateTimeSettings(
            StorSimple8000SeriesTestBase testBase,
            string deviceName)
        {
            TimeSettings timeSettingsToCreate = new TimeSettings("Pacific Standard Time");
            timeSettingsToCreate.PrimaryTimeServer = "time.windows.com";
            timeSettingsToCreate.SecondaryTimeServer = new List<string>() { "8.8.8.8" };

            testBase.Client.DeviceSettings.CreateOrUpdateTimeSettings(
                    deviceName.GetDoubleEncoded(),
                    timeSettingsToCreate,
                    testBase.ResourceGroupName,
                    testBase.ManagerName);

            var timeSettings = testBase.Client.DeviceSettings.GetTimeSettings(
                deviceName.GetDoubleEncoded(),
                testBase.ResourceGroupName,
                testBase.ManagerName);

            //validation
            Assert.True(timeSettings != null && timeSettings.PrimaryTimeServer.Equals("time.windows.com") &&
                timeSettings.SecondaryTimeServer.Equals("8.8.8.8") , "Creation of Time Setting was not successful.");

            return timeSettings;
        }

        /// <summary>
        /// Create AlertSettings on the Device.
        /// </summary>
        /// <param name="testBase"></param>
        /// <param name="deviceName"></param>
        public static AlertSettings CreateAlertSettings(
            StorSimple8000SeriesTestBase testBase,
            string deviceName)
        {
            AlertSettings alertsettingsToCreate = new AlertSettings(AlertEmailNotificationStatus.Enabled);
            alertsettingsToCreate.AlertNotificationCulture = "en-US";
            alertsettingsToCreate.NotificationToServiceOwners = AlertEmailNotificationStatus.Enabled;
            
            alertsettingsToCreate.AdditionalRecipientEmailList = new List<string>();

            testBase.Client.DeviceSettings.CreateOrUpdateAlertSettings(
                    deviceName.GetDoubleEncoded(),
                    alertsettingsToCreate,
                    testBase.ResourceGroupName,
                    testBase.ManagerName);

            var alertSettings = testBase.Client.DeviceSettings.GetAlertSettings(
                deviceName.GetDoubleEncoded(),
                testBase.ResourceGroupName,
                testBase.ManagerName);

            //validation
            Assert.True(alertSettings != null && alertSettings.AlertNotificationCulture.Equals("en-US") &&
                alertSettings.EmailNotification.Equals(AlertEmailNotificationStatus.Enabled) && 
                alertSettings.NotificationToServiceOwners.Equals(AlertEmailNotificationStatus.Enabled), "Creation of Alert Setting was not successful.");

            return alertSettings;
        }

        /// <summary>
        /// Create NetworkSettings on the Device.
        /// </summary>
        /// <param name="testBase"></param>
        /// <param name="deviceName"></param>
        public static NetworkSettings CreateNetworkSettings(
            StorSimple8000SeriesTestBase testBase,
            string deviceName)
        {
            //TODO: add test data get network settings and set primary DNS from response.
            DNSSettings dnsSettings = new DNSSettings();
            NetworkAdapterList networkAdapterList = new NetworkAdapterList();

            NetworkSettingsPatch networkSettings = new NetworkSettingsPatch();
            return testBase.Client.DeviceSettings.UpdateNetworkSettings(
                    deviceName.GetDoubleEncoded(),
                    networkSettings,
                    testBase.ResourceGroupName,
                    testBase.ManagerName);
        }

        /// <summary>
        /// Create SecuritySettings on the Device.
        /// </summary>
        /// <param name="testBase"></param>
        /// <param name="deviceName"></param>
        public static SecuritySettings CreateSecuritySettings(
            StorSimple8000SeriesTestBase testBase,
            string deviceName)
        {
            RemoteManagementSettingsPatch remoteManagementSettings = new RemoteManagementSettingsPatch(RemoteManagementModeConfiguration.HttpsAndHttpEnabled);
            AsymmetricEncryptedSecret deviceAdminpassword = testBase.Client.Managers.GetAsymmetricEncryptedSecret(testBase.ResourceGroupName, testBase.ManagerName, "test-secret");
            AsymmetricEncryptedSecret snapshotmanagerPassword = testBase.Client.Managers.GetAsymmetricEncryptedSecret(testBase.ResourceGroupName, testBase.ManagerName, "test-secret1");
            

            ChapSettings chapSettings = new ChapSettings("test-initiator-user",
                testBase.Client.Managers.GetAsymmetricEncryptedSecret(testBase.ResourceGroupName, testBase.ManagerName, "test-chapsecret1"),
            "test-target-user",
                testBase.Client.Managers.GetAsymmetricEncryptedSecret(testBase.ResourceGroupName, testBase.ManagerName, "test-chapsecret2"));

            SecuritySettingsPatch securitySettingsToCreate = new SecuritySettingsPatch(remoteManagementSettings, deviceAdminpassword, snapshotmanagerPassword, chapSettings);

            testBase.Client.DeviceSettings.UpdateSecuritySettings(
                    deviceName.GetDoubleEncoded(),
                    securitySettingsToCreate,
                    testBase.ResourceGroupName,
                    testBase.ManagerName);

            var securitySettings = testBase.Client.DeviceSettings.GetSecuritySettings(
                deviceName.GetDoubleEncoded(),
                testBase.ResourceGroupName,
                testBase.ManagerName);

            //validation
            Assert.True(securitySettings != null && securitySettings.RemoteManagementSettings.Equals(RemoteManagementModeConfiguration.HttpsAndHttpEnabled) &&
                securitySettings.ChapSettings.InitiatorUser.Equals("test-initiator-user") && 
                securitySettings.ChapSettings.TargetUser.Equals("test-target-user"), "Creation of Security Setting was not successful.");

            return securitySettings;
        }


        #endregion

        #region GetMetrics Calls

        public static IEnumerable<Metrics> GetManagerMetrics(StorSimple8000SeriesTestBase testBase, MetricDefinition metricDefinition)
        {
            var managerMetrics = testBase.Client.Managers.ListMetrics(
                GenerateOdataFiler(metricDefinition),
                testBase.ResourceGroupName, 
                testBase.ManagerName);

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

            return vcMetrics;
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

        #region Backup, Restore and Clone
        /// <summary>
        /// Create Schedule for a Backup 
        /// </summary>
        /// <param name="testBase"></param>
        /// <param name="deviceName"></param>
        /// <param name="backupPolicyName"></param>
        /// <param name="name"></param>
        /// <param name="recurrenceType"></param>
        /// <returns></returns>
        private static BackupSchedule CreateBackupSchedule(
            StorSimple8000SeriesTestBase testBase,
            string deviceName,
            string backupPolicyName,
            string name,
            RecurrenceType recurrenceType)
        {
            // Initialize defaults
            DateTime startTime = DateTime.UtcNow;
            ScheduleStatus scheduleStatus = ScheduleStatus.Enabled;
            int recurrenceValue = 1;
            long retentionCount = 1;
            List<SSModels.DayOfWeek> weeklyDays = new List<SSModels.DayOfWeek>()
                                     {
                                         SSModels.DayOfWeek.Friday,
                                         SSModels.DayOfWeek.Thursday,
                                         SSModels.DayOfWeek.Monday
                                     };

            var schedule = new BackupSchedule()
            {
                BackupType = BackupType.CloudSnapshot,
                Kind = Kind.Series8000,
                RetentionCount = retentionCount,
                ScheduleStatus = scheduleStatus,
                StartTime = startTime,
                ScheduleRecurrence = new ScheduleRecurrence(
                    recurrenceType,
                    recurrenceValue)
            };

            // Set the week days for the weekly schedule
            if (schedule.ScheduleRecurrence.RecurrenceType == RecurrenceType.Weekly)
            {
                schedule.ScheduleRecurrence.WeeklyDaysList =
                    weeklyDays.Select(d => (SSModels.DayOfWeek?)d).ToList();
            }

            return testBase.Client.BackupSchedules.CreateOrUpdate(
                    deviceName.GetDoubleEncoded(),
                    backupPolicyName.GetDoubleEncoded(),
                    name.GetDoubleEncoded(),
                    schedule,
                    testBase.ResourceGroupName,
                    testBase.ManagerName);
        }

        /// <summary>
        /// Helper method to create backup policy for a given volumes
        /// </summary>
        /// <param name="testBase"></param>
        /// <param name="deviceName"></param>
        /// <param name="name"></param>
        /// <param name="volumeIds"></param>
        /// <returns></returns>
        public static BackupPolicy CreateBackupPolicy(
            StorSimple8000SeriesTestBase testBase,
            string deviceName,
            string name,
            IList<string> volumeIds)
        {
            var bp = new BackupPolicy()
            {
                Kind = Kind.Series8000,
                VolumeIds = volumeIds
            };

            var backupPolicy = testBase.Client.BackupPolicies.CreateOrUpdate(
                                    deviceName.GetDoubleEncoded(),
                                    name.GetDoubleEncoded(),
                                    bp,
                                    testBase.ResourceGroupName,
                                    testBase.ManagerName);

            Assert.NotNull(backupPolicy);
            Assert.Equal(backupPolicy.SchedulesCount, 0);

            List<string> scheduleNames = new List<string>()
            {
                "schedule1",
                "schedule2",
            };

            Dictionary<string, BackupSchedule> schNameToObject =
                new Dictionary<string, BackupSchedule>();

            foreach (string schName in scheduleNames)
            {
                // Create a backup schedule
                BackupSchedule bs = CreateBackupSchedule(
                                        testBase,
                                        deviceName,
                                        backupPolicy.Name,
                                        schName,
                                        RecurrenceType.Daily);
                schNameToObject.Add(schName, bs);
            }

            var schedules = testBase.Client.BackupSchedules.ListByBackupPolicy(
                deviceName.GetDoubleEncoded(),
                backupPolicy.Name.GetDoubleEncoded(),
                testBase.ResourceGroupName,
                testBase.ManagerName);

            Assert.Equal(schedules.Count(), schNameToObject.Count());
            return backupPolicy;
        }

        /// <summary>
        ///  Trigger a manual backup
        /// </summary>
        /// <param name="testBase"></param>
        /// <param name="deviceName"></param>
        /// <param name="policyName"></param>
        /// <returns></returns>
        public static Backup BackupNow(
            StorSimple8000SeriesTestBase testBase,
            string deviceName,
            string policyName)
        {
            string backupType = BackupType.CloudSnapshot.ToString();
            var jobStartTime = DateTime.UtcNow;

            var backupPolicy = testBase.Client.BackupPolicies.Get(
                deviceName,
                policyName,
                testBase.ResourceGroupName,
                testBase.ManagerName);

            // Take the backup
            testBase.Client.BackupPolicies.BackupNow(
                deviceName.GetDoubleEncoded(),
                policyName.GetDoubleEncoded(),
                backupType,
                testBase.ResourceGroupName,
                testBase.ManagerName);

            // Get the backup job
            var allBackupJobs = testBase.Client.Jobs.ListByDevice(
                deviceName,
                testBase.ResourceGroupName,
                testBase.ManagerName);

            var backupJob =
                allBackupJobs.FirstOrDefault(
                    j =>
                    j.StartTime > jobStartTime
                    && j.EntityLabel.Equals(policyName, StringComparison.CurrentCultureIgnoreCase));

            Assert.NotNull(backupJob);

            // Get the backup
            Expression<Func<BackupFilter, bool>> filter = backupFilter =>
                backupFilter.CreatedTime >= jobStartTime &&
                backupFilter.BackupPolicyId == backupPolicy.Id;

            var backups = testBase.Client.Backups.ListByDevice(
                                deviceName,
                                testBase.ResourceGroupName,
                                testBase.ManagerName,
                                new ODataQuery<BackupFilter>(filter));

            Assert.Equal(1, backups.Count());

            return backups.First() as Backup;
        }

        /// <summary>
        /// Helper method to restore volumes by backup
        /// </summary>
        /// <param name="testBase"></param>
        /// <param name="deviceName"></param>
        /// <param name="backupName"></param>
        public static void RestoreBackup(
            StorSimple8000SeriesTestBase testBase,
            string deviceName,
            string backupName)
        {
            DateTime jobStartTime = DateTime.UtcNow;

            // Get the backups by volume name
            var backups = testBase.Client.Backups.ListByDevice(
                                deviceName.GetDoubleEncoded(),
                                testBase.ResourceGroupName,
                                testBase.ManagerName);

            Assert.NotNull(backups);
            Assert.NotEmpty(backups);

            var backup = backups.FirstOrDefault();

            Assert.NotNull(backup);

            // Get volumes in the device in one call
            var volumeContainersByDevice = testBase.Client.VolumeContainers.ListByDevice(
                deviceName.GetDoubleEncoded(),
                testBase.ResourceGroupName,
                testBase.ManagerName);

            var volumesByDevice = testBase.Client.Volumes.ListByDevice(
                deviceName.GetDoubleEncoded(),
                testBase.ResourceGroupName,
                testBase.ManagerName);

            // First delete the volumes associated with backup
            foreach (BackupElement be in backup.Elements)
            {
                var volumeContainer = volumeContainersByDevice.FirstOrDefault(vc =>
                                    vc.Id.Equals(be.VolumeContainerId));

                Assert.NotNull(volumeContainer);

                var volumeToDelete = volumesByDevice.FirstOrDefault(v =>
                                v.Name.Equals(be.VolumeName));

                Assert.NotNull(volumeToDelete);

                // Diable the volume before deleting
                volumeToDelete.VolumeStatus = VolumeStatus.Offline;
                testBase.Client.Volumes.CreateOrUpdate(
                    deviceName.GetDoubleEncoded(),
                    volumeContainer.Name.GetDoubleEncoded(),
                    volumeToDelete.Name,
                    volumeToDelete,
                    testBase.ResourceGroupName,
                    testBase.ManagerName);

                testBase.Client.Volumes.Delete(
                                        deviceName.GetDoubleEncoded(),
                                        volumeContainer.Name.GetDoubleEncoded(),
                                        be.VolumeName.GetDoubleEncoded(),
                                        testBase.ResourceGroupName,
                                        testBase.ManagerName);
            }

            testBase.Client.Backups.Restore(
                                deviceName.GetDoubleEncoded(),
                                backup.Name.GetDoubleEncoded(),
                                testBase.ResourceGroupName,
                                testBase.ManagerName);

            var volumes = testBase.Client.Volumes.ListByDevice(
                                deviceName.GetDoubleEncoded(),
                                testBase.ResourceGroupName,
                                testBase.ManagerName);

            Assert.NotNull(volumes);
            Assert.NotEmpty(volumes);

            var volumesAfterRestore = testBase.Client.Volumes.ListByDevice(
                                                deviceName.GetDoubleEncoded(),
                                                testBase.ResourceGroupName,
                                                testBase.ManagerName);

            Assert.NotNull(volumesAfterRestore);
            Assert.NotEmpty(volumesAfterRestore);

            // Validate that the each element has corresponding volume
            foreach (BackupElement be in backup.Elements)
            {
                var vol = volumesAfterRestore.FirstOrDefault(v =>
                    v.Name.Equals(
                        be.VolumeName,
                        StringComparison.CurrentCultureIgnoreCase));

                Assert.NotNull(vol);
            }
        }

        /// <summary>
        /// Helper method to return backups for a given volume
        /// </summary>
        /// <param name="testBase"></param>
        /// <param name="deviceName"></param>
        /// <param name="volumeName"></param>
        /// <returns></returns>
        private static IEnumerable<Backup> GetBackupsByVolume(
            StorSimple8000SeriesTestBase testBase,
            string deviceName,
            string volumeName)
        {
            // Get the device
            var device = testBase.Client.Devices.Get(
                deviceName.GetDoubleEncoded(),
                testBase.ResourceGroupName,
                testBase.ManagerName);

            Assert.NotNull(device);

            // Get the volume
            var volumes = testBase.Client.Volumes.ListByDevice(
                                deviceName.GetDoubleEncoded(),
                                testBase.ResourceGroupName,
                                testBase.ManagerName);

            var volume = volumes.FirstOrDefault(
                v => v.Name.Equals(volumeName, StringComparison.CurrentCultureIgnoreCase));

            Assert.NotNull(volume);

            string vid = volume.Id;

            // Get the backups by the VolumeId
            Expression<Func<BackupFilter, bool>> filter = backupFilter =>
                backupFilter.VolumeId == vid;

            return testBase.Client.Backups.ListByDevice(
                                deviceName,
                                testBase.ResourceGroupName,
                                testBase.ManagerName,
                                new ODataQuery<BackupFilter>(filter)) as IEnumerable<Backup>;
        }

        public static void CloneVolume(
            StorSimple8000SeriesTestBase testBase,
            string deviceName,
            string volumeName)
        {
            string cloneVolumeName = TestUtilities.GenerateRandomName("CloneVolForSDKTest");

            // Get the device
            var device = testBase.Client.Devices.Get(
                deviceName.GetDoubleEncoded(),
                testBase.ResourceGroupName,
                testBase.ManagerName);

            // Get the backups for the volume
            var backups = GetBackupsByVolume(testBase, deviceName, volumeName);

            Assert.NotNull(backups);

            // Use backup and choose first element
            var backup = backups.First();

            Assert.NotNull(backup);
            Assert.NotNull(backup.Elements);

            var backupElement = backup.Elements.FirstOrDefault();
            Assert.NotNull(backupElement);

            var volumes = testBase.Client.Volumes.ListByDevice(
                    deviceName.GetDoubleEncoded(),
                    testBase.ResourceGroupName,
                    testBase.ManagerName);

            var volume = volumes.FirstOrDefault(
                v => v.Name.Equals(volumeName, StringComparison.CurrentCultureIgnoreCase));

            // Prepare clone request and trigger clone
            var cloneRequest = new CloneRequest
            {
                BackupElement = backupElement,
                TargetDeviceId = device.Id,
                TargetVolumeName = cloneVolumeName,
                TargetAccessControlRecordIds = volume.AccessControlRecordIds
            };

            testBase.Client.Backups.Clone(
                deviceName.GetDoubleEncoded(),
                backup.Name,
                backupElement.ElementName,
                cloneRequest,
                testBase.ResourceGroupName,
                testBase.ManagerName);

            // Verify that the clone volume is created
            var refreshedVolumes = testBase.Client.Volumes.ListByDevice(
                                                deviceName,
                                                testBase.ResourceGroupName,
                                                testBase.ManagerName);

            var clonedVolume = refreshedVolumes.FirstOrDefault(
                                v => v.Name.Equals(
                                    cloneVolumeName,
                                    StringComparison.CurrentCultureIgnoreCase));

            Assert.NotNull(clonedVolume);
        }

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
        #endregion

        #region Access Control Record
        /// <summary>
        /// Creates Access control record.
        /// </summary>
        public static AccessControlRecord CreateAccessControlRecord(StorSimple8000SeriesTestBase testBase, string acrNameWithoutDoubleEncoding, string initiatorName)
        {
            var acrToCreate = new AccessControlRecord()
            {
                InitiatorName = initiatorName
            };

            testBase.Client.AccessControlRecords.CreateOrUpdate(
                acrNameWithoutDoubleEncoding.GetDoubleEncoded(),
                acrToCreate,
                testBase.ResourceGroupName,
                testBase.ManagerName);

            var acr = testBase.Client.AccessControlRecords.Get(
                acrNameWithoutDoubleEncoding.GetDoubleEncoded(),
                testBase.ResourceGroupName,
                testBase.ManagerName);

            Assert.True(acr != null && acr.Name.Equals(acrNameWithoutDoubleEncoding) &&
                acr.InitiatorName.Equals(initiatorName),
                "Creation of ACR was not successful.");

            return acr;
        }
        #endregion
        #endregion

        public static void DeleteSacAcrBwsVolumeAndVolumeContainersInDevice(StorSimple8000SeriesTestBase testBase, string deviceName)
        {
            var sacs = testBase.Client.StorageAccountCredentials.ListByManager(testBase.ResourceGroupName, testBase.ManagerName);
            var acrs = testBase.Client.AccessControlRecords.ListByManager(testBase.ResourceGroupName, testBase.ManagerName);
        }

        #region Volume Container
        /// <summary>
        /// Create Volume Container, with Bandwidth Setting template.
        /// </summary>
        public static VolumeContainer CreateVolumeContainerWithBWS(StorSimple8000SeriesTestBase testBase, string deviceName, string volumeContainerName, string sacName, string bwsName)
        {
            var sac = testBase.Client.StorageAccountCredentials.Get(sacName.GetDoubleEncoded(), testBase.ResourceGroupName, testBase.ManagerName);
            var bws = testBase.Client.BandwidthSettings.Get(bwsName.GetDoubleEncoded(), testBase.ResourceGroupName, testBase.ManagerName);
            Assert.True(sac != null && sac.Id != null, "Storage account credential name passed for use in volume container doesn't exists.");
            Assert.True(bws != null && bws.Id != null, "Bandwidth setting name passed for use in volume container doesn't exists.");

            var vcToCreate = new VolumeContainer()
            {
                StorageAccountCredentialId = sac.Id,
                BandwidthSettingId = bws.Id,
                EncryptionKey = testBase.Client.Managers.GetAsymmetricEncryptedSecret(testBase.ResourceGroupName, testBase.ManagerName, TestUtilities.GenerateRandomName("EncryptionKeyForVC"))
            };

            testBase.Client.VolumeContainers.CreateOrUpdate(
                deviceName.GetDoubleEncoded(),
                volumeContainerName.GetDoubleEncoded(),
                vcToCreate,
                testBase.ResourceGroupName,
                testBase.ManagerName);

            var vc = testBase.Client.VolumeContainers.Get(
                deviceName.GetDoubleEncoded(),
                volumeContainerName.GetDoubleEncoded(),
                testBase.ResourceGroupName,
                testBase.ManagerName);

            Assert.True(vc != null && vc.Name.Equals(volumeContainerName) &&
                vc.StorageAccountCredentialId.Equals(sac.Id) &&
                vc.BandwidthSettingId.Equals(bws.Id),
                "Creation of Volume Container was not successful");

            return vc;
        }
        #endregion

        #region Volume
        /// <summary>
        /// Creates Volume.
        /// </summary>
        public static Volume CreateVolume(StorSimple8000SeriesTestBase testBase, string deviceName, string volumeContainerName, string volumeName, VolumeType volumeType, string acrName)
        {
            var acr = testBase.Client.AccessControlRecords.Get(acrName.GetDoubleEncoded(), testBase.ResourceGroupName, testBase.ManagerName);
            Assert.True(acr != null && acr.Name.Equals(acrName), "Access control record name passed for use in volume doesn't exists.");

            var volumeToCreate = new Volume()
            {
                AccessControlRecordIds = new List<string>() { acr.Id },
                MonitoringStatus = MonitoringStatus.Enabled,
                SizeInBytes = (long)5*1024*1024*1024, //5 Gb
                VolumeType = volumeType,
                VolumeStatus = VolumeStatus.Online
            };
            
            testBase.Client.Volumes.CreateOrUpdate(deviceName.GetDoubleEncoded(),
                volumeContainerName.GetDoubleEncoded(),
                volumeName.GetDoubleEncoded(),
                volumeToCreate,
                testBase.ResourceGroupName,
                testBase.ManagerName);

            var volume = testBase.Client.Volumes.Get(
                deviceName.GetDoubleEncoded(),
                volumeContainerName.GetDoubleEncoded(),
                volumeName.GetDoubleEncoded(),
                testBase.ResourceGroupName,
                testBase.ManagerName);

            Assert.True(volume != null && volume.Name.Equals(volumeName) &&
                volume.MonitoringStatus.Equals(MonitoringStatus.Enabled) &&
                volume.VolumeType.Equals(volumeType) &&
                volume.VolumeStatus.Equals(VolumeStatus.Online),
                "Creation of Volume was not successful");

            return volume;
        }
        #endregion

        #region Prerequisite Checks for tests
        /// <summary>
        /// Checks if minimum number of configured devices required for the testcase exists. If yes, returns names of them.
        /// </summary>
        /// <param name="testBase">The test base.</param>
        /// <param name="minimumRequiredConfiguredDevices">The minimum number of devices required to be configured for the testcase.</param>
        public static List<string> CheckAndGetConfiguredDevices1(StorSimple8000SeriesTestBase testBase, int minimumRequiredConfiguredDevices)
        {
            var devices = testBase.Client.Devices.ListByManager(testBase.ResourceGroupName, testBase.ManagerName);

            var configuredDeviceCount = 0;
            var configuredDeviceNames = new List<string>();

            foreach (var device in devices)
            {
                if (device.Status == DeviceStatus.Online)
                {
                    configuredDeviceCount++;
                    configuredDeviceNames.Add(device.Name);
                    if (configuredDeviceCount == minimumRequiredConfiguredDevices)
                    {
                        break;
                    }
                }
            }

            Assert.True(configuredDeviceCount == minimumRequiredConfiguredDevices, string.Format("Could not found minimum configured devices: Required={0}, ActuallyFound={1}", minimumRequiredConfiguredDevices, configuredDeviceCount));

            return configuredDeviceNames;
        }

        public static IEnumerable<StorageAccountCredential> CheckAndGetStorageAccountCredentials(StorSimple8000SeriesTestBase testBase, int requiredCount)
        {
            var sacs = testBase.Client.StorageAccountCredentials.ListByManager(
                testBase.ResourceGroupName,
                testBase.ManagerName);

            Assert.True(sacs.Count() >= requiredCount, string.Format("Could not found minimum Storage Account Credentials: Required={0}, ActuallyFound={1}", requiredCount, sacs.Count()));

            return sacs;
        }

        public static IEnumerable<BandwidthSetting> CheckAndGetBandwidthSettings(StorSimple8000SeriesTestBase testBase, int requiredCount)
        {
            var bandwidthSettings = testBase.Client.BandwidthSettings.ListByManager(
                testBase.ResourceGroupName,
                testBase.ManagerName);

            Assert.True(bandwidthSettings.Count() >= requiredCount, string.Format("Could not found minimum Bandwidth settings: Required={0}, ActuallyFound={1}", requiredCount, bandwidthSettings.Count()));

            return bandwidthSettings;
        }
        
        public static IEnumerable<AccessControlRecord> CheckAndGetAccessControlRecords(StorSimple8000SeriesTestBase testBase, int requiredCount)
        {
            var accessControlRecords = testBase.Client.AccessControlRecords.ListByManager(
                testBase.ResourceGroupName,
                testBase.ManagerName);

            Assert.True(accessControlRecords.Count() >= requiredCount, string.Format("Could not found minimum access control records: Required={0}, ActuallyFound={1}", requiredCount, accessControlRecords.Count()));

            return accessControlRecords;
        }

        #endregion
    }
}