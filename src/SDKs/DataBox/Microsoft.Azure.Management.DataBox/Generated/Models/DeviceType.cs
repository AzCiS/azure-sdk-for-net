
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
    /// Defines values for DeviceType.
    /// </summary>
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum DeviceType
    {
        [EnumMember(Value = "Pod")]
        Pod,
        [EnumMember(Value = "Disk")]
        Disk,
        [EnumMember(Value = "Cabinet")]
        Cabinet
    }
}

