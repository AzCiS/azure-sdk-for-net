
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
    /// Defines values for StageName.
    /// </summary>
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum StageName
    {
        [EnumMember(Value = "DeviceOrdered")]
        DeviceOrdered,
        [EnumMember(Value = "DevicePrepared")]
        DevicePrepared,
        [EnumMember(Value = "Dispatched")]
        Dispatched,
        [EnumMember(Value = "Delivered")]
        Delivered,
        [EnumMember(Value = "PickedUp")]
        PickedUp,
        [EnumMember(Value = "AtAzureDC")]
        AtAzureDC,
        [EnumMember(Value = "DataCopy")]
        DataCopy,
        [EnumMember(Value = "Completed")]
        Completed,
        [EnumMember(Value = "CompletedWithErrors")]
        CompletedWithErrors,
        [EnumMember(Value = "Cancelled")]
        Cancelled,
        [EnumMember(Value = "Failed_IssueReportedAtCustomer")]
        FailedIssueReportedAtCustomer,
        [EnumMember(Value = "Failed_IssueDetectedAtAzureDC")]
        FailedIssueDetectedAtAzureDC,
        [EnumMember(Value = "Aborted")]
        Aborted
    }
}

