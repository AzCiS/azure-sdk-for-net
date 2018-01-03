
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
    /// Defines values for AddressType.
    /// </summary>
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum AddressType
    {
        [EnumMember(Value = "None")]
        None,
        [EnumMember(Value = "Residential")]
        Residential,
        [EnumMember(Value = "Commercial")]
        Commercial
    }
}

