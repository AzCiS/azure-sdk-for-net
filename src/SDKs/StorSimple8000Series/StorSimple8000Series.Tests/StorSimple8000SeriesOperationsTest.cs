using Microsoft.Azure.Management.StorSimple8000Series;
using Microsoft.Azure.Management.StorSimple8000Series.Models;
using Microsoft.Rest.Azure.OData;
using Microsoft.Rest.ClientRuntime.Azure.TestFramework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace StorSimple8000Series.Tests
{
    public class StorSimple8000SeriesTest : TestBase
    {
        [Fact]
        public void TestStorsimpleOperationsOnConfiguredDevices()
        {
            using (MockContext context = MockContext.Start(this.GetType().FullName))
            {
                var testBase = new StorSimple8000SeriesTestBase(context);

                string firstDeviceName = null;
                string secondDeviceName = null;
                var requiredCountOfConfiguredDevices = 1;
                CheckPrerequisitesAndInitialize(testBase, requiredCountOfConfiguredDevices, out firstDeviceName, out secondDeviceName);

                try
                {
                    //Create SAC
                    var sac = CreateStorageAccountCredential(testBase, TestConstants.DefaultStorageAccountName, TestConstants.DefaultStorageAccountAccessKey);

                    //Create ACR
                    var acr = CreateAccessControlRecord(testBase, Helpers.GenerateRandomName("ACRForSDKTest"), Helpers.GenerateRandomName(TestConstants.DefaultInitiatorName));

                    //Create Volume Container
                    var vc = CreateVolumeContainer(testBase, firstDeviceName, Helpers.GenerateRandomName("VCForSDKTest"));

                    //Create volumes
                    var vol = CreateVolume(testBase, firstDeviceName, vc.Name, Helpers.GenerateRandomName("VolForSDKTest"));
                }
                catch (Exception e)
                {
                    Assert.Null(e);
                }
                finally
                {
                    //Delete all entities created in the devices
                }
            }
        }

        [Fact]
        public void CreateStorSimpleManagerAndConfigureDevice()
        {
            using (MockContext context = MockContext.Start(this.GetType().FullName))
            {
                var testBase = new StorSimple8000SeriesTestBase(context);

                //create StorSimple Manager
                //var manager = CreateManager(testBase, testBase.ManagerName);

                //Get Device Registration Key
                var registrationKey = GetDeviceRegistrationKey(testBase);

                //Configure and Get device
                var device = ConfigureAndGetDevice(testBase, TestConstants.FirstDeviceName, TestConstants.FirstDeviceControllerZeroIp, TestConstants.FirstDeviceControllerOneIp);

                //TODO: Deactivate device

                //TODO: Delete device

                //TODO: Delete StorSimple Manager
            }
        }

        [Fact]
        public void TestMetricOperations()
        {
            using (MockContext context = MockContext.Start(this.GetType().FullName))
            {
                var testBase = new StorSimple8000SeriesTestBase(context);

                //checking for prerequisites
                var devices = Helpers.CheckAndGetConfiguredDevices(testBase, requiredCount: 1);
                var volumeContainers = Helpers.CheckAndGetVolumeContainers(testBase, devices.First().Name, requiredCount: 1);
                var volumes = Helpers.CheckAndGetVolumes(testBase, devices.First().Name, volumeContainers.First().Name, requiredCount: 1);

                var deviceName = devices.First().Name;
                var volumeContainerName = volumeContainers.First().Name;
                var volumeName = volumes.First().Name;


                //Get MetricDefinitions for Manager
                var resourceMetricDefinition = Helpers.GetManagerMetricDefinitions(testBase);

                //Get MetricDefinitions for Device
                var deviceMetricDefinition = Helpers.GetDeviceMetricDefinition(testBase, deviceName);

                //Get MetricDefinitions for VolumeContainer
                var volumeContainerMetricDefinition = Helpers.GetVolumeContainerMetricDefinition(testBase, deviceName, volumeContainerName);

                //Get MetricDefinitions for Volume
                var volumeMetricDefinition = Helpers.GetVolumeMetricDefinition(testBase, deviceName, volumeContainerName, volumeName);

                //Get Metrics for Manager
                var resourceMetrics = Helpers.GetManagerMetrics(testBase, resourceMetricDefinition.First());

                //Get Metrics for Device
                var deviceMetrics = Helpers.GetDeviceMetrics(testBase, deviceName, deviceMetricDefinition.First());

                //Get Metrics for VolumeContainer
                var volumeContainerMetrics = Helpers.GetVolumeContainerMetrics(testBase, deviceName, volumeContainerName, volumeContainerMetricDefinition.First());

                //Get Metrics for Volume
                var volumeMetrics = Helpers.GetVolumeMetrics(testBase, deviceName, volumeContainerName, volumeName, volumeMetricDefinition.First());

            }
        }


        #region Private wrappers starts
        
        private Manager CreateManager(StorSimple8000SeriesTestBase testBase, string managerName)
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

        private VolumeContainer CreateVolumeContainer(StorSimple8000SeriesTestBase testBase, string name)
        {
            throw new NotImplementedException();
        }

        private Volume CreateVolume(StorSimple8000SeriesTestBase testBase, string name, string name2)
        {
            throw new NotImplementedException();
        }

        private Manager CreateManagerAndValidate()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get the device registration key.
        /// </summary>
        private string GetDeviceRegistrationKey(StorSimple8000SeriesTestBase testBase)
        {
            return testBase.Client.Managers.GetDeviceRegistrationKey(testBase.ResourceGroupName, testBase.ManagerName);
        }

        /// <summary>
        /// Configure device and get the device.
        /// </summary>
        private Device ConfigureAndGetDevice(StorSimple8000SeriesTestBase testBase, string deviceNameWithoutDoubleEncoding, string controllerZeroIp, string controllerOneIp)
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
                    TimeZone = TimeZoneInfo.Local.DisplayName,
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
        /// Checks if minimum number of configured devices required for the testcase exists. If yes, populates names of them in the out-parameters.
        /// </summary>
        /// <param name="testBase">The test base.</param>
        /// <param name="minimumConfigureDevicesCount">The minimum number of devices required to be configured for the testcase.</param>
        /// <param name="firstDeviceName">The name of first configured device.</param>
        /// <param name="secondDeviceName">The name of second configured device.</param>
        private void CheckPrerequisitesAndInitialize(StorSimple8000SeriesTestBase testBase, int minimumConfigureDevicesCount, out string firstDeviceName, out string secondDeviceName)
        {
            var devices = testBase.Client.Devices.ListByManager(testBase.ResourceGroupName, testBase.ManagerName);

            var countOfConfiguredDevicesFound = 0;
            var configuredDeviceNames = new List<string>();

            foreach (var device in devices)
            {
                if (device.Status == DeviceStatus.Online)
                {
                    countOfConfiguredDevicesFound++;
                    configuredDeviceNames.Append(device.Name);
                }
            }

            Assert.True(countOfConfiguredDevicesFound >= minimumConfigureDevicesCount, string.Format("Minimum configured devices: Required={0}, ActuallyFound={1}", minimumConfigureDevicesCount, countOfConfiguredDevicesFound));

            firstDeviceName = minimumConfigureDevicesCount > 0 ? configuredDeviceNames[0] : null;
            secondDeviceName = minimumConfigureDevicesCount > 1 ? configuredDeviceNames[1] : null;
        }

        /// <summary>
        /// Create storage account credential.
        /// </summary>
        private StorageAccountCredential CreateStorageAccountCredential(StorSimple8000SeriesTestBase testBase, string sacNameWithoutDoubleEncoding, string sacAccessKeyInPlainText)
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

            return sac;
        }

        /// <summary>
        /// Create Volume Container.
        /// </summary>
        private VolumeContainer CreateVolumeContainer(StorSimple8000SeriesTestBase testBase, string deviceName, string volumeContainerName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates Access control record.
        /// </summary>
        private AccessControlRecord CreateAccessControlRecord(StorSimple8000SeriesTestBase testBase, string acrNameWithoutDoubleEncoding, string initiatorName)
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

            return acr;            
        }

        /// <summary>
        /// Creates Volume.
        /// </summary>
        private Volume CreateVolume(StorSimple8000SeriesTestBase testBase, string deviceName, string volumeContainerName, string volumeNameWithoutDoubleEncoding)
        {
            throw new NotImplementedException();
        }

        #endregion Private wrappers ends
    }
}
