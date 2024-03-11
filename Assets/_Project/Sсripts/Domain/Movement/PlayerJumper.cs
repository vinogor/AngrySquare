using Config;
using Controllers.Sound;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

namespace Domain.Movement
{
    public class PlayerJumper
    {
        private readonly Transform _playerTransform;
        private readonly Transform _enemyTransform;
        private readonly Coefficients _coefficients;
        private readonly GameSoundsPresenter _gameSoundsPresenter;

        private Vector3 _playerCellPosition;
        private Sequence _currentSequence;

        public PlayerJumper(Transform playerTransform, Transform enemyTransform, Coefficients coefficients,
            GameSoundsPresenter gameSoundsPresenter)
        {
            Assert.IsNotNull(playerTransform);
            Assert.IsNotNull(enemyTransform);
            Assert.IsNotNull(coefficients);
            Assert.IsNotNull(gameSoundsPresenter);

            _playerTransform = playerTransform;
            _enemyTransform = enemyTransform;
            _coefficients = coefficients;
            _gameSoundsPresenter = gameSoundsPresenter;
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

            _currentSequence = DOTween.Sequence()
            .AppendCallback(() => _gameSoundsPresenter.PlayTeleport())
            .Append(_playerTransform.DOMoveY(_playerTransform.position.y - lossyScaleY * offset, jumpDuration))
            .Append(_playerTransform.DOMove(cellCenter - Vector3.up * (lossyScaleY * offset), jumpDuration))
            .AppendCallback(() => _gameSoundsPresenter.PlayTeleport())
            .Append(_playerTransform.DOMove(cellCenter + Vector3.up * lossyScaleY, jumpDuration));
            
            return _currentSequence;
        }

        public void ForceStop()
        {
            _currentSequence.Kill();
        }

        private Sequence Jump(Transform transform, Vector3 target, bool instantly = false)
        {
            Debug.Log($"Player - Jump - from {transform.position} - to {target}");
            float epsilonTime = 0.001f;
            int jumpsAmount = 1;
            float duration = instantly ? epsilonTime : _coefficients.JumpDuration;
            _currentSequence = transform
                .DOJump(target, _coefficients.JumpPower, jumpsAmount, duration)
                .AppendCallback(() => _gameSoundsPresenter.PlayPlayerStep())
                .SetEase(Ease.Linear);
            return _currentSequence;
        }
    }
}