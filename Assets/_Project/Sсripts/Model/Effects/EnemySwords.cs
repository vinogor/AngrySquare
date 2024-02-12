using System;
using _Project.Sсripts.Dmg;
using _Project.Sсripts.Hp;
using _Project.Sсripts.Movement;
using _Project.Sсripts.Scriptable;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Model.Effects
{
    public class EnemySwords : Effect
    {
        private Transform _enemyTransform;
        private Cell _targetCell;
        private Health _playerHealth;
        private Damage _enemyDamage;
        private PlayerMovement _playerMovement;
        private BaseSettings _baseSettings;

        public EnemySwords(Transform enemyTransform, Cell targetCell, Health playerHealth,
            Damage enemyDamage, PlayerMovement playerMovement, BaseSettings baseSettings)
        {
            Assert.IsNotNull(enemyTransform);
            Assert.IsNotNull(targetCell);
            Assert.IsNotNull(playerHealth);
            Assert.IsNotNull(enemyDamage);
            Assert.IsNotNull(playerMovement);
            Assert.IsNotNull(baseSettings);

            _enemyTransform = enemyTransform;
            _targetCell = targetCell;
            _playerHealth = playerHealth;
            _enemyDamage = enemyDamage;
            _playerMovement = playerMovement;
            _baseSettings = baseSettings;
        }

        public override void Activate(Action onComplete)
        {
            base.Activate(null);

            Vector3 startEnemyPosition = _enemyTransform.position;

            _enemyTransform
                .DOJump(_targetCell.Center() + Vector3.up * _baseSettings.EnemyHeight,
                    _baseSettings.JumpPower, 1, _baseSettings.JumpDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    int enemyDamage = _enemyDamage.Value;

                    if (_targetCell == _playerMovement.PlayerStayCell)
                    {
                        // TODO: тройной урон - как то выделить анимацией по особенному
                        _playerHealth.TakeDamage(enemyDamage * 3);
                        JumpToBase(onComplete, startEnemyPosition);
                    }
                    else
                    {
                        JumpOnPlayer(onComplete, enemyDamage, startEnemyPosition);
                    }
                });
        }

        private void JumpOnPlayer(Action callNextTurn, int enemyDamage, Vector3 startEnemyPosition)
        {
            _enemyTransform
                .DOJump(_playerMovement.PlayerStayCell.Center() + Vector3.up * _baseSettings.EnemyHeight,
                    _baseSettings.JumpPower, 1, _baseSettings.JumpDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    _playerHealth.TakeDamage(enemyDamage);
                    JumpToBase(callNextTurn, startEnemyPosition);
                });
        }

        private void JumpToBase(Action callNextTurn, Vector3 startEnemyPosition)
        {
            _enemyTransform
                .DOJump(startEnemyPosition, _baseSettings.JumpPower, 1, _baseSettings.JumpDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() => { callNextTurn.Invoke(); });
        }
    }
}