using System;
using System.Collections.Generic;
using _Project.Sсripts.Animation;
using _Project.Sсripts.Dmg;
using _Project.Sсripts.Hp;
using _Project.Sсripts.Model;
using _Project.Sсripts.Model.Effects;
using _Project.Sсripts.Scriptable;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Movement
{
    public class EnemyMovement : MonoBehaviour
    {
        private Cell _targetCell;
        private Dictionary<EffectName, Effect> _enemyEffects;
        private EnemyTargetController _enemyTargetController;
        private EnemyJumper _enemyJumper;
        private PlayerMovement _playerMovement;
        private Health _playerHealth;
        private Damage _enemyDamage;

        public void Initialize(Cell targetCell, Dictionary<EffectName, Effect> enemyEffects,
            EnemyTargetController enemyTargetController, EnemyJumper enemyJumper, PlayerMovement playerMovement,
            Health playerHealth, Damage enemyDamage)
        {
            Assert.IsNotNull(targetCell);
            Assert.IsNotNull(enemyEffects);
            Assert.IsNotNull(enemyTargetController);
            Assert.IsNotNull(enemyJumper);
            Assert.IsNotNull(playerMovement);
            Assert.IsNotNull(playerHealth);
            Assert.IsNotNull(enemyDamage);

            _targetCell = targetCell;
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
            if (_enemyTargetController.GetCurrentTargetCell() == _playerMovement.PlayerStayCell)
            {
                _enemyJumper.EnemyJumpToTargetCell(() =>
                {
                    // TODO: обыграть визуал по особенному 
                    // TODO: почему-то прыгает 2 раза
                    _playerHealth.TakeDamage(_enemyDamage.Value * 3);
                    _enemyJumper.EnemyJumpBackToBase(() => TurnCompleted?.Invoke());
                });
            }
            else
            {
                EffectName effectName = _targetCell.EffectName;
                _enemyEffects[effectName].Activate(() =>
                {
                    _enemyTargetController.SetAimToNewRandomTargetCell();
                    TurnCompleted?.Invoke();
                });
            }
        }
    }
}