using System;
using System.Collections.Generic;
using Config;
using Controllers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

namespace Domain.Movement
{
    public class EnemyJumper
    {
        private readonly Transform _enemyTransform;
        private readonly PlayerMovement _playerMovement;
        private readonly Coefficients _coefficients;
        private readonly EnemyTargetController _enemyTargetController;

        private Vector3 _baseEnemyPosition;

        public EnemyJumper(Transform enemyTransform, PlayerMovement playerMovement, Coefficients coefficients,
            EnemyTargetController enemyTargetController)
        {
            Assert.IsNotNull(enemyTransform);
            Assert.IsNotNull(playerMovement);
            Assert.IsNotNull(coefficients);
            Assert.IsNotNull(enemyTargetController);

            _enemyTransform = enemyTransform;
            _playerMovement = playerMovement;
            _coefficients = coefficients;
            _enemyTargetController = enemyTargetController;
        }

        public event Action MadeStep;

        public Sequence JumpToTargetThreeInRowCells(List<Cell> targetCells, Action attackAction)
        {
            Debug.Log("Enemy - JumpToTargetThreeInRowCells");
            _baseEnemyPosition = _enemyTransform.position;
            Sequence sequence = DOTween.Sequence();

            foreach (Cell targetCell in targetCells)
            {
                sequence.Append(Jump(_enemyTransform, targetCell.Center() + Vector3.up * _coefficients.EnemyHeight));
                sequence.AppendCallback(() =>
                {
                    if (targetCell == _playerMovement.PlayerStayCell)
                        attackAction.Invoke();
                });
            }

            sequence.Append(JumpBackToBase());
            return sequence;
        }

        public Sequence JumpToAttackPlayer(Cell targetCell, Action attackAction)
        {
            Debug.Log("Enemy - JumpToAttackPlayer");
            _baseEnemyPosition = _enemyTransform.position;
            Sequence sequence = DOTween.Sequence();
            sequence.Append(Jump(_enemyTransform, targetCell.Center() + Vector3.up * _coefficients.EnemyHeight));
            sequence.AppendCallback(attackAction.Invoke);
            sequence.Append(JumpBackToBase());

            return sequence;
        }

        public Sequence JumpToTargetCell()
        {
            Debug.Log("Enemy - JumpToCell");
            _baseEnemyPosition = _enemyTransform.position;
            Cell targetCell = _enemyTargetController.GetCurrentTargetCells()[0];
            return Jump(_enemyTransform, targetCell.Center() + Vector3.up * _coefficients.EnemyHeight);
        }

        public Sequence JumpOnPlayer()
        {
            Debug.Log("Enemy - JumpOnPlayer");
            return Jump(_enemyTransform,
                _playerMovement.PlayerStayCell.Center() + Vector3.up * _coefficients.EnemyHeight);
        }

        public Sequence JumpBackToBase()
        {
            Debug.Log("Enemy - JumpToBase");
            return Jump(_enemyTransform, _baseEnemyPosition);
        }

        private Sequence Jump(Transform transform, Vector3 target)
        {
            Debug.Log($"Enemy - Jump - from {transform.position} - to {target}");
            return DOTween.Sequence()
                .AppendCallback(() => MadeStep?.Invoke())
                .Join(transform.DOJump(target, _coefficients.JumpPower, 1, _coefficients.JumpDuration))
                .SetEase(Ease.Linear);
        }
    }
}