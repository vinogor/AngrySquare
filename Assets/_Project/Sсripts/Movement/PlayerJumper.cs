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
        private Coefficients _coefficients;

        private Vector3 _playerCellPosition;

        public PlayerJumper(Transform playerTransform, Transform enemyTransform, Coefficients coefficients)
        {
            _playerTransform = playerTransform;
            _enemyTransform = enemyTransform;
            _coefficients = coefficients;
        }

        public Sequence PlayerJumpOnEnemy()
        {
            Debug.Log("Player - JumpOnEnemy");

            _playerCellPosition = _playerTransform.position;
            return Jump(_playerTransform, _enemyTransform.position);
        }

        public Sequence PlayerJumpBackToCell()
        {
            Debug.Log("Player - JumpBackToCell");

            return Jump(_playerTransform, _playerCellPosition);
        }

        public Sequence JumpToNextCell(Cell nextCell)
        {
            return Jump(_playerTransform, nextCell.Center() + Vector3.up * _playerTransform.lossyScale.y);
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
            float jumpDuration = _coefficients.JumpDuration;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(_playerTransform.DOMoveY(_playerTransform.position.y - lossyScaleY * offset, jumpDuration));
            sequence.Append(_playerTransform.DOMove(cellCenter - Vector3.up * (lossyScaleY * offset), jumpDuration));
            sequence.Append(_playerTransform.DOMove(cellCenter + Vector3.up * lossyScaleY, jumpDuration));
            return sequence;
        }

        private Sequence Jump(Transform transform, Vector3 target)
        {
            Debug.Log($"Player - Jump - from {transform.position} - to {target}");
            return transform
                .DOJump(target, _coefficients.JumpPower, 1, _coefficients.JumpDuration)
                .SetEase(Ease.Linear);
        }
    }
}