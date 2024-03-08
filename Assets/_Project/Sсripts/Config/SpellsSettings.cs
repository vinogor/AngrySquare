using System;
using Domain.Spells;
using UnityEngine;
using UnityEngine.Assertions;

namespace Config
{
    [CreateAssetMenu(fileName = "SpellsSettings", menuName = "Gameplay/SpellsSettings")]
    public class SpellsSettings : ScriptableObject
    {
        private const int ExpectedSpellAmount = 5;
        [field: SerializeField] public SpellInfo[] SpellInfos { get; private set; }

        private void Awake()
        {
            Assert.AreEqual(ExpectedSpellAmount, SpellInfos.Length);
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