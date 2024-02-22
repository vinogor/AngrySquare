using System;
using _Project.Sсripts.Domain.Spells;
using UnityEngine;

namespace _Project.Sсripts.Config
{
    [Serializable]
    public class SpellInfo
    {
        [field: SerializeField] public SpellName SpellName { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public int CostMana { get; private set; }
    }
}