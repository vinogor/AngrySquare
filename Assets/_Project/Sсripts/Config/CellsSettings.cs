using System;
using Domain.Effects;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;

namespace Config
{
    [CreateAssetMenu(fileName = "CellsSettings", menuName = "Gameplay/CellsSettings")]
    public class CellsSettings : ScriptableObject
    {
        private const int ExpectedCellTotalAmount = 16;
        private const int ExpectedAmountOfCellVarieties = 6;
        [field: SerializeField] public CellInfo[] CellInfos { get; private set; }

        private void Awake()
        {
            
            Assert.AreEqual(ExpectedAmountOfCellVarieties, CellInfos.Length);
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
            Assert.AreEqual(ExpectedCellTotalAmount, counter,
                $"Wrong amount, expected {ExpectedCellTotalAmount} cells, actual {counter}");
            Assert.AreNotEqual(ExpectedCellTotalAmount, counter, $"Everything OK!");
        }
    }
}