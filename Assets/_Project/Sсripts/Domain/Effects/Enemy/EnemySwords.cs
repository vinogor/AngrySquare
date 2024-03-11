using System;
using DG.Tweening;
using Domain.Movement;
using UnityEngine.Assertions;

namespace Domain.Effects.Enemy
{
    public class EnemySwords : EnemyBaseEffect
    {
        private readonly EnemyJumper _enemyJumper;
        private readonly Health _playerHealth;
        private readonly Damage _enemyDamage;

        public EnemySwords(EnemyJumper enemyJumper, Health playerHealth, Damage enemyDamage) : base(enemyJumper)
        {
            Assert.IsNotNull(enemyJumper);
            Assert.IsNotNull(playerHealth);
            Assert.IsNotNull(enemyDamage);

            _enemyJumper = enemyJumper;
            _playerHealth = playerHealth;
            _enemyDamage = enemyDamage;
        }

        protected override void Execute(Action onComplete)
        {
            Sequence = DOTween.Sequence()
                .Append(_enemyJumper.JumpToTargetCell())
                .Append(_enemyJumper.JumpOnPlayer())
                .AppendCallback(() => _playerHealth.TakeDamage(_enemyDamage.Value))
                .Append(_enemyJumper.JumpBackToBase())
                .AppendCallback(onComplete.Invoke)
                .Play();
        }
    }
}