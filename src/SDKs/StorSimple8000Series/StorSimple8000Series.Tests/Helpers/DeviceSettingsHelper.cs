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

        /// <summary>
        /// Authorize device for service data encryption key rollover.
        /// </summary>
        public static void AuthorizeDeviceForRollover(
            StorSimple8000SeriesTestBase testBase,
            string deviceName)
        {
            testBase.Client.Devices.AuthorizeForServiceEncryptionKeyRollover(
                deviceName.GetDoubleEncoded(),
                testBase.ResourceGroupName,
                testBase.ManagerName);
        }
    }
}