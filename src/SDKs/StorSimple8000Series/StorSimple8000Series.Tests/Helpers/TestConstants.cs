using System;
using System.Collections.Generic;
using System.Text;

namespace StorSimple8000Series.Tests
{
    public static class TestConstants
    {
        public const string DefaultResourceGroupName = "ResourceGroupForSDKTest";
        public const string DefaultManagerName = "ManagerForSDKTest1";
        public const string ManagerForManagerOperationTests = "ManagerForSDKTest2";
        public const string DefaultVirtualNetworkName = "ClientSdkTest";
        public const string DefaultSubnetName = "Default";
        public const string DefaultStorageAccountEndPoint = "blob.core.windows.net";
        public const string DefaultDeviceName = "Device05ForSDKTest";
        public const string DeviceForFailover = "FailoverTargetDevice";
        public const string DeviceForKeyRollover = "DeviceForKey";
        public const string DeviceForUpdateTests = "DeviceForTestingUpdates";
        public const string DeviceForMonitoringTest = "Device05ForSDKTest";
        public const string SecondaryDnsServers = "10.67.64.15;10.67.64.14";
        public const string FirstDeviceControllerZeroIp = "10.168.241.122";
        public const string FirstDeviceControllerOneIp = "10.168.241.121";

        public static readonly DateTime MetricsStartTime = DateTime.Today.AddDays(-1);
        public static readonly DateTime MetricsEndTime = DateTime.Today;
    }
}
