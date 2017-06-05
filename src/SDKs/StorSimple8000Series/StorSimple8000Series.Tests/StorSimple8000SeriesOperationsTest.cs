using Microsoft.Azure.Management.StorSimple8000Series;
using Microsoft.Azure.Management.StorSimple8000Series.Models;
using Microsoft.Rest.ClientRuntime.Azure.TestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace StorSimple8000Series.Tests
{
    public class StorSimple8000SeriesTest : TestBase
    {
        [Fact]
        public void TestStorsimpleOperations()
        {
            using (MockContext context = MockContext.Start(this.GetType().FullName))
            {
                var testBase = new StorSimple8000SeriesTestBase(context);

                //create StorSimple Manager
                var manager = CreateManagerAndValidate();

                //Get Device Registration Key
                var registrationKey = GetDeviceRegistrationKey(testBase);

                //Configure Device
                var device = ConfigureAndGetDevice(testBase);

                //Create SAC
                var sac = CreateStorageAccountCredential(testBase);

                //Create Volume Container
                var vc = CreateVolumeContainer(testBase, device.Name);

            }
        }

        /// <summary>
        /// Get the device registration key.
        /// </summary>
        private string GetDeviceRegistrationKey(StorSimple8000SeriesTestBase testBase)
        {
            return testBase.client.Managers.GetDeviceRegistrationKey(testBase.ResourceGroupName, testBase.ManagerName);
        }

        /// <summary>
        /// Configure device and get the device.
        /// </summary>
        /// <param name="testBase">The testbase.</param>
        /// <returns></returns>
        private Device ConfigureAndGetDevice(StorSimple8000SeriesTestBase testBase)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create storage account credential.
        /// </summary>
        /// <param name="testBase">The testbase.</param>
        /// <returns></returns>
        private StorageAccountCredential CreateStorageAccountCredential(StorSimple8000SeriesTestBase testBase)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create Volume Container.
        /// </summary>
        /// <param name="testBase"></param>
        /// <param name="deviceName"></param>
        /// <returns></returns>
        private VolumeContainer CreateVolumeContainer(StorSimple8000SeriesTestBase testBase, string deviceName)
        {
            throw new NotImplementedException();
        }
    }
}
