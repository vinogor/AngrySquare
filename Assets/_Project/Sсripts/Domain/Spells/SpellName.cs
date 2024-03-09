using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Domain.Spells
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SpellName
    {
        NotSet,
        UpDamage,
        UpDefence,
        UpMaxHealth,
        FullHealth,
        UpMaxMana
    }
}