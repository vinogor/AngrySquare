using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Config;
using _Project.Domain;
using _Project.Domain.Effects;
using _Project.Services.Utility;
using UnityEngine;

namespace _Project.Services
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
            Array.ForEach(_cellsSettings.CellInfos, RandomFill);
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
            int leftIndex = (centerIndex - 1 + _cells.Length) % _cells.Length;
            int rightIndex = (centerIndex + 1 + _cells.Length) % _cells.Length;

            return new() { _cells[leftIndex], center, _cells[rightIndex] };
        }

        public Dictionary<int, EffectName> GetCellIndexesWithEffectNames()
        {
            Dictionary<int, EffectName> result = new();

            for (var i = 0; i < _cells.Length; i++)
            {
                result.Add(i, _cells[i].EffectName);
            }

            return result;
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

        public void SetCellsEffects(Dictionary<int, EffectName> cellIndexesWithEffectNames)
        {
            CleanAll();

            for (var i = 0; i < _cells.Length; i++)
            {
                Cell cell = _cells[i];

                Debug.Log($"CellsManager - SetCellsEffects - OLD VALUES: cell index={i}, effectName={cell.EffectName}");

                EffectName effectName = cellIndexesWithEffectNames[i];

                cell.SetEffectName(effectName);
                Sprite cellSprite = _cellsSettings.GetCellSprite(effectName);
                cell.SetSprite(cellSprite);

                Debug.Log(
                    $"CellsManager - SetCellsEffects - NEW VALUES: cell index={i}, effectName={effectName}, cellSprite={cellSprite.name}");
            }
        }

        private void Clean(Cell cell)
        {
            Debug.Log($"CellsManager - Clean - OLD VALUES: cell index={Index(cell)}, effectName={cell.EffectName}");
            cell.SetEffectName(EffectName.NotSet);
            Debug.Log($"CellsManager - Clean - NEW VALUES: cell index={Index(cell)}, effectName={cell.EffectName}");
        }

        private void RandomFill(CellInfo cellInfo)
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
                    Debug.Log(
                        $"CellsManager - RandomFill - OLD VALUES: cell index={Index(cell)}, effectName={cell.EffectName}");
                    cell.SetEffectName(effectName);
                    cell.SetSprite(sprite);
                    Debug.Log(
                        $"CellsManager - RandomFill - NEW VALUES: cell index={Index(cell)}, effectName={effectName}, cellSprite={sprite.name}");
                });
        }

        private Cell[] CellsWithoutEffect(Cell[] cells)
        {
            return cells.Where(cell => cell.IsEffectSet() == false).ToArray();
        }
    }
}