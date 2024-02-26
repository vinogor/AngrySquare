using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Sсripts.Config;
using _Project.Sсripts.Domain;
using _Project.Sсripts.Domain.Effects;
using _Project.Sсripts.Services.Utility;
using UnityEngine;

namespace _Project.Sсripts.Services
{
    public class CellsManager
    {
        private readonly Cell[] _cells;
        private readonly CellsSettings _cellsSettings;

        public CellsManager(Cell[] cells, CellsSettings cellsSettings)
        {
            _cells = cells;
            _cellsSettings = cellsSettings;
        }

        public int Length => _cells.Length;

        public void FillWithEffects()
        {
            Array.ForEach(_cellsSettings.CellInfos, Fill);
        }

        public Cell[] Find(EffectName effectName)
        {
            return _cells.Where(cell => cell.EffectName == effectName).ToArray();
        }

        public List<Cell> GetRandomInRow(bool isTripleAim)
        {
            Cell center = _cells.Shuffle().First();

            if (isTripleAim == false)
                return new() { center };

            int centerIndex = Index(center);
            int leftIndex = (centerIndex - 1) % _cells.Length;
            int rightIndex = (centerIndex + 1) % _cells.Length;

            return new() { _cells[leftIndex], center, _cells[rightIndex] };
        }

        public Cell Get(int index)
        {
            return _cells[index];
        }

        public int Index(Cell cell)
        {
            return Array.IndexOf(_cells, cell);
        }

        public void CleanAll()
        {
            Array.ForEach(_cells, Clean);
        }

        private void Clean(Cell cell)
        {
            cell.SetEffectName(EffectName.NotSet);
        }

        private void Fill(CellInfo cellInfo)
        {
            EffectName effectName = cellInfo.EffectName;
            Sprite sprite = cellInfo.Sprite;
            int amount = cellInfo.Amount;

            var cellsWithoutEffect = CellsWithoutEffect(_cells);

            cellsWithoutEffect
                .Shuffle()
                .Take(amount)
                .ToList()
                .ForEach(cell =>
                {
                    cell.SetEffectName(effectName);
                    cell.SetSprite(sprite);
                });
        }

        private Cell[] CellsWithoutEffect(Cell[] cells)
        {
            return cells.Where(cell => cell.IsEffectSet() == false).ToArray();
        }
    }
}