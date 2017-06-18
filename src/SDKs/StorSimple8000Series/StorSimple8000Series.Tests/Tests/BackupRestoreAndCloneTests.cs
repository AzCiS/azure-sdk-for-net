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
using SSModels = Microsoft.Azure.Management.StorSimple8000Series.Models;
using Microsoft.Azure.Test.HttpRecorder;
using Microsoft.Azure.Management.Compute;
using Microsoft.Azure.Management.Network;
using Microsoft.Rest.Azure.OData;

namespace StorSimple8000Series.Tests
{
    public class BackupRestoreAndCloneTests : StorSimpleTestBase
    {
        public BackupRestoreAndCloneTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper) { }

        [Fact]
        public void TestBackupRestoreAndClone()
        {
            //check and get pre-requisites - device, volumeContainer, volumes
            var devices = Helpers.CheckAndGetConfiguredDevices(this, requiredCount: 1);
            var deviceName = devices.First().Name;
            var volumeContainerNames = Helpers.CheckAndGetVolumeContainers(this, deviceName, requiredCount: 1);
            var volumeContainerName = volumeContainerNames.First().Name;
            var volumes = Helpers.CheckAndGetVolumes(this, deviceName, volumeContainerName, requiredCount: 2);
            var firstVolumeName = volumes.ElementAt(0).Name;
            var volumeIds = new List<String>();
            volumeIds.Add(volumes.ElementAt(0).Id);
            volumeIds.Add(volumes.ElementAt(1).Id);

            //initialize entity names
            var backupPolicyName = "BkUpPolicyForSDKTest";

            try
            {
                // Create a backup policy
                var backupPolicy = CreateBackupPolicy(deviceName, backupPolicyName, volumeIds);

                // Take manual backup
                var backup = BackupNow(deviceName, backupPolicy.Name);

                // Restore
                RestoreBackup(deviceName, backup.Name);

                // Clone
                CloneVolume(deviceName, firstVolumeName);

                //Delete backup-policies, and associated schedules and backups
                DeleteBackupPolicieSchedulesAndBackups(deviceName, backupPolicyName);
            }
            catch (Exception e)
            {
                Assert.Null(e);
            }
        }

