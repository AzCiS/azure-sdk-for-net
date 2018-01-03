
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
    /// Defines values for CopyStatus.
    /// </summary>
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum CopyStatus
    {
        [EnumMember(Value = "NotStarted")]
        NotStarted,
        [EnumMember(Value = "InProgress")]
        InProgress,
        [EnumMember(Value = "Completed")]
        Completed,
        [EnumMember(Value = "CompletedWithErrors")]
        CompletedWithErrors,
        [EnumMember(Value = "Failed")]
        Failed
    }
}

