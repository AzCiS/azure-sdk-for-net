
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
    /// Defines values for ServiceRegion.
    /// </summary>
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum ServiceRegion
    {
        [EnumMember(Value = "westus")]
        Westus,
        [EnumMember(Value = "westeurope")]
        Westeurope,
        [EnumMember(Value = "northeurope")]
        Northeurope,
        [EnumMember(Value = "centraluseuap")]
        Centraluseuap
    }
}

