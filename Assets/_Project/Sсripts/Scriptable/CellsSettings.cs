using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Scriptable
{
    [CreateAssetMenu(fileName = "CellsSettings", menuName = "Gameplay/CellsSettings")]
    public class CellsSettings : ScriptableObject
    {
        [field: SerializeField] public List<CellInfo> CellInfos { get; private set; }

        [Button]
        private void Validate()
        {
            int counter = 0;
            CellInfos.ForEach(cellInfo => counter += cellInfo.Amount);
            int expectedAmount = 16;
            Assert.AreEqual(expectedAmount, counter,
                $"Wrong amount, expected {expectedAmount} cells, actual {counter}");
        }
    }
}