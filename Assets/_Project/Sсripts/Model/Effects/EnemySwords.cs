using System;
using _Project.Sсripts.Animation;
using _Project.Sсripts.Dmg;
using _Project.Sсripts.Hp;
using _Project.Sсripts.Movement;
using _Project.Sсripts.Scriptable;
using UnityEngine;

namespace _Project.Sсripts.Model.Effects
{
    public class EnemySwords : Effect
    {
        public EnemySwords(Transform enemyTransform, BaseSettings baseSettings, Health enemyHealth,
            PlayerMovement playerMovement, Health playerHealth, EnemyTargetController enemyTargetController,
            Damage enemyDamage, Transform playerTransform, Damage playerDamage) : base(enemyTransform, baseSettings,
            enemyHealth, playerMovement, playerHealth, enemyTargetController, enemyDamage, playerTransform,
            playerDamage)
        {
        }

        protected override void ActivateSpecial(Action onComplete)
        {
            EnemyJumpToTargetCell(
                () => EnemyJumpOnPlayer(() =>
                {
                    PlayerHealth.TakeDamage(EnemyDamageValue);
                    EnemyJumpBackToBase(() => onComplete.Invoke());
                }));
        }
    }
}