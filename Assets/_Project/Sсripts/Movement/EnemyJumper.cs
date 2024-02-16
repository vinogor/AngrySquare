using System;
using _Project.Sсripts.Animation;
using _Project.Sсripts.Model;
using _Project.Sсripts.Scriptable;
using DG.Tweening;
using UnityEngine;

namespace _Project.Sсripts.Movement
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
            _enemyTransform = enemyTransform;
            _playerMovement = playerMovement;
            _coefficients = coefficients;
            _enemyTargetController = enemyTargetController;
        }

        public Sequence EnemyJumpToTargetCell()
        {
            Debug.Log("Enemy - JumpToCell");

            _baseEnemyPosition = _enemyTransform.position;

            Cell targetCell = _enemyTargetController.GetCurrentTargetCell();

            return Jump(_enemyTransform, targetCell.Center() + Vector3.up * _coefficients.EnemyHeight);
        }

        public Sequence EnemyJumpOnPlayer()
        {
            Debug.Log("Enemy - JumpOnPlayer");

            return Jump(_enemyTransform,
                _playerMovement.PlayerStayCell.Center() + Vector3.up * _coefficients.EnemyHeight);
        }

        public Sequence EnemyJumpBackToBase()
        {
            Debug.Log("Enemy - JumpToBase");

            return Jump(_enemyTransform, _baseEnemyPosition);
        }

        private Sequence Jump(Transform transform, Vector3 target)
        {
            Debug.Log($"Enemy - Jump - from {transform.position} - to {target}");
            return transform
                .DOJump(target, _coefficients.JumpPower, 1, _coefficients.JumpDuration)
                .SetEase(Ease.Linear);
        }

        public void ForcedAttack(Action everyAttackAction, Action onJumpComplete)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(EnemyJumpToTargetCell());
            sequence.AppendCallback(everyAttackAction.Invoke);
            sequence.Append(EnemyJumpLooped(2, everyAttackAction.Invoke));
            sequence.Append(EnemyJumpBackToBase());
            sequence.AppendCallback(onJumpComplete.Invoke);
            sequence.Play();
        }

        public Sequence EnemyJumpLooped(int amount, Action onEveryLoop)
        {
            Cell targetCell = _enemyTargetController.GetCurrentTargetCell();

            return _enemyTransform
                .DOJump(targetCell.Center() + Vector3.up * _coefficients.EnemyHeight, _coefficients.JumpPower, 1,
                    _coefficients.JumpDuration)
                .SetEase(Ease.Linear)
                .SetLoops(amount)
                .OnStepComplete(onEveryLoop.Invoke);
        }
    }
}