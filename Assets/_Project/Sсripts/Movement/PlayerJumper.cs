using System;
using _Project.Sсripts.Model;
using _Project.Sсripts.Scriptable;
using DG.Tweening;
using UnityEngine;

namespace _Project.Sсripts.Movement
{
    public class PlayerJumper
    {
        private Transform _playerTransform;
        private Transform _enemyTransform;
        private BaseSettings _baseSettings;
        
        private Vector3 _playerCellPosition;

        public PlayerJumper(Transform playerTransform, Transform enemyTransform, BaseSettings baseSettings)
        {
            _playerTransform = playerTransform;
            _enemyTransform = enemyTransform;
            _baseSettings = baseSettings;
        }

        public void PlayerJumpOnEnemy(Action onJumpComplete)
        {
            Debug.Log("Player - JumpOnEnemy");

            _playerCellPosition = _playerTransform.position;
            Jump(_playerTransform, _enemyTransform.position, onJumpComplete);
        }

        public void PlayerJumpBackToCell(Action onJumpComplete)
        {
            Debug.Log("Player - JumpBackToCell");

            Jump(_playerTransform, _playerCellPosition, onJumpComplete);
        }
        
        public void JumpToNextCell(Cell nextCell, Action onJumpComplete)
        {
            _playerTransform
                .DOJump(nextCell.Center() + Vector3.up * _playerTransform.lossyScale.y,
                    _baseSettings.JumpPower, 1, _baseSettings.JumpDuration)
                .SetEase(Ease.Linear)
                .OnComplete(onJumpComplete.Invoke);
        }

        public void PlayerJumpInPlace(Action onJumpComplete)
        {
            Jump(_playerTransform, _playerTransform.position, onJumpComplete);
        }

        // TODO: общий метод
        public void Jump(Transform transform, Vector3 target, Action onJumpComplete)
        {
            transform
                .DOJump(target, _baseSettings.JumpPower, 1, _baseSettings.JumpDuration)
                .SetEase(Ease.Linear)
                .OnComplete(onJumpComplete.Invoke);
        }
    }
}