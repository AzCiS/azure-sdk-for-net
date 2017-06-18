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
using Microsoft.Azure.Test.HttpRecorder;
using Microsoft.Azure.Management.Compute;
using Microsoft.Azure.Management.Network;
using Microsoft.Rest.Azure.OData;

namespace StorSimple8000Series.Tests
{
    public class DeviceSettingsTests : StorSimpleTestBase
    {
        public DeviceSettingsTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper) { }

        [Fact]
        public void TestDeviceSettingsOperationsOnConfiguredDevices()
        {
            var devices = Helpers.CheckAndGetConfiguredDevices(this, requiredCount: 1);
            var deviceName = devices.First().Name;

            try
            {
                //Create Time Settings
                var timeSettings = CreateTimeSettings(deviceName);

                //Create Alert Settings
                var alertSettings = CreateAlertSettings(deviceName);

                //Create Network Settings
                var bws = CreateNetworkSettings(deviceName);

                //Create Security Settings
                var securitySettings = CreateSecuritySettings(deviceName);
            }
            catch (Exception e)
            {
                Assert.Null(e);
            }

        }

        private AlertSettings CheckAndGetDeviceAlertSettings(string deviceName)
        {
            var alertSettings = this.Client.DeviceSettings.GetAlertSettings(
                                    deviceName,
                                    this.ResourceGroupName,
                                    this.ManagerName);

            Assert.True(alertSettings != null, string.Format("Alert Settings is not present on device"));
            return alertSettings;
        }

        private TimeSettings CheckAndGetDeviceTimeSettings(string deviceName)
        {
            var timeSettings = this.Client.DeviceSettings.GetTimeSettings(
                                    deviceName,
                                    this.ResourceGroupName,
                                    this.ManagerName);

            Assert.True(timeSettings != null, string.Format("Time Settings is not present on device"));
            return timeSettings;
        }

        private NetworkSettings CheckAndGetDeviceNetworkSettings(string deviceName)
        {
            var networkSettings = this.Client.DeviceSettings.GetNetworkSettings(
                                    deviceName,
                                    this.ResourceGroupName,
                                    this.ManagerName);

            Assert.True(networkSettings != null, string.Format("Network Settings is not present on device"));
            return networkSettings;
        }

        private SecuritySettings CheckAndGetDeviceSecuritySettings(string deviceName)
        {
            var securitySettings = this.Client.DeviceSettings.GetSecuritySettings(
                                    deviceName,
                                    this.ResourceGroupName,
                                    this.ManagerName);

            Assert.True(securitySettings != null, string.Format("Security Settings is not present on device"));
            return securitySettings;
        }

        /// <summary>
        /// Create TimeSettings on the Device.
        /// </summary>
        private TimeSettings CreateAndValidateTimeSettings(string deviceName)
        {
            TimeSettings timeSettingsToCreate = new TimeSettings("Pacific Standard Time");
            timeSettingsToCreate.PrimaryTimeServer = "time.windows.com";
            timeSettingsToCreate.SecondaryTimeServer = new List<string>() { "8.8.8.8" };

            this.Client.DeviceSettings.CreateOrUpdateTimeSettings(
                    deviceName.GetDoubleEncoded(),
                    timeSettingsToCreate,
                    this.ResourceGroupName,
                    this.ManagerName);

            var timeSettings = this.Client.DeviceSettings.GetTimeSettings(
                deviceName.GetDoubleEncoded(),
                this.ResourceGroupName,
                this.ManagerName);

            //validation
            Assert.True(timeSettings != null && timeSettings.PrimaryTimeServer.Equals("time.windows.com") &&
                timeSettings.SecondaryTimeServer.Equals("8.8.8.8"), "Creation of Time Setting was not successful.");

            return timeSettings;
        }

