using System;
using DG.Tweening;
using Sсripts.Dmg;
using Sсripts.Hp;
using UnityEngine;

namespace Sсripts.Model.Effects
{
    public class Swords : Effect
    {
        private Health _enemyHealth;
        private Damage _playerDamage;

        private Transform _playerTransform;
        private Vector3 _enemyPosition;

        public Swords(Health enemyHealth, Damage playerDamage, Transform playerTransform, Vector3 enemyPosition)
        { 
            if (enemyHealth == null)
                throw new NullReferenceException("enemyHealth cant be null");

            if (playerDamage == null)
                throw new NullReferenceException("playerDamage cant be null");

            if (playerTransform == null)
                throw new NullReferenceException("playerTransform cant be null");

            _enemyHealth = enemyHealth;
            _playerDamage = playerDamage;
            _playerTransform = playerTransform;
            _enemyPosition = enemyPosition;
        }

        public override void Activate()
        {
            Debug.Log("Effect - Swords - Activate");

            Vector3 startPlayerPosition = _playerTransform.position;

            _playerTransform
                .DOJump(_enemyPosition, Constants.JumpPower, 1, Constants.JumpDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    int playerDamage = _playerDamage.Value;
                    _enemyHealth.TakeDamage(playerDamage);
                    _playerTransform
                        .DOJump(startPlayerPosition, Constants.JumpPower, 1, Constants.JumpDuration)
                        .SetEase(Ease.Linear);
                });
        }
    }
}