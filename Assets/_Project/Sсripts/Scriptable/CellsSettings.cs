using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Scriptable
{
    [CreateAssetMenu(fileName = "CellsSettings", menuName = "Gameplay/CellsSettings")]
    public class CellsSettings : ScriptableObject
    {
        [field: SerializeField] public CellInfo[] CellInfos { get; private set; }

        private void Awake()
        {
            Assert.AreEqual(6, CellInfos.Length);
        }

        public Sprite GetCellSprite(EffectName effectName)
        {
            return Array.Find(CellInfos, cellInfo => cellInfo.EffectName == effectName).Sprite;
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