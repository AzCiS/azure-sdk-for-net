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

            var sac = testBase.Client.StorageAccountCredentials.CreateOrUpdate(
                sacNameWithoutDoubleEncoding.GetDoubleEncoded(),
                sacToCreate,
                testBase.ResourceGroupName,
                testBase.ManagerName);

            Assert.True(sac != null && sac.Name.Equals(sacNameWithoutDoubleEncoding) &&
                sac.SslStatus.Equals(SslStatus.Enabled) &&
                sac.EndPoint.Equals(TestConstants.DefaultStorageAccountEndPoint),
                "Creation of SAC was not successful.");

            return sac;
        }

        /// <summary>
        /// Creates Access control record.
        /// </summary>
        public static AccessControlRecord CreateAccessControlRecord(StorSimple8000SeriesTestBase testBase, string acrNameWithoutDoubleEncoding, string initiatorName)
        {
            var acrToCreate = new AccessControlRecord()
            {
                InitiatorName = initiatorName
            };

            var acr = testBase.Client.AccessControlRecords.CreateOrUpdate(
                acrNameWithoutDoubleEncoding.GetDoubleEncoded(),
                acrToCreate,
                testBase.ResourceGroupName,
                testBase.ManagerName);


            Assert.True(acr != null && acr.Name.Equals(acrNameWithoutDoubleEncoding) &&
                acr.InitiatorName.Equals(initiatorName),
                "Creation of ACR was not successful.");

            return acr;
        }

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

            var bws = testBase.Client.BandwidthSettings.CreateOrUpdate(
                bwsName.GetDoubleEncoded(),
                bwsToCreate,
                testBase.ResourceGroupName,
                testBase.ManagerName);

            //validation
            Assert.True(bws != null && bws.Name.Equals(bwsName) &&
                bws.Schedules != null && bws.Schedules.Count != 0, "Creation of Bandwidth Setting was not successful.");

            return bws;
        }

        /// <summary>
        /// Deletes the specified storage account credential.
        /// </summary>
        public static void DeleteStorageAccountCredentialAndValidate(StorSimple8000SeriesTestBase testBase, string storageAccountCredentialName)
        {
            var sacToDelete = testBase.Client.StorageAccountCredentials.Get(
                storageAccountCredentialName.GetDoubleEncoded(),
                testBase.ResourceGroupName,
                testBase.ManagerName);

            testBase.Client.StorageAccountCredentials.Delete(
                    sacToDelete.Name.GetDoubleEncoded(),
                    testBase.ResourceGroupName,
                    testBase.ManagerName);

            var storageAccountCredentials = testBase.Client.StorageAccountCredentials.ListByManager(
                testBase.ResourceGroupName,
                testBase.ManagerName);

            var sac = storageAccountCredentials.FirstOrDefault(s => s.Name.Equals(sacToDelete.Name));

            Assert.True(sac == null, "Deletion of storage account credential was not successful.");
        }

        /// <summary>
        /// Deletes the specified access control record.
        /// </summary>
        public static void DeleteAccessControlRecordAndValidate(StorSimple8000SeriesTestBase testBase, string acrName)
        {
            var acrToDelete = testBase.Client.AccessControlRecords.Get(
                acrName.GetDoubleEncoded(),
                testBase.ResourceGroupName,
                testBase.ManagerName);

            testBase.Client.AccessControlRecords.Delete(
                acrToDelete.Name.GetDoubleEncoded(),
                testBase.ResourceGroupName,
                testBase.ManagerName);

            var accessControlRecords = testBase.Client.AccessControlRecords.ListByManager(
                testBase.ResourceGroupName,
                testBase.ManagerName);

            var acr = accessControlRecords.FirstOrDefault(a => a.Name.Equals(acrToDelete.Name));

            Assert.True(acr == null, "Access control record deletion was not successful.");
        }

        /// <summary>
        /// Deletes the specified bandwidth setting.
        /// </summary>
        public static void DeleteBandwidthSettingAndValidate(StorSimple8000SeriesTestBase testBase, string bwsName)
        {
            var bwsToDelete = testBase.Client.BandwidthSettings.Get(
                bwsName.GetDoubleEncoded(),
                testBase.ResourceGroupName,
                testBase.ManagerName);

            testBase.Client.BandwidthSettings.Delete(
                bwsToDelete.Name.GetDoubleEncoded(),
                testBase.ResourceGroupName,
                testBase.ManagerName);

            var bandwidthSettings = testBase.Client.BandwidthSettings.ListByManager(
                testBase.ResourceGroupName,
                testBase.ManagerName);

            var bws = bandwidthSettings.FirstOrDefault(b => b.Name.Equals(bwsToDelete.Name));

            Assert.True(bws == null, "Bandwidth setting deletion was not successful.");
        }
    }
}