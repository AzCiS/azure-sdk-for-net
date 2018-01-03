
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
    /// Defines values for AddressValidationStatus.
    /// </summary>
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum AddressValidationStatus
    {
        [EnumMember(Value = "Valid")]
        Valid,
        [EnumMember(Value = "Invalid")]
        Invalid,
        [EnumMember(Value = "Ambiguous")]
        Ambiguous
    }
}

