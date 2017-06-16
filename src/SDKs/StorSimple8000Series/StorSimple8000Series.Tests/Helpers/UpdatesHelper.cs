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
        /// Get the update summary of a device.
        /// </summary>
        public static Updates GetUpdateSummary(StorSimple8000SeriesTestBase testBase, string deviceName)
        {
            var updateSummary = testBase.Client.Devices.GetUpdateSummary(
                deviceName,
                testBase.ResourceGroupName,
                testBase.ManagerName);

            return updateSummary;
        }

		/// <summary>
        /// Scan for new updates.
        /// </summary>
        public static void ScanForUpdates(StorSimple8000SeriesTestBase testBase, string deviceName)
        {
            testBase.Client.Devices.ScanForUpdates(
                deviceName,
                testBase.ResourceGroupName,
                testBase.ManagerName);
        }

		/// <summary>
        /// Install updates.
        /// </summary>
        public static void InstallUpdates(StorSimple8000SeriesTestBase testBase, string deviceName)
        {
            testBase.Client.Devices.InstallUpdates(
                deviceName,
                testBase.ResourceGroupName,
                testBase.ManagerName);
        }
    }
}