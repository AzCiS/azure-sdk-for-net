
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
    /// Defines values for CountryCode.
    /// </summary>
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
    public enum CountryCode
    {
        [EnumMember(Value = "US")]
        US,
        [EnumMember(Value = "NL")]
        NL,
        [EnumMember(Value = "IE")]
        IE,
        [EnumMember(Value = "AT")]
        AT,
        [EnumMember(Value = "IT")]
        IT,
        [EnumMember(Value = "BE")]
        BE,
        [EnumMember(Value = "LV")]
        LV,
        [EnumMember(Value = "BG")]
        BG,
        [EnumMember(Value = "LT")]
        LT,
        [EnumMember(Value = "HR")]
        HR,
        [EnumMember(Value = "LU")]
        LU,
        [EnumMember(Value = "CY")]
        CY,
        [EnumMember(Value = "MT")]
        MT,
        [EnumMember(Value = "CZ")]
        CZ,
        [EnumMember(Value = "DK")]
        DK,
        [EnumMember(Value = "PL")]
        PL,
        [EnumMember(Value = "EE")]
        EE,
        [EnumMember(Value = "PT")]
        PT,
        [EnumMember(Value = "FI")]
        FI,
        [EnumMember(Value = "RO")]
        RO,
        [EnumMember(Value = "FR")]
        FR,
        [EnumMember(Value = "SK")]
        SK,
        [EnumMember(Value = "DE")]
        DE,
        [EnumMember(Value = "SI")]
        SI,
        [EnumMember(Value = "GR")]
        GR,
        [EnumMember(Value = "ES")]
        ES,
        [EnumMember(Value = "HU")]
        HU,
        [EnumMember(Value = "SE")]
        SE,
        [EnumMember(Value = "GB")]
        GB
    }
}

