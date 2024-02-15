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
            Jump(_playerTransform, nextCell.Center() + Vector3.up * _playerTransform.lossyScale.y, onJumpComplete);
        }

        public Sequence PlayerJumpInPlace(Action onJumpComplete)
        {
            return Jump(_playerTransform, _playerTransform.position, onJumpComplete);
        }

        public Sequence PlayerJumpInPlace()
        {
            return Jump(_playerTransform, _playerTransform.position);
        }

        public Sequence PlayerTeleport(Cell targetCell)
        {
            float offset = 2.1f;
            float lossyScaleY = _playerTransform.lossyScale.y;
            Vector3 cellCenter = targetCell.Center();
            float jumpDuration = _baseSettings.JumpDuration;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(_playerTransform.DOMoveY(_playerTransform.position.y - lossyScaleY * offset, jumpDuration));
            sequence.Append(_playerTransform.DOMove(cellCenter - Vector3.up * (lossyScaleY * offset), jumpDuration));
            sequence.Append(_playerTransform.DOMove(cellCenter + Vector3.up * lossyScaleY, jumpDuration));
            return sequence;
        }

        // TODO: вынести в абстрактный класс общий метод
        private Sequence Jump(Transform transform, Vector3 target, Action onJumpComplete)
        {
            return transform
                .DOJump(target, _baseSettings.JumpPower, 1, _baseSettings.JumpDuration)
                .SetEase(Ease.Linear)
                .OnComplete(onJumpComplete.Invoke);
        }

        private Sequence Jump(Transform transform, Vector3 target)
        {
            Debug.Log($"Player - Jump - from {transform.position} - to {target}");
            return transform
                .DOJump(target, _baseSettings.JumpPower, 1, _baseSettings.JumpDuration)
                .SetEase(Ease.Linear);
        }
    }
}