using System;
using System.Collections.Generic;
using _Project.Sсripts.Controllers;
using _Project.Sсripts.Domain.Effects;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Domain.Movement{
    public class EnemyMovement : MonoBehaviour
    {
        private Dictionary<EffectName, Effect> _enemyEffects;
        private EnemyTargetController _enemyTargetController;
        private EnemyJumper _enemyJumper;
        private PlayerMovement _playerMovement;
        private Health _playerHealth;
        private Damage _enemyDamage;

        public void Initialize(Dictionary<EffectName, Effect> enemyEffects,
            EnemyTargetController enemyTargetController, EnemyJumper enemyJumper, PlayerMovement playerMovement,
            Health playerHealth, Damage enemyDamage)
        {
            Assert.IsNotNull(enemyEffects);
            Assert.IsNotNull(enemyTargetController);
            Assert.IsNotNull(enemyJumper);
            Assert.IsNotNull(playerMovement);
            Assert.IsNotNull(playerHealth);
            Assert.IsNotNull(enemyDamage);

            _enemyEffects = enemyEffects;
            _enemyTargetController = enemyTargetController;
            _enemyJumper = enemyJumper;
            _playerMovement = playerMovement;
            _playerHealth = playerHealth;
            _enemyDamage = enemyDamage;
        }

        public event Action TurnCompleted;

        public void Move()
        {
            Cell currentTargetCell = _enemyTargetController.GetCurrentTargetCell();

            if (currentTargetCell == _playerMovement.PlayerStayCell)
            {
                _enemyJumper.ForcedAttack(() => _playerHealth.TakeDamage(_enemyDamage.Value),
                    () => TurnCompleted?.Invoke());
            }
            else
            {
                EffectName effectName = currentTargetCell.EffectName;
                _enemyEffects[effectName].Activate(() =>
                {
                    _enemyTargetController.SetAimToNewRandomTargetCell();
                    TurnCompleted?.Invoke();
                });
            }
        }
    }
}