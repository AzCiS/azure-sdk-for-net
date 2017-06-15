﻿using System;
using System.Collections.Generic;
using System.Text;

namespace StorSimple8000Series.Tests
{
    public static class TestConstants
    {
        public const string DefaultResourceGroupName = "ResourceGroupForSDKTest";
        public const string DefaultManagerName = "ManagerForSDKTest1";
        public const string DefaultStorageAccountName = "safortestrecording";
        public const string DefaultStorageAccountAccessKey = "HvESZmug6HAx5tOmbtpOJsm/Mu/LdtsUcRaABGXqqJNnzV0WmzkLVzdrZjFZ3RkLAs+Oa00aM/9y8Xn47u6w/w==";
        public const string DefaultStorageAccountEndPoint = "blob.core.windows.net";
        public const string DefaultInitiatorName = "iqn.2017-06.com.contoso:uniquename";
        public const string FirstDeviceName = "Device05ForSDKTest";
        public const string SecondaryDnsServers = "10.67.64.15;10.67.64.14";
        public const string FirstDeviceControllerZeroIp = "10.168.241.122";
        public const string FirstDeviceControllerOneIp = "10.168.241.121";

        public static readonly DateTime MetricsStartTime = DateTime.Today.AddDays(-1);
        public static readonly DateTime MetricsEndTime = DateTime.Today;
    }
}
