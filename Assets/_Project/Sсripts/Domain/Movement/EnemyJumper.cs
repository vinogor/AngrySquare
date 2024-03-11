using System;
using System.Collections.Generic;
using Config;
using Controllers;
using Controllers.Sound;
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
        private readonly GameSoundsPresenter _gameSoundsPresenter;
        private readonly Vector3 _enemyBasePosition;

        private Sequence _currentSequence;

        public EnemyJumper(Transform enemyTransform, PlayerMovement playerMovement, Coefficients coefficients,
            EnemyTargetController enemyTargetController, GameSoundsPresenter gameSoundsPresenter, Vector3 enemyBasePosition)
        {
            Assert.IsNotNull(enemyTransform);
            Assert.IsNotNull(playerMovement);
            Assert.IsNotNull(coefficients);
            Assert.IsNotNull(enemyTargetController);
            Assert.IsNotNull(gameSoundsPresenter);

            _enemyTransform = enemyTransform;
            _playerMovement = playerMovement;
            _coefficients = coefficients;
            _enemyTargetController = enemyTargetController;
            _gameSoundsPresenter = gameSoundsPresenter;
            _enemyBasePosition = enemyBasePosition;
        }

        public Sequence JumpToTargetThreeInRowCells(List<Cell> targetCells, Action attackAction)
        {
            Debug.Log("Enemy - JumpToTargetThreeInRowCells");
            // _baseEnemyPosition = _enemyTransform.position;
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
            _currentSequence = sequence;
            return _currentSequence;
        }

        public Sequence JumpToAttackPlayer(Cell targetCell, Action attackAction)
        {
            Debug.Log("Enemy - JumpToAttackPlayer");
            // _baseEnemyPosition = _enemyTransform.position;
            _currentSequence = DOTween.Sequence()
                .Append(Jump(_enemyTransform, targetCell.Center() + Vector3.up * _coefficients.EnemyHeight))
                .AppendCallback(attackAction.Invoke)
                .Append(JumpBackToBase());

            return _currentSequence;
        }

        public Sequence JumpToTargetCell()
        {
            Debug.Log("Enemy - JumpToCell");
            // _baseEnemyPosition = _enemyTransform.position;
            Cell targetCell = _enemyTargetController.GetCurrentTargetCells()[0];
            return Jump(_enemyTransform, targetCell.Center() + Vector3.up * _coefficients.EnemyHeight);
        }

        public Sequence JumpOnPlayer()
        {
            Debug.Log("Enemy - JumpOnPlayer");
            return Jump(_enemyTransform,
                _playerMovement.PlayerStayCell.Center() + Vector3.up * _coefficients.EnemyHeight);
        }

        public Sequence JumpBackToBase(bool instantly = false)
        {
            Debug.Log("Enemy - JumpToBase");
            return Jump(_enemyTransform, _enemyBasePosition, instantly);
        }
        
        public void ForceStop()
        {
            _currentSequence.Kill();
        }

        private Sequence Jump(Transform transform, Vector3 target, bool instantly = false)
        {
            Debug.Log($"Enemy - Jump - from {transform.position} - to {target}");
            const int jumpsAmount = 1;
            float epsilonTime = 0.001f;
            float duration = instantly ? epsilonTime : _coefficients.JumpDuration;
            _currentSequence  = DOTween.Sequence()
                .AppendCallback(() => _gameSoundsPresenter.PlayEnemyStep())
                .Join(transform.DOJump(target, _coefficients.JumpPower, jumpsAmount, duration))
                .SetEase(Ease.Linear);
            return _currentSequence;
        }
    }
}