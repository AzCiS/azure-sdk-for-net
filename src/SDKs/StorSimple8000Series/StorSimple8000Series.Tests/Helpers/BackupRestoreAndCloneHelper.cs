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
        /// Helper method to create backup policy for a given set of volumes.
        /// </summary>
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

            //validate one of the schedules
            var schedule = testBase.Client.BackupSchedules.Get(
                deviceName.GetDoubleEncoded(),
                backupPolicy.Name.GetDoubleEncoded(),
                scheduleNames.First().GetDoubleEncoded(),
                testBase.ResourceGroupName,
                testBase.ManagerName);

            Assert.True(schedule != null && schedule.Name.Equals(scheduleNames.First()) &&
                schedule.ScheduleRecurrence.Equals(RecurrenceType.Daily), "Schedule creation was not successful.");

            return backupPolicy;
        }

        /// <summary>
        /// Helper method to trigger a manual backup.
        /// </summary>
        public static Backup BackupNow(StorSimple8000SeriesTestBase testBase, string deviceName, string policyName)
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
        /// Helper method to restore volumes by backup.
        /// </summary>
        public static void RestoreBackup(StorSimple8000SeriesTestBase testBase, string deviceName, string backupName)
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

        public static void CloneVolume(StorSimple8000SeriesTestBase testBase, string deviceName, string volumeName)
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
        /// Deletes the backup-policy and all backups, backup-schedules for the specified backupPolicy
        /// </summary>
        public static void DeleteBackupPolicieSchedulesAndBackups(StorSimple8000SeriesTestBase testBase, string deviceName, string backupPolicyName)
        {
            var doubleEncodedDeviceName = deviceName.GetDoubleEncoded();

            //get backupPolicy
            var bp = testBase.Client.BackupPolicies.Get(
                doubleEncodedDeviceName,
                backupPolicyName.GetDoubleEncoded(),
                testBase.ResourceGroupName,
                testBase.ManagerName);

            //create oDataQuery
            var startTime = DateTime.MinValue;
            var endTime = DateTime.Now;
            Expression<Func<BackupFilter, bool>> filter = f => f.CreatedTime >= startTime && f.CreatedTime <= endTime && f.BackupPolicyId == bp.Id;
            var oDataQuery = new ODataQuery<BackupFilter>(filter);

            //get backups for the backup-policy and delete
            var backups = testBase.Client.Backups.ListByDevice(
                doubleEncodedDeviceName,
                testBase.ResourceGroupName,
                testBase.ManagerName,
                oDataQuery);

            foreach (var backup in backups)
            {
                testBase.Client.Backups.Delete(
                    doubleEncodedDeviceName,
                    backup.Name.GetDoubleEncoded(),
                    testBase.ResourceGroupName,
                    testBase.ManagerName);
            }

            //get schedules for the backup-policy and delete
            var backupSchedules = testBase.Client.BackupSchedules.ListByBackupPolicy(
                doubleEncodedDeviceName,
                bp.Name.GetDoubleEncoded(),
                testBase.ResourceGroupName,
                testBase.ManagerName);

            foreach (var schedule in backupSchedules)
            {
                testBase.Client.BackupSchedules.Delete(
                    doubleEncodedDeviceName,
                    bp.Name.GetDoubleEncoded(),
                    schedule.Name.GetDoubleEncoded(),
                    testBase.ResourceGroupName,
                    testBase.ManagerName);
            }

            //delete backup-policy
            testBase.Client.BackupPolicies.Delete(
                doubleEncodedDeviceName,
                bp.Name.GetDoubleEncoded(),
                testBase.ResourceGroupName,
                testBase.ManagerName);

            //validate deletion
            var backupPolicies = testBase.Client.BackupPolicies.ListByDevice(
                doubleEncodedDeviceName,
                testBase.ResourceGroupName,
                testBase.ManagerName);

            var backupPolicy = backupPolicies.FirstOrDefault(b => b.Name.Equals(backupPolicyName));

            Assert.True(backupPolicy == null, "Deletion of backup-policy was not successful.");
        }

        /// <summary>
        /// Create Schedule for a Backup 
        /// </summary>
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
        /// Helper method to return backups for a given volume
        /// </summary>
        private static IEnumerable<Backup> GetBackupsByVolume(StorSimple8000SeriesTestBase testBase, string deviceName, string volumeName)
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
    }
}