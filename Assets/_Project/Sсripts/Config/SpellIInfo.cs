using System;
using Domain.Spells;
using UnityEngine;

namespace Config
{
    [Serializable]
    public class SpellInfo
    {
        [field: SerializeField] public SpellName SpellName { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public int CostMana { get; private set; }
    }
}