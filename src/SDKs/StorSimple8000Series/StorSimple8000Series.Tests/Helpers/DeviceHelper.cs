using System.Linq.Expressions;
using Xunit;
using Microsoft.Azure.Management.StorSimple8000Series;
using Microsoft.Azure.Management.StorSimple8000Series.Models;
using Microsoft.Rest.Azure.OData;
using SSModels = Microsoft.Azure.Management.StorSimple8000Series.Models;
using System.Collections.Generic;
using System.Linq;

namespace StorSimple8000Series.Tests
{
    public static partial class Helpers
    {
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

        /// <summary>
        /// Deletes the specified device.
        /// </summary>
        public static void DeleteDevice(StorSimple8000SeriesTestBase testBase, string managerName, string deviceName)
        {
            var deviceToDelete = testBase.Client.Devices.Get(
                deviceName.GetDoubleEncoded(),
                testBase.ResourceGroupName,
                testBase.ManagerName);

            Assert.True(deviceToDelete.Status == DeviceStatus.Deactivated, "For deletion, device should be in deactivated state.");

            testBase.Client.Devices.Delete(
                deviceToDelete.Name.GetDoubleEncoded(),
                testBase.ResourceGroupName,
                testBase.ManagerName);

            var devices = testBase.Client.Devices.ListByManager(
                testBase.ResourceGroupName,
                testBase.ManagerName);

            var device = devices.FirstOrDefault(d => d.Name.Equals(deviceToDelete.Name));

            Assert.True(device == null, "Device deletion was not successful.");
        }
		
		/// <summary>
        /// Get list of devices.
        /// </summary>
        public static IEnumerable<Device> GetDevices(StorSimple8000SeriesTestBase testBase)
        {
            return testBase.Client.Devices.ListByManager(
                testBase.ResourceGroupName,
                testBase.ManagerName,
                "details");
        }

		/// <summary>
        /// Get details of a particular device.
        /// </summary>
        public static Device GetByName(StorSimple8000SeriesTestBase testBase, string deviceName)
        {
            return testBase.Client.Devices.Get(
                deviceName,
                testBase.ResourceGroupName,
                testBase.ManagerName,
                "details");
        }

		/// <summary>
        /// Complete mandatory configuration of a device.
        /// </summary>
        public static void ConfigureDevice(StorSimple8000SeriesTestBase testBase, string deviceName)
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

            testBase.Client.Devices.Configure(
                configureRequest,
                testBase.ResourceGroupName,
                testBase.ManagerName);
        }

		/// <summary>
        /// Update the device description.
        /// </summary>
        public static void UpdateDevice(StorSimple8000SeriesTestBase testBase, string deviceName, string description)
        {
            DevicePatch devicePatch = new DevicePatch();
            devicePatch.DeviceDescription = description;

            testBase.Client.Devices.Update(
                deviceName,
                devicePatch,
                testBase.ResourceGroupName,
                testBase.ManagerName);
        }

		/// <summary>
        /// Deactivates the device.
        /// </summary>
        public static void Deactivate(StorSimple8000SeriesTestBase testBase, string deviceName)
        {
            testBase.Client.Devices.Deactivate(
                deviceName,
                testBase.ResourceGroupName,
                testBase.ManagerName);
        }

		/// <summary>
        /// Deletes the device.
        /// </summary>
        public static void Delete(StorSimple8000SeriesTestBase testBase, string deviceName)
        {
            testBase.Client.Devices.Delete(
                deviceName,
                testBase.ResourceGroupName,
                testBase.ManagerName);
        }
    }
}