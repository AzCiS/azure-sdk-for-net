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
            string cloneVolumeName = Helpers.GenerateRandomName("CloneVolForSDKTest");

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
        #endregion
    }
}
