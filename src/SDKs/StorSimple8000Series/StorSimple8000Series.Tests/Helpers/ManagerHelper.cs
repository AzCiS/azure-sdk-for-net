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

        /// <summary>
        /// Get the device registration key.
        /// </summary>
        public static string GetDeviceRegistrationKey(StorSimple8000SeriesTestBase testBase)
        {
            return testBase.Client.Managers.GetDeviceRegistrationKey(testBase.ResourceGroupName, testBase.ManagerName);
        }

        /// <summary>
        /// Deletes the manager-extendedInfo for the specified StorSimple Manager.
        /// </summary>
        public static void DeleteManagerExtendedInfo(StorSimple8000SeriesTestBase testBase, string managerName)
        {
            var extendedInfo = testBase.Client.Managers.GetExtendedInfo(
                testBase.ResourceGroupName,
                managerName);

            testBase.Client.Managers.DeleteExtendedInfo(
                testBase.ResourceGroupName,
                managerName);
        }

        /// <summary>
        /// Deletes the specified StorSimple Manager.
        /// </summary>
        public static void DeleteManager(StorSimple8000SeriesTestBase testBase, string managerName)
        {
            var managerToDelete = testBase.Client.Managers.Get(
                testBase.ResourceGroupName,
                managerName);

            testBase.Client.Managers.Delete(
               testBase.ResourceGroupName,
               managerToDelete.Name);

            var managers = testBase.Client.Managers.ListByResourceGroup(testBase.ResourceGroupName);

            var manager = managers.FirstOrDefault(m => m.Name.Equals(managerName));

            Assert.True(manager == null, "Manager deletion was not successful.");
        }

    }
}