using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace _Project.Sсripts.Domain.Effects
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EffectName
    {
        NotSet,
        Swords,
        SpellBook,
        Portal,
        Question,
        Mana,
        Health
    }
}