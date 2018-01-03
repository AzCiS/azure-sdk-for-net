
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
    /// Defines values for AccountType.
    /// </summary>
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum AccountType
    {
        [EnumMember(Value = "UnknownType")]
        UnknownType,
        [EnumMember(Value = "GeneralPurposeStorage")]
        GeneralPurposeStorage,
        [EnumMember(Value = "BlobStorage")]
        BlobStorage
    }
}

