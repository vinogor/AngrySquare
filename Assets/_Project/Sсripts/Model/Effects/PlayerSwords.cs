using System;
using DG.Tweening;
using _Project.Sсripts.Dmg;
using _Project.Sсripts.Hp;
using _Project.Sсripts.Scriptable;
using UnityEngine;

namespace _Project.Sсripts.Model.Effects
{
    public class Swords : Effect
    {
        private Health _enemyHealth;
        private Damage _playerDamage;

        private Transform _playerTransform;
        private BaseSettings _baseSettings;

        public Swords(Health enemyHealth, Damage playerDamage, Transform playerTransform, Vector3 enemyPosition, BaseSettings baseSettings)
        {
            _enemyHealth = enemyHealth ?? throw new NullReferenceException("enemyHealth cant be null");
            _playerDamage = playerDamage ?? throw new NullReferenceException("playerDamage cant be null");
            _playerTransform = playerTransform;
            if (playerTransform == null)
                throw new NullReferenceException("playerTransform cant be null");
            _enemyPosition = enemyPosition;
            _baseSettings = baseSettings;
        }

        private Vector3 _enemyPosition;

        public override void Activate()
        {
            Debug.Log("Effect - Swords - Activate");

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
                        .SetEase(Ease.Linear);
                });
        }
    }
}