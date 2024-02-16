using System;
using UnityEngine;

namespace _Project.Sсripts.Scriptable
{
    [Serializable]
    public class SpellInfo
    {
        [field: SerializeField] public SpellName SpellName { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        
        // TODO: наверное лучше перенести в Coefficients
        [field: SerializeField] public int CostMana { get; private set; }
    }

    public enum SpellName
    {
        NotSet,
        UpDamage,
        UpDefence,
        UpMaxHealth,
        FullHealth
    }
}