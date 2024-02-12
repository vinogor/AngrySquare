using System;
using _Project.Sсripts.Animation;
using _Project.Sсripts.Dmg;
using _Project.Sсripts.Hp;
using _Project.Sсripts.Movement;
using _Project.Sсripts.Scriptable;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Model.Effects
{
    // TODO: разделить на 2 абстрактных - для Игрока и для Противника ?
    public abstract class Effect
    {
        // enemy
        protected Transform EnemyTransform;
        protected Transform PlayerTransform;
        protected Health EnemyHealth;
        protected Damage EnemyDamage;
        protected EnemyTargetController EnemyTargetController;
        private Vector3 _startEnemyPosition;

        // player
        protected PlayerMovement PlayerMovement;
        protected Health PlayerHealth;
        protected Damage PlayerDamage;
        private Vector3 _playerCellPosition;

        // common
        protected BaseSettings BaseSettings;

        protected Effect(Transform enemyTransform, BaseSettings baseSettings, Health enemyHealth,
            PlayerMovement playerMovement,
            Health playerHealth, EnemyTargetController enemyTargetController, Damage enemyDamage,
            Transform playerTransform,
            Damage playerDamage)
        {
            Assert.IsNotNull(enemyTransform);
            Assert.IsNotNull(baseSettings);
            Assert.IsNotNull(playerMovement);
            Assert.IsNotNull(playerHealth);
            Assert.IsNotNull(enemyTargetController);
            Assert.IsNotNull(enemyDamage);
            Assert.IsNotNull(playerTransform);
            Assert.IsNotNull(enemyHealth);
            Assert.IsNotNull(playerDamage);

            EnemyTransform = enemyTransform;
            BaseSettings = baseSettings;
            PlayerMovement = playerMovement;
            PlayerHealth = playerHealth;
            EnemyTargetController = enemyTargetController;
            EnemyDamage = enemyDamage;
            PlayerTransform = playerTransform;
            EnemyHealth = enemyHealth;
            PlayerDamage = playerDamage;

            _startEnemyPosition = EnemyTransform.position;
        }

        protected int EnemyDamageValue => EnemyDamage.Value;
        protected int PlayerDamageValue => PlayerDamage.Value;

        public void Activate(Action onComplete)
        {
            Debug.Log($"Effect - {GetType().Name} - Activate");

            if (EnemyTargetController.GetCurrentTargetCell() == PlayerMovement.PlayerStayCell)
            {
                EnemyJumpToTargetCell(() =>
                {
                    // TODO: обыграть визуал по особенному 
                    // TODO: почему-то прыгает 2 раза
                    PlayerHealth.TakeDamage(EnemyDamageValue * 3);
                    EnemyJumpBackToBase(onComplete.Invoke);
                });
            }
            else
            {
                ActivateSpecial(onComplete);
            }
        }

        protected abstract void ActivateSpecial(Action onComplete);

        protected void EnemyJumpToTargetCell(Action onJumpComplete)
        {
            Debug.Log("Enemy - JumpToCell");

            Jump(EnemyTransform,
                EnemyTargetController.GetCurrentTargetCell().Center() + Vector3.up * BaseSettings.EnemyHeight,
                onJumpComplete);
        }

        protected void EnemyJumpOnPlayer(Action onJumpComplete)
        {
            Debug.Log("Enemy - JumpOnPlayer");

            Jump(EnemyTransform, PlayerMovement.PlayerStayCell.Center() + Vector3.up * BaseSettings.EnemyHeight,
                onJumpComplete);
        }

        protected void EnemyJumpBackToBase(Action onJumpComplete)
        {
            Debug.Log("Enemy - JumpToBase");

            Jump(EnemyTransform, _startEnemyPosition, onJumpComplete);
        }

        protected void PlayerJumpOnEnemy(Action onJumpComplete)
        {
            Debug.Log("Player - JumpOnEnemy");

            _playerCellPosition = PlayerTransform.position;
            Jump(PlayerTransform, EnemyTransform.position, onJumpComplete);
        }

        protected void PlayerJumpBackToCell(Action onJumpComplete)
        {
            Debug.Log("Player - JumpBackToCell");

            Jump(PlayerTransform, _playerCellPosition, onJumpComplete);
        }

        private void Jump(Transform transform, Vector3 target, Action onJumpComplete)
        {
            transform
                .DOJump(target, BaseSettings.JumpPower, 1, BaseSettings.JumpDuration)
                .SetEase(Ease.Linear)
                .OnComplete(onJumpComplete.Invoke);
        }
    }
}