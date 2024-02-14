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
        private Transform _enemyTransform;
        private PlayerMovement _playerMovement;
        private BaseSettings _baseSettings;
        private EnemyTargetController _enemyTargetController;

        private Vector3 _baseEnemyPosition;

        public EnemyJumper(Transform enemyTransform, PlayerMovement playerMovement, BaseSettings baseSettings,
            EnemyTargetController enemyTargetController)
        {
            _enemyTransform = enemyTransform;
            _playerMovement = playerMovement;
            _baseSettings = baseSettings;
            _enemyTargetController = enemyTargetController;
        }

        public void EnemyJumpToTargetCell(Action onJumpComplete)
        {
            Debug.Log("Enemy - JumpToCell");

            _baseEnemyPosition = _enemyTransform.position;

            Cell targetCell = _enemyTargetController.GetCurrentTargetCell();

            Jump(_enemyTransform, targetCell.Center() + Vector3.up * _baseSettings.EnemyHeight, onJumpComplete);
        }

        public void EnemyJumpOnPlayer(Action onJumpComplete)
        {
            Debug.Log("Enemy - JumpOnPlayer");

            Jump(_enemyTransform, _playerMovement.PlayerStayCell.Center() + Vector3.up * _baseSettings.EnemyHeight,
                onJumpComplete);
        }

        public void EnemyJumpBackToBase(Action onJumpComplete)
        {
            Debug.Log("Enemy - JumpToBase");

            Jump(_enemyTransform, _baseEnemyPosition, onJumpComplete);
        }

        public void EnemyJumpInPlace(Action onJumpComplete)
        {
            Jump(_enemyTransform, _enemyTransform.position, onJumpComplete);
        }

        private void Jump(Transform transform, Vector3 target, Action onJumpComplete)
        {
            transform
                .DOJump(target, _baseSettings.JumpPower, 1, _baseSettings.JumpDuration)
                .SetEase(Ease.Linear)
                .OnComplete(onJumpComplete.Invoke);
        }
    }
}