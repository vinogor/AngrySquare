using System;
using System.Collections.Generic;
using _Project.Sсripts.Controllers;
using _Project.Sсripts.Domain.Effects;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Domain.Movement
{
    public class EnemyMovement : MonoBehaviour
    {
        private Dictionary<EffectName, Effect> _enemyEffects;
        private EnemyTargetController _enemyTargetController;
        private EnemyJumper _enemyJumper;
        private Health _playerHealth;
        private Damage _enemyDamage;

        public void Initialize(Dictionary<EffectName, Effect> enemyEffects,
            EnemyTargetController enemyTargetController, EnemyJumper enemyJumper,
            Health playerHealth, Damage enemyDamage)
        {
            Assert.IsNotNull(enemyEffects);
            Assert.IsNotNull(enemyTargetController);
            Assert.IsNotNull(enemyJumper);
            Assert.IsNotNull(playerHealth);
            Assert.IsNotNull(enemyDamage);

            _enemyEffects = enemyEffects;
            _enemyTargetController = enemyTargetController;
            _enemyJumper = enemyJumper;
            _playerHealth = playerHealth;
            _enemyDamage = enemyDamage;
        }

        public event Action TurnCompleted;

        public void Move()
        {
            List<Cell> targetCells = _enemyTargetController.GetCurrentTargetCells();

            if (targetCells.Count == 3)
            {
                _enemyJumper.JumpToTargetThreeInRowCells(targetCells,
                        () => _playerHealth.TakeTripleDamage(_enemyDamage.Value))
                    .AppendCallback(() =>
                    {
                        _enemyTargetController.NextTurnOneTarget();
                        _enemyTargetController.SetAimToNewRandomTargetCell();
                        TurnCompleted?.Invoke();
                    })
                    .Play();
            }
            else
            {
                EffectName effectName = targetCells[0].EffectName;
                _enemyEffects[effectName].Activate(() =>
                {
                    _enemyTargetController.SetAimToNewRandomTargetCell();
                    TurnCompleted?.Invoke();
                });
            }
        }
    }
}