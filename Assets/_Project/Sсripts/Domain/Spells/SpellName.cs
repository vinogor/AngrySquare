using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace _Project.Sсripts.Domain.Spells
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