        /// <summary>
        /// Helper method to create backup policy for a given set of volumes.
        /// </summary>
        private BackupPolicy CreateBackupPolicy(string deviceName, string name, IList<string> volumeIds)
        {
            var bp = new BackupPolicy()
            {
                Kind = Kind.Series8000,
                VolumeIds = volumeIds
            };

            var backupPolicy = this.Client.BackupPolicies.CreateOrUpdate(
                                    deviceName.GetDoubleEncoded(),
                                    name.GetDoubleEncoded(),
                                    bp,
                                    this.ResourceGroupName,
                                    this.ManagerName);

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
                                        deviceName,
                                        backupPolicy.Name,
                                        schName,
                                        RecurrenceType.Daily);
                schNameToObject.Add(schName, bs);
            }

            //validate one of the schedules
            var schedule = this.Client.BackupSchedules.Get(
                deviceName.GetDoubleEncoded(),
                backupPolicy.Name.GetDoubleEncoded(),
                scheduleNames.First().GetDoubleEncoded(),
                this.ResourceGroupName,
                this.ManagerName);

            Assert.True(schedule != null && schedule.Name.Equals(scheduleNames.First()) &&
                schedule.ScheduleRecurrence.Equals(RecurrenceType.Daily), "Schedule creation was not successful.");

            return backupPolicy;
        }

        /// <summary>
        /// Helper method to trigger a manual backup.
        /// </summary>
        private Backup BackupNow(string deviceName, string policyName)
        {
            string backupType = BackupType.CloudSnapshot.ToString();
            var jobStartTime = DateTime.UtcNow;

            var backupPolicy = this.Client.BackupPolicies.Get(
                deviceName,
                policyName,
                this.ResourceGroupName,
                this.ManagerName);

            // Take the backup
            this.Client.BackupPolicies.BackupNow(
                deviceName.GetDoubleEncoded(),
                policyName.GetDoubleEncoded(),
                backupType,
                this.ResourceGroupName,
                this.ManagerName);

            // Get the backup job
            var allBackupJobs = this.Client.Jobs.ListByDevice(
                deviceName,
                this.ResourceGroupName,
                this.ManagerName);

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

            var backups = this.Client.Backups.ListByDevice(
                                deviceName,
                                this.ResourceGroupName,
                                this.ManagerName,
                                new ODataQuery<BackupFilter>(filter));

            Assert.Equal(1, backups.Count());

            return backups.First() as Backup;
        }

        /// <summary>
        /// Helper method to restore volumes by backup.
        /// </summary>
        private void RestoreBackup(string deviceName, string backupName)
        {
            DateTime jobStartTime = DateTime.UtcNow;

            // Get the backups by volume name
            var backups = this.Client.Backups.ListByDevice(
                                deviceName.GetDoubleEncoded(),
                                this.ResourceGroupName,
                                this.ManagerName);

            Assert.NotNull(backups);
            Assert.NotEmpty(backups);

            var backup = backups.FirstOrDefault();

            Assert.NotNull(backup);

            // Get volumes in the device in one call
            var volumeContainersByDevice = this.Client.VolumeContainers.ListByDevice(
                deviceName.GetDoubleEncoded(),
                this.ResourceGroupName,
                this.ManagerName);

            var volumesByDevice = this.Client.Volumes.ListByDevice(
                deviceName.GetDoubleEncoded(),
                this.ResourceGroupName,
                this.ManagerName);

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
                this.Client.Volumes.CreateOrUpdate(
                    deviceName.GetDoubleEncoded(),
                    volumeContainer.Name.GetDoubleEncoded(),
                    volumeToDelete.Name,
                    volumeToDelete,
                    this.ResourceGroupName,
                    this.ManagerName);

                this.Client.Volumes.Delete(
                                        deviceName.GetDoubleEncoded(),
                                        volumeContainer.Name.GetDoubleEncoded(),
                                        be.VolumeName.GetDoubleEncoded(),
                                        this.ResourceGroupName,
                                        this.ManagerName);
            }

            this.Client.Backups.Restore(
                                deviceName.GetDoubleEncoded(),
                                backup.Name.GetDoubleEncoded(),
                                this.ResourceGroupName,
                                this.ManagerName);

            var volumes = this.Client.Volumes.ListByDevice(
                                deviceName.GetDoubleEncoded(),
                                this.ResourceGroupName,
                                this.ManagerName);

            Assert.NotNull(volumes);
            Assert.NotEmpty(volumes);

            var volumesAfterRestore = this.Client.Volumes.ListByDevice(
                                                deviceName.GetDoubleEncoded(),
                                                this.ResourceGroupName,
                                                this.ManagerName);

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

        private void CloneVolume(string deviceName, string volumeName)
        {
            string cloneVolumeName = "CloneVolForSDKTest";

            // Get the device
            var device = this.Client.Devices.Get(
                deviceName.GetDoubleEncoded(),
                this.ResourceGroupName,
                this.ManagerName);

            // Get the backups for the volume
            var backups = GetBackupsByVolume(deviceName, volumeName);

            Assert.NotNull(backups);

            // Use backup and choose first element
            var backup = backups.First();

            Assert.NotNull(backup);
            Assert.NotNull(backup.Elements);

            var backupElement = backup.Elements.FirstOrDefault();
            Assert.NotNull(backupElement);

            var volumes = this.Client.Volumes.ListByDevice(
                    deviceName.GetDoubleEncoded(),
                    this.ResourceGroupName,
                    this.ManagerName);

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

            this.Client.Backups.Clone(
                deviceName.GetDoubleEncoded(),
                backup.Name,
                backupElement.ElementName,
                cloneRequest,
                this.ResourceGroupName,
                this.ManagerName);

            // Verify that the clone volume is created
            var refreshedVolumes = this.Client.Volumes.ListByDevice(
                                                deviceName,
                                                this.ResourceGroupName,
                                                this.ManagerName);

            var clonedVolume = refreshedVolumes.FirstOrDefault(
                                v => v.Name.Equals(
                                    cloneVolumeName,
                                    StringComparison.CurrentCultureIgnoreCase));

            Assert.NotNull(clonedVolume);
        }

        /// <summary>
        /// Deletes the backup-policy and all backups, backup-schedules for the specified backupPolicy
        /// </summary>
        private void DeleteBackupPolicieSchedulesAndBackups(string deviceName, string backupPolicyName)
        {
            var doubleEncodedDeviceName = deviceName.GetDoubleEncoded();

            //get backupPolicy
            var bp = this.Client.BackupPolicies.Get(
                doubleEncodedDeviceName,
                backupPolicyName.GetDoubleEncoded(),
                this.ResourceGroupName,
                this.ManagerName);

            //create oDataQuery
            var startTime = DateTime.MinValue;
            var endTime = DateTime.Now;
            Expression<Func<BackupFilter, bool>> filter = f => f.CreatedTime >= startTime && f.CreatedTime <= endTime && f.BackupPolicyId == bp.Id;
            var oDataQuery = new ODataQuery<BackupFilter>(filter);

            //get backups for the backup-policy and delete
            var backups = this.Client.Backups.ListByDevice(
                doubleEncodedDeviceName,
                this.ResourceGroupName,
                this.ManagerName,
                oDataQuery);

            foreach (var backup in backups)
            {
                this.Client.Backups.Delete(
                    doubleEncodedDeviceName,
                    backup.Name.GetDoubleEncoded(),
                    this.ResourceGroupName,
                    this.ManagerName);
            }

            //get schedules for the backup-policy and delete
            var backupSchedules = this.Client.BackupSchedules.ListByBackupPolicy(
                doubleEncodedDeviceName,
                bp.Name.GetDoubleEncoded(),
                this.ResourceGroupName,
                this.ManagerName);

            foreach (var schedule in backupSchedules)
            {
                this.Client.BackupSchedules.Delete(
                    doubleEncodedDeviceName,
                    bp.Name.GetDoubleEncoded(),
                    schedule.Name.GetDoubleEncoded(),
                    this.ResourceGroupName,
                    this.ManagerName);
            }

            //delete backup-policy
            this.Client.BackupPolicies.Delete(
                doubleEncodedDeviceName,
                bp.Name.GetDoubleEncoded(),
                this.ResourceGroupName,
                this.ManagerName);

            //validate deletion
            var backupPolicies = this.Client.BackupPolicies.ListByDevice(
                doubleEncodedDeviceName,
                this.ResourceGroupName,
                this.ManagerName);

            var backupPolicy = backupPolicies.FirstOrDefault(b => b.Name.Equals(backupPolicyName));

            Assert.True(backupPolicy == null, "Deletion of backup-policy was not successful.");
        }

        /// <summary>
        /// Create Schedule for a Backup 
        /// </summary>
        private BackupSchedule CreateBackupSchedule(
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

            return this.Client.BackupSchedules.CreateOrUpdate(
                    deviceName.GetDoubleEncoded(),
                    backupPolicyName.GetDoubleEncoded(),
                    name.GetDoubleEncoded(),
                    schedule,
                    this.ResourceGroupName,
                    this.ManagerName);
        }
        /// <summary>
        /// Helper method to return backups for a given volume
        /// </summary>
        private IEnumerable<Backup> GetBackupsByVolume(string deviceName, string volumeName)
        {
            // Get the device
            var device = this.Client.Devices.Get(
                deviceName.GetDoubleEncoded(),
                this.ResourceGroupName,
                this.ManagerName);

            Assert.NotNull(device);

            // Get the volume
            var volumes = this.Client.Volumes.ListByDevice(
                                deviceName.GetDoubleEncoded(),
                                this.ResourceGroupName,
                                this.ManagerName);

            var volume = volumes.FirstOrDefault(
                v => v.Name.Equals(volumeName, StringComparison.CurrentCultureIgnoreCase));

            Assert.NotNull(volume);

            string vid = volume.Id;

            // Get the backups by the VolumeId
            Expression<Func<BackupFilter, bool>> filter = backupFilter =>
                backupFilter.VolumeId == vid;

            return this.Client.Backups.ListByDevice(
                                deviceName,
                                this.ResourceGroupName,
                                this.ManagerName,
                                new ODataQuery<BackupFilter>(filter)) as IEnumerable<Backup>;
        }
    }
}
