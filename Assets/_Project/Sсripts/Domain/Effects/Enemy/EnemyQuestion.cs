using System;
using _Project.Sсripts.Domain.Movement;
using DG.Tweening;

namespace _Project.Sсripts.Domain.Effects.Enemy{
    public class EnemyQuestion : Effect
    {
        private readonly EnemyJumper _enemyJumper;

        public EnemyQuestion(EnemyJumper enemyJumper)
        {
            _enemyJumper = enemyJumper;
        }

        protected override void Execute(Action onComplete)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_enemyJumper.JumpToTargetCell());
            sequence.Append(_enemyJumper.JumpBackToBase());
            sequence.AppendCallback(onComplete.Invoke);
            sequence.Play();
        }
    }
}