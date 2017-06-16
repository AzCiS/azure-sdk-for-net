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
        /// List hardware component groups on the device.
        /// </summary>
        public static IEnumerable<HardwareComponentGroup> GetHardwareComponentGroups(StorSimple8000SeriesTestBase testBase, string deviceName)
        {
            var hardwareComponents = testBase.Client.HardwareComponentGroups.ListByDevice(
                deviceName,
                testBase.ResourceGroupName,
                testBase.ManagerName);

            return hardwareComponents;
        }

		/// <summary>
        /// Changes the power state of the controller.
        /// </summary>
        public static void ChangeControllerPowerState(StorSimple8000SeriesTestBase testBase, string deviceName, string hardwareComponentGroupName,
        ControllerId activeController, ControllerPowerStateAction controllerAction)
        {
            ControllerPowerStateChangeRequest powerChangeRequest = new ControllerPowerStateChangeRequest();
            powerChangeRequest.ActiveController = activeController;
            powerChangeRequest.Controller0State = ControllerStatus.Ok;
            powerChangeRequest.Controller1State = ControllerStatus.NotPresent;
            powerChangeRequest.Action = controllerAction;

            testBase.Client.HardwareComponentGroups.ChangeControllerPowerState(
                deviceName,
                hardwareComponentGroupName,
                powerChangeRequest,
                testBase.ResourceGroupName,
                testBase.ManagerName);
        }
    }
}