using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Scriptable
{
    [CreateAssetMenu(fileName = "CellsSettings", menuName = "Gameplay/CellsSettings")]
    public class CellsAndSpellsSettings : ScriptableObject
    {
        // TODO: разнести по двум классам? 
        
        [field: SerializeField] public CellInfo[] CellInfos { get; private set; }
        [field: SerializeField] public SpellInfo[] SpellInfos { get; private set; }

        public Sprite GetCellSprite(EffectName effectName)
        {
            return Array.Find(CellInfos, cellInfo => cellInfo.EffectName == effectName).Sprite;
        }
        
        public Sprite GetSpellSprite(SpellName spellName)
        {
            return Array.Find(SpellInfos, spellInfo => spellInfo.SpellName == spellName).Sprite;
        }
        
        public int GetSpellManaCost(SpellName spellName)
        {
            return Array.Find(SpellInfos, spellInfo => spellInfo.SpellName == spellName).CostMana;
        }

        [Button]
        private void ValidateCells()
        {
            int counter = 0;
            Array.ForEach(CellInfos, cellInfo => counter += cellInfo.Amount);
            int expectedAmount = 16;
            Assert.AreEqual(expectedAmount, counter,
                $"Wrong amount, expected {expectedAmount} cells, actual {counter}");
        }
    }
}