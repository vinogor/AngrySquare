using System;
using _Project.Sсripts.Utility;
using UnityEngine;

namespace _Project.Sсripts.Scriptable
{
    [CreateAssetMenu(fileName = "SpellsSettings", menuName = "Gameplay/SpellsSettings")]
    public class SpellsSettings : ScriptableObject
    {
        [field: SerializeField] public SpellInfo[] SpellInfos { get; private set; }

        private void Awake()
        {
            Validator.ValidateAmount(SpellInfos, 5);
        }

        public Sprite GetSprite(SpellName spellName)
        {
            return Array.Find(SpellInfos, spellInfo => spellInfo.SpellName == spellName).Sprite;
        }

        public int GetManaCost(SpellName spellName)
        {
            return Array.Find(SpellInfos, spellInfo => spellInfo.SpellName == spellName).CostMana;
        }
    }
}