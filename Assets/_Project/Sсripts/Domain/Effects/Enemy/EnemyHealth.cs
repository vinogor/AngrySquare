using System;
using _Project.Sсripts.Config;
using _Project.Sсripts.Controllers;
using _Project.Sсripts.Domain.Movement;
using DG.Tweening;

namespace _Project.Sсripts.Domain.Effects.Enemy
{
    public class EnemyHealth : Effect
    {
        private readonly EnemyJumper _enemyJumper;
        private readonly Health _enemyHealth;
        private readonly Coefficients _coefficients;
        private readonly EnemyTargetController _enemyTargetController;

        public EnemyHealth(EnemyJumper enemyJumper, Health enemyHealth, Coefficients coefficients,
            EnemyTargetController enemyTargetController)
        {
            _enemyJumper = enemyJumper;
            _enemyHealth = enemyHealth;
            _coefficients = coefficients;
            _enemyTargetController = enemyTargetController;
        }

        protected override void Execute(Action onComplete)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_enemyJumper.JumpToTargetCell());

            if (_enemyHealth.Value != _enemyHealth.MaxValue)
            {
                sequence.AppendCallback(() => _enemyHealth.ReplenishToMax());
                sequence.AppendInterval(_coefficients.DelayAfterVfxSeconds);
            }
            sequence.AppendCallback(() => _enemyTargetController.NextTurnTripleTarget());
            sequence.Append(_enemyJumper.JumpBackToBase());
            sequence.AppendCallback(onComplete.Invoke);
            sequence.Play();
        }
    }
}