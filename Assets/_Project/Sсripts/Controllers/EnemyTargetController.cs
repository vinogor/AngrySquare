using _Project.Sсripts.Domain;
using _Project.Sсripts.Services;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Controllers
{
    public class EnemyTargetController
    {
        private readonly CellsManager _cellsManager;
        private readonly EnemyAim _enemyAim;
        private Cell _targetCell;

        public EnemyTargetController(CellsManager cellsManager, EnemyAim enemyAim)
        {
            Assert.IsNotNull(cellsManager);
            Assert.IsNotNull(enemyAim);

            _cellsManager = cellsManager;
            _enemyAim = enemyAim;
        }

        public void SetAimToNewRandomTargetCell()
        {
            _targetCell = _cellsManager.GetRandom();
            _enemyAim.transform.position = _targetCell.Center() + Vector3.up * 0.03f;
        }

        public Cell GetCurrentTargetCell()
        {
            return _targetCell;
        }
    }
}