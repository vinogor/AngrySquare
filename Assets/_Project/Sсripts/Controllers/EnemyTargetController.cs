using _Project.Sсripts.Domain;
using _Project.Sсripts.Services;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Controllers
{
    public class EnemyTargetController
    {
        private readonly CellsManager _cellsManager;
        
        private readonly EnemyAim[] _enemyAims;
        private Cell[] _targetCells;

        private bool _isTripleAim = false;

        public EnemyTargetController(CellsManager cellsManager, EnemyAim[] enemyAims)
        {
            Assert.IsNotNull(cellsManager);
            Assert.AreEqual(3, enemyAims.Length);

            _cellsManager = cellsManager;
            _enemyAims = enemyAims;
        }

        public void SetAimToNewRandomTargetCell()
        {
            _targetCells = _cellsManager.GetRandomTreeInRow();

            EnemyAim leftEnemyAim = _enemyAims[0];
            EnemyAim centerEnemyAim = _enemyAims[1];
            EnemyAim rightEnemyAim = _enemyAims[2];

            centerEnemyAim.transform.position = _targetCells[1].Center() + Vector3.up * 0.03f;

            if (_isTripleAim)
            {
                leftEnemyAim.transform.position = _targetCells[0].Center() + Vector3.up * 0.03f;
                rightEnemyAim.transform.position = _targetCells[2].Center() + Vector3.up * 0.03f;
            }
            else
            {
                leftEnemyAim.transform.position = _targetCells[1].Center() + Vector3.up * 0.03f;
                rightEnemyAim.transform.position = _targetCells[1].Center() + Vector3.up * 0.03f;
            }
        }

        public void NextTurnTripleTarget()
        {
            _isTripleAim = true;
        }
        
        public void NextTurnOneTarget()
        {
            _isTripleAim = false;
        }

        public Cell[] GetCurrentTargetCells()
        {
            Cell[] currentTargetCells = _isTripleAim ? _targetCells : new[] { _targetCells[1] };
            return currentTargetCells;
        }
    }
}