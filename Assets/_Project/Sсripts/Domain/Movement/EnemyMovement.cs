using System;
using System.Collections.Generic;
using Controllers;
using DG.Tweening;
using Domain.Effects;
using UnityEngine;
using UnityEngine.Assertions;

namespace Domain.Movement
{
    public class EnemyMovement : MonoBehaviour
    {
        private Dictionary<EffectName, Effect> _enemyEffects;
        private EnemyTargetController _enemyTargetController;
        private PlayerMovement _playerMovement;
        private EnemyJumper _enemyJumper;
        private Health _playerHealth;
        private Damage _enemyDamage;
        private int TripleAimAmount;

        public void Initialize(Dictionary<EffectName, Effect> enemyEffects,
            EnemyTargetController enemyTargetController, EnemyJumper enemyJumper,
            Health playerHealth, Damage enemyDamage, PlayerMovement playerMovement)
        {
            Assert.IsNotNull(enemyEffects);
            Assert.IsNotNull(enemyTargetController);
            Assert.IsNotNull(enemyJumper);
            Assert.IsNotNull(playerHealth);
            Assert.IsNotNull(enemyDamage);
            Assert.IsNotNull(playerMovement);

            _enemyEffects = enemyEffects;
            _enemyTargetController = enemyTargetController;
            _enemyJumper = enemyJumper;
            _playerHealth = playerHealth;
            _enemyDamage = enemyDamage;
            _playerMovement = playerMovement;
        }

        public event Action TurnCompleted;

        public void Move()
        {
            List<Cell> targetCells = _enemyTargetController.GetCurrentTargetCells();
            TripleAimAmount = 3;

            if (targetCells.Count == TripleAimAmount)
            {
                _enemyJumper.JumpToTargetThreeInRowCells(targetCells,
                        () => _playerHealth.TakeDoubleDamage(_enemyDamage.Value))
                    .AppendCallback(() =>
                    {
                        _enemyTargetController.NextTurnOneTarget();
                        _enemyTargetController.SetAimToNewRandomTargetCell();
                        TurnCompleted?.Invoke();
                    })
                    .Play();
            }
            else if (targetCells[0] == _playerMovement.PlayerStayCell)
            {
                _enemyJumper.JumpToAttackPlayer(targetCells[0],
                        () => _playerHealth.TakeDoubleDamage(_enemyDamage.Value))
                    .AppendCallback(() => TurnCompleted?.Invoke())
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