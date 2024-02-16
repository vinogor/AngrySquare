using System;
using _Project.Sсripts.Movement;
using DG.Tweening;

namespace _Project.Sсripts.Model.Effects.Enemy
{
    public class EnemyHealth : Effect
    {
        private readonly EnemyJumper _enemyJumper;

        public EnemyHealth(EnemyJumper enemyJumper)
        {
            _enemyJumper = enemyJumper;
        }

        protected override void Execute(Action onComplete)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_enemyJumper.EnemyJumpToTargetCell());
            sequence.Append(_enemyJumper.EnemyJumpBackToBase());
            sequence.AppendCallback(onComplete.Invoke);
            sequence.Play();
        }
    }
}