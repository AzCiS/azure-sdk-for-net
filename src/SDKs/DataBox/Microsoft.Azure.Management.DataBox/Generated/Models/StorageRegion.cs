
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
    /// Defines values for StorageRegion.
    /// </summary>
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum StorageRegion
    {
        [EnumMember(Value = "westus")]
        Westus,
        [EnumMember(Value = "centralus")]
        Centralus,
        [EnumMember(Value = "eastus")]
        Eastus,
        [EnumMember(Value = "northcentralus")]
        Northcentralus,
        [EnumMember(Value = "southcentralus")]
        Southcentralus,
        [EnumMember(Value = "eastus2")]
        Eastus2,
        [EnumMember(Value = "westus2")]
        Westus2,
        [EnumMember(Value = "westcentralus")]
        Westcentralus,
        [EnumMember(Value = "westeurope")]
        Westeurope,
        [EnumMember(Value = "northeurope")]
        Northeurope
    }
}

