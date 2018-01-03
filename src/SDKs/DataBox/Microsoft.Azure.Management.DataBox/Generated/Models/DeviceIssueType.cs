
namespace Microsoft.Azure.Management.DataBox.Models
{
    using Azure;
    using Management;
    using DataBox;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;
    using System.Runtime;
    using System.Runtime.Serialization;

    /// <summary>
    /// Defines values for DeviceIssueType.
    /// </summary>
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum DeviceIssueType
    {
        [EnumMember(Value = "DeviceTampering")]
        DeviceTampering,
        [EnumMember(Value = "DeviceNotBootingUp")]
        DeviceNotBootingUp,
        [EnumMember(Value = "DeviceHealthCheckShowFailures")]
        DeviceHealthCheckShowFailures,
        [EnumMember(Value = "NICsAreNotWorking")]
        NICsAreNotWorking,
        [EnumMember(Value = "Misc")]
        Misc
    }
}