        /// <summary>
        /// Create AlertSettings on the Device.
        /// </summary>
        private AlertSettings CreateAndValidateAlertSettings(string deviceName)
        {
            AlertSettings alertsettingsToCreate = new AlertSettings(AlertEmailNotificationStatus.Enabled);
            alertsettingsToCreate.AlertNotificationCulture = "en-US";
            alertsettingsToCreate.NotificationToServiceOwners = AlertEmailNotificationStatus.Enabled;

            alertsettingsToCreate.AdditionalRecipientEmailList = new List<string>();

            this.Client.DeviceSettings.CreateOrUpdateAlertSettings(
                    deviceName.GetDoubleEncoded(),
                    alertsettingsToCreate,
                    this.ResourceGroupName,
                    this.ManagerName);

            var alertSettings = this.Client.DeviceSettings.GetAlertSettings(
                deviceName.GetDoubleEncoded(),
                this.ResourceGroupName,
                this.ManagerName);

            //validation
            Assert.True(alertSettings != null && alertSettings.AlertNotificationCulture.Equals("en-US") &&
                alertSettings.EmailNotification.Equals(AlertEmailNotificationStatus.Enabled) &&
                alertSettings.NotificationToServiceOwners.Equals(AlertEmailNotificationStatus.Enabled), "Creation of Alert Setting was not successful.");

            return alertSettings;
        }

        /// <summary>
        /// Create NetworkSettings on the Device.
        /// </summary>
        private NetworkSettings CreateAndValidateNetworkSettings(string deviceName)
        {
            var networkSettingsBeforeUpdate = this.Client.DeviceSettings.GetNetworkSettings(
                deviceName.GetDoubleEncoded(),
				this.ResourceGroupName,
				this.ManagerName
            );
            
            DNSSettings dnsSettings = new DNSSettings();
            dnsSettings.PrimaryDnsServer = networkSettingsBeforeUpdate.DnsSettings.PrimaryDnsServer;
            dnsSettings.SecondaryDnsServers = new List<string>() { "8.8.8.8" };

            NetworkSettingsPatch networkSettingsPatch = new NetworkSettingsPatch();
            networkSettingsPatch.DnsSettings = dnsSettings;
            
            return testBase.Client.DeviceSettings.UpdateNetworkSettings(
                    deviceName.GetDoubleEncoded(),
                    networkSettingsPatch,
                    testBase.ResourceGroupName,
                    testBase.ManagerName);
        }

        /// <summary>
        /// Create SecuritySettings on the Device.
        /// </summary>
        public static SecuritySettings CreateAndValidateSecuritySettings(
            StorSimple8000SeriesTestBase testBase,
            string deviceName)
        {
            RemoteManagementSettingsPatch remoteManagementSettings = new RemoteManagementSettingsPatch(RemoteManagementModeConfiguration.HttpsAndHttpEnabled);
            AsymmetricEncryptedSecret deviceAdminpassword = this.Client.Managers.GetAsymmetricEncryptedSecret(this.ResourceGroupName, this.ManagerName, "test-secret");
            AsymmetricEncryptedSecret snapshotmanagerPassword = this.Client.Managers.GetAsymmetricEncryptedSecret(this.ResourceGroupName, this.ManagerName, "test-secret1");

            ChapSettings chapSettings = new ChapSettings("test-initiator-user",
                this.Client.Managers.GetAsymmetricEncryptedSecret(this.ResourceGroupName, this.ManagerName, "test-chapsecret1"),
            "test-target-user",
                this.Client.Managers.GetAsymmetricEncryptedSecret(this.ResourceGroupName, this.ManagerName, "test-chapsecret2"));

            SecuritySettingsPatch securitySettingsPatch = new SecuritySettingsPatch(remoteManagementSettings, deviceAdminpassword, snapshotmanagerPassword, chapSettings);

            this.Client.DeviceSettings.UpdateSecuritySettings(
                    deviceName.GetDoubleEncoded(),
                    securitySettingsPatch,
                    this.ResourceGroupName,
                    this.ManagerName);

            var securitySettings = this.Client.DeviceSettings.GetSecuritySettings(
                deviceName.GetDoubleEncoded(),
                this.ResourceGroupName,
                this.ManagerName);

            //validation
            Assert.True(securitySettings != null && securitySettings.RemoteManagementSettings.Equals(RemoteManagementModeConfiguration.HttpsAndHttpEnabled) &&
                securitySettings.ChapSettings.InitiatorUser.Equals("test-initiator-user") &&
                securitySettings.ChapSettings.TargetUser.Equals("test-target-user"), "Creation of Security Setting was not successful.");

            return securitySettings;
        }
    }
}