
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
    /// Defines values for StageStatus.
    /// </summary>
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum StageStatus
    {
        [EnumMember(Value = "None")]
        None,
        [EnumMember(Value = "InProgress")]
        InProgress,
        [EnumMember(Value = "Succeeded")]
        Succeeded,
        [EnumMember(Value = "Failed")]
        Failed,
        [EnumMember(Value = "Cancelled")]
        Cancelled,
        [EnumMember(Value = "Cancelling")]
        Cancelling,
        [EnumMember(Value = "SucceededWithErrors")]
        SucceededWithErrors
    }
}

