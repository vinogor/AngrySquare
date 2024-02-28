using System.Collections.Generic;
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
        private List<Cell> _targetCells;

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
            _targetCells = _cellsManager.GetRandomInRow(_isTripleAim);

            for (var i = 0; i < _targetCells.Count; i++)
            {
                _enemyAims[i].gameObject.SetActive(true);
                _enemyAims[i].transform.position = _targetCells[i].Center() + Vector3.up * 0.03f;
            }

            for (var i = _targetCells.Count; i < _enemyAims.Length; i++)
                _enemyAims[i].gameObject.SetActive(false);
        }

        public void NextTurnTripleTarget()
        {
            _isTripleAim = true;
        }

        public void NextTurnOneTarget()
        {
            _isTripleAim = false;
        }

        public List<Cell> GetCurrentTargetCells()
        {
            return _targetCells;
        }

        public void SetNewTargetCells(List<int> cellsIndexes)
        {
            _targetCells = new List<Cell>();
            cellsIndexes.ForEach(index => _targetCells.Add(_cellsManager.Get(index)));
            
            for (var i = 0; i < _targetCells.Count; i++)
            {
                _enemyAims[i].gameObject.SetActive(true);
                _enemyAims[i].transform.position = _targetCells[i].Center() + Vector3.up * 0.03f;
            }

            for (var i = _targetCells.Count; i < _enemyAims.Length; i++)
                _enemyAims[i].gameObject.SetActive(false);
        }
    }
}