using System.Linq;
using _Project.Sсripts.Domain;
using _Project.Sсripts.Services.Utility;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Controllers
{
    public class EnemyTargetController
    {
        private readonly Cell[] _cells;
        private readonly EnemyAim _enemyAim;
        private Cell _targetCell;

        public EnemyTargetController(Cell[] cells, EnemyAim enemyAim)
        {
            Assert.IsNotNull(cells);
            Assert.IsNotNull(enemyAim);

            _cells = cells;
            _enemyAim = enemyAim;
        }

        public void SetAimToNewRandomTargetCell()
        {
            _targetCell = _cells.Shuffle().First();
            _enemyAim.transform.position = _targetCell.Center() + Vector3.up * 0.03f;
        }

        public Cell GetCurrentTargetCell()
        {
            return _targetCell;
        }
    }
}