using System.Collections.Generic;
using System.Linq;
using _Project.Sсripts.Model;
using _Project.Sсripts.Utility;
using UnityEngine;

namespace _Project.Sсripts.Animation
{
    public class EnemyAimToCellMover
    {
        private readonly List<Cell> _cells;
        private readonly EnemyAim _enemyAim;

        public EnemyAimToCellMover(List<Cell> cells, EnemyAim enemyAim)
        {
            _cells = cells;
            _enemyAim = enemyAim;
        }

        public Cell SetToNewRandomCell()
        {
            Cell targetCell = _cells.Shuffle().Take(1).ToList()[0];
            _enemyAim.transform.position = targetCell.Center() + Vector3.up * 0.03f;
            return targetCell;
        }
    }
}