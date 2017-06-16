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
        /// Create Volume Container, with Bandwidth Setting template.
        /// </summary>
        public static VolumeContainer CreateVolumeContainer(StorSimple8000SeriesTestBase testBase, string deviceName, string volumeContainerName, string sacName, string bwsName)
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
                SizeInBytes = (long)5 * 1024 * 1024 * 1024, //5 Gb
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

        /// <summary>
        /// Deletes volumeContainers and all volumes in the specified volumeContainer.
        /// </summary>
        public static void DeleteVolumeContainersAndVolumes(StorSimple8000SeriesTestBase testBase, string deviceName, string volumeContainerName)
        {
            var doubleEncodedDeviceName = deviceName.GetDoubleEncoded();

            var volumeContainerToDelete = testBase.Client.VolumeContainers.Get(
                doubleEncodedDeviceName,
                volumeContainerName.GetDoubleEncoded(),
                testBase.ResourceGroupName,
                testBase.ManagerName);

            var volumesInDevice = testBase.Client.Volumes.ListByDevice(
                doubleEncodedDeviceName,
                testBase.ResourceGroupName,
                testBase.ManagerName);

            var volumesInVC = testBase.Client.Volumes.ListByVolumeContainer(
                doubleEncodedDeviceName,
                volumeContainerToDelete.Name.GetDoubleEncoded(),
                testBase.ResourceGroupName,
                testBase.ManagerName);
            
            Assert.True(volumesInDevice.Count() >= volumesInVC.Count(), "List operations of volumes was not successful.");

            foreach (var v in volumesInVC)
            {
                testBase.Client.Volumes.Delete(
                    doubleEncodedDeviceName,
                    volumeContainerToDelete.Name.GetDoubleEncoded(),
                    v.Name.GetDoubleEncoded(),
                    testBase.ResourceGroupName,
                    testBase.ManagerName);
            }

            testBase.Client.VolumeContainers.Delete(
                doubleEncodedDeviceName,
                volumeContainerToDelete.Name.GetDoubleEncoded(),
                testBase.ResourceGroupName,
                testBase.ManagerName);

            var volumeContainers = testBase.Client.VolumeContainers.ListByDevice(
                doubleEncodedDeviceName,
                testBase.ResourceGroupName,
                testBase.ManagerName);

            var volumeContainer = volumeContainers.FirstOrDefault(vc => vc.Name.Equals(volumeContainerToDelete.Name));

            Assert.True(volumeContainer == null, "Deletion of volume-container was not successful.");
        }
    }
}