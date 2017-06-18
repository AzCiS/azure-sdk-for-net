﻿using System;
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
    public class DeviceTests : StorSimpleTestBase
    {
        public DeviceTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper) { }

        [Fact]
        public void TestDeviceOperations()
        {
            //List devices
            var devices = GetDevices();
            var deviceName = devices.Where(d => d.Status == DeviceStatus.ReadyToSetup).First().Name;

            //Get device
            var device = GetByName(deviceName);

            //Configure device
            ConfigureDevice(deviceName);

            //Update device
            UpdateDevice(deviceName, "updated device description");

            //Get device
            device = GetByName(deviceName);

            //Deactivate device
            Deactivate(deviceName);

            //Delete device
            Delete(deviceName);
        }

        [Fact]
        public void TestServiceDataEncryptionKeyRolloverOnConfiguredDevices()
        {
            {
                //checking for prerequisites
                var device = Helpers.CheckAndGetConfiguredDevices(this, TestConstants.DeviceForKeyRollover);
                var firstDeviceName = device.Name;

                try
                {
                    //Authorize device for Key rollover.
                    AuthorizeDeviceForRollover(firstDeviceName);
                }
                catch (Exception e)
                {
                    Assert.Null(e);
                }
            }
        }

        private Device ConfigureAndGetDevice(string deviceNameWithoutDoubleEncoding, string controllerZeroIp, string controllerOneIp)
        {
            Device device = this.Client.Devices.Get(
                deviceNameWithoutDoubleEncoding.GetDoubleEncoded(),
                this.ResourceGroupName,
                this.ManagerName);

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

                this.Client.Devices.Configure(
                    configureDeviceRequest,
                    this.ResourceGroupName,
                    this.ManagerName);

                //need to add details related to odata filter
                device = this.Client.Devices.Get(
                    deviceNameWithoutDoubleEncoding.GetDoubleEncoded(),
                    this.ResourceGroupName,
                    this.ManagerName);
            }

            return device;
        }

        /// <summary>
        /// Deletes the specified device.
        /// </summary>
        private void DeleteDevice(string managerName, string deviceName)
        {
            var deviceToDelete = this.Client.Devices.Get(
                deviceName.GetDoubleEncoded(),
                this.ResourceGroupName,
                this.ManagerName);

            Assert.True(deviceToDelete.Status == DeviceStatus.Deactivated, "For deletion, device should be in deactivated state.");

            this.Client.Devices.Delete(
                deviceToDelete.Name.GetDoubleEncoded(),
                this.ResourceGroupName,
                this.ManagerName);

            var devices = this.Client.Devices.ListByManager(
                this.ResourceGroupName,
                this.ManagerName);

            var device = devices.FirstOrDefault(d => d.Name.Equals(deviceToDelete.Name));

            Assert.True(device == null, "Device deletion was not successful.");
        }

        /// <summary>
        /// Get list of devices.
        /// </summary>
        private IEnumerable<Device> GetDevices()
        {
            return this.Client.Devices.ListByManager(
                this.ResourceGroupName,
                this.ManagerName,
                "details");
        }

        /// <summary>
        /// Get details of a particular device.
        /// </summary>
        private Device GetByName(string deviceName)
        {
            return this.Client.Devices.Get(
                deviceName,
                this.ResourceGroupName,
                this.ManagerName,
                "details");
        }

        /// <summary>
        /// Complete mandatory configuration of a device.
        /// </summary>
        private void ConfigureDevice(string deviceName)
        {
            ConfigureDeviceRequest configureRequest = new ConfigureDeviceRequest();
            configureRequest.CurrentDeviceName = deviceName;
            configureRequest.FriendlyName = deviceName;
            configureRequest.TimeZone = "Pacific Standard Time";
            configureRequest.DnsSettings = null;
            NetworkInterfaceData0Settings data0NetworkSetting = new NetworkInterfaceData0Settings();
            data0NetworkSetting.ControllerZeroIp = "10.168.220.227";
            data0NetworkSetting.ControllerOneIp = "10.168.220.228";
            configureRequest.NetworkInterfaceData0Settings = data0NetworkSetting;

            this.Client.Devices.Configure(
                configureRequest,
                this.ResourceGroupName,
                this.ManagerName);
        }

        /// <summary>
        /// Update the device description.
        /// </summary>
        private void UpdateDevice(string deviceName, string description)
        {
            DevicePatch devicePatch = new DevicePatch();
            devicePatch.DeviceDescription = description;

            this.Client.Devices.Update(
                deviceName,
                devicePatch,
                this.ResourceGroupName,
                this.ManagerName);
        }

        /// <summary>
        /// Deactivates the device.
        /// </summary>
        private void Deactivate(string deviceName)
        {
            this.Client.Devices.Deactivate(
                deviceName,
                this.ResourceGroupName,
                this.ManagerName);
        }

        /// <summary>
        /// Deletes the device.
        /// </summary>
        private void Delete(string deviceName)
        {
            this.Client.Devices.Delete(
                deviceName,
                this.ResourceGroupName,
                this.ManagerName);
        }

        /// <summary>
        /// Authorize device for service data encryption key rollover.
        /// </summary>
        private void AuthorizeDeviceForRollover(string deviceName)
        {
            this.Client.Devices.AuthorizeForServiceEncryptionKeyRollover(
                deviceName.GetDoubleEncoded(),
                this.ResourceGroupName,
                this.ManagerName);
        }
    }
}
