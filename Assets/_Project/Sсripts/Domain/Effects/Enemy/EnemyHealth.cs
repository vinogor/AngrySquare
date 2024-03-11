using System;
using Config;
using Controllers;
using DG.Tweening;
using Domain.Movement;
using UnityEngine.Assertions;

namespace Domain.Effects.Enemy
{
    public class EnemyHealth : EnemyBaseEffect
    {
        private readonly EnemyJumper _enemyJumper;
        private readonly Health _enemyHealth;
        private readonly Coefficients _coefficients;
        private readonly EnemyTargetController _enemyTargetController;

        public EnemyHealth(EnemyJumper enemyJumper, Health enemyHealth, Coefficients coefficients,
            EnemyTargetController enemyTargetController) : base(enemyJumper)
        {
            Assert.IsNotNull(enemyJumper);
            Assert.IsNotNull(enemyHealth);
            Assert.IsNotNull(coefficients);
            Assert.IsNotNull(enemyTargetController);

            _enemyJumper = enemyJumper;
            _enemyHealth = enemyHealth;
            _coefficients = coefficients;
            _enemyTargetController = enemyTargetController;
        }

        protected override void Execute(Action onComplete)
        {
            Sequence = DOTween.Sequence()
                .Append(_enemyJumper.JumpToTargetCell());

            if (_enemyHealth.Value != _enemyHealth.MaxValue)
            {
                Sequence.AppendCallback(() => _enemyHealth.ReplenishToMax());
                Sequence.AppendInterval(_coefficients.DelayAfterVfxSeconds);
            }

            Sequence.AppendCallback(() => _enemyTargetController.NextTurnTripleTarget())
                .Append(_enemyJumper.JumpBackToBase())
                .AppendCallback(onComplete.Invoke)
                .Play();
        }
    }
}