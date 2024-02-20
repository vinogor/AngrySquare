using System;
using _Project.Sсripts.Services.Movement;
using DG.Tweening;

namespace _Project.Sсripts.Services.Effects.Enemy{
    public class EnemyMana : Effect
    {
        private readonly EnemyJumper _enemyJumper;

        public EnemyMana(EnemyJumper enemyJumper)
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