
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
    /// Defines values for IssueType.
    /// </summary>
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum IssueType
    {
        [EnumMember(Value = "DeviceMismatch")]
        DeviceMismatch,
        [EnumMember(Value = "ValidationStringMismatch")]
        ValidationStringMismatch,
        [EnumMember(Value = "CredentialNotWorking")]
        CredentialNotWorking,
        [EnumMember(Value = "DeviceFailure")]
        DeviceFailure
    }
}

