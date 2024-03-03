using _Project.Config;
using _Project.Controllers.Sound;
using DG.Tweening;
using UnityEngine;

namespace _Project.Domain.Movement
{
    public class PlayerJumper
    {
        private readonly Transform _playerTransform;
        private readonly Transform _enemyTransform;
        private readonly Coefficients _coefficients;
        private readonly GameSounds _gameSounds; 

        private Vector3 _playerCellPosition;

        public PlayerJumper(Transform playerTransform, Transform enemyTransform, Coefficients coefficients, GameSounds gameSounds)
        {
            _playerTransform = playerTransform;
            _enemyTransform = enemyTransform;
            _coefficients = coefficients;
            _gameSounds = gameSounds;
        }

        public Sequence JumpOnEnemy()
        {
            Debug.Log("Player - JumpOnEnemy");

            _playerCellPosition = _playerTransform.position;
            return Jump(_playerTransform, _enemyTransform.position);
        }

        public Sequence JumpBackToCell()
        {
            Debug.Log("Player - JumpBackToCell");

            return Jump(_playerTransform, _playerCellPosition);
        }

        public Sequence JumpToNextCell(Cell nextCell, bool instantly = false)
        {
            return Jump(_playerTransform, nextCell.Center() + Vector3.up * _playerTransform.lossyScale.y, instantly);
        }

        public Sequence JumpInPlace()
        {
            return Jump(_playerTransform, _playerTransform.position);
        }

        public Sequence Teleport(Cell targetCell)
        {
            float offset = 2.1f;
            float lossyScaleY = _playerTransform.lossyScale.y;
            Vector3 cellCenter = targetCell.Center();
            float jumpDuration = _coefficients.JumpDuration;

            Sequence sequence = DOTween.Sequence();
            sequence.AppendCallback(_gameSounds.PlayTeleport);
            sequence.Append(_playerTransform.DOMoveY(_playerTransform.position.y - lossyScaleY * offset, jumpDuration));
            sequence.Append(_playerTransform.DOMove(cellCenter - Vector3.up * (lossyScaleY * offset), jumpDuration));
            sequence.AppendCallback(_gameSounds.PlayTeleport);
            sequence.Append(_playerTransform.DOMove(cellCenter + Vector3.up * lossyScaleY, jumpDuration));
            return sequence;
        }

        private Sequence Jump(Transform transform, Vector3 target, bool instantly = false)
        {
            Debug.Log($"Player - Jump - from {transform.position} - to {target}");
            float epsilonTime = 0.001f;
            float duration = instantly ? epsilonTime : _coefficients.JumpDuration;
            return transform
                .DOJump(target, _coefficients.JumpPower, 1, duration)
                .AppendCallback(_gameSounds.PlayPlayerStep)
                .SetEase(Ease.Linear);
        }
    }
}