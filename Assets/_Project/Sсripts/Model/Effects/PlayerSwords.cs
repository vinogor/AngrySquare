using System;
using _Project.Sсripts.Dmg;
using _Project.Sсripts.Hp;
using _Project.Sсripts.Scriptable;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Model.Effects
{
    public class PlayerSwords : Effect
    {
        private Health _enemyHealth;
        private Damage _playerDamage;

        private Transform _playerTransform;
        private BaseSettings _baseSettings;
        private Vector3 _enemyPosition;

        public PlayerSwords(Health enemyHealth, Damage playerDamage, Transform playerTransform, Vector3 enemyPosition,
            BaseSettings baseSettings)
        {
            Assert.IsNotNull(enemyHealth);
            Assert.IsNotNull(playerDamage);
            Assert.IsNotNull(playerTransform);
            Assert.IsNotNull(baseSettings);

            _enemyHealth = enemyHealth;
            _playerDamage = playerDamage;
            _playerTransform = playerTransform;
            _enemyPosition = enemyPosition;
            _baseSettings = baseSettings;
        }

        public override void Activate(Action onComplete)
        {
            base.Activate(null);

            Vector3 startPlayerPosition = _playerTransform.position;

            _playerTransform
                .DOJump(_enemyPosition, _baseSettings.JumpPower, 1, _baseSettings.JumpDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    int playerDamage = _playerDamage.Value;
                    _enemyHealth.TakeDamage(playerDamage);
                    _playerTransform
                        .DOJump(startPlayerPosition, _baseSettings.JumpPower, 1, _baseSettings.JumpDuration)
                        .SetEase(Ease.Linear)
                        .OnComplete(onComplete.Invoke);
                });
        }
    }
}