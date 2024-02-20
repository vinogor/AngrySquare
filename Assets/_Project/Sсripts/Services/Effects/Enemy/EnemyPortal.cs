using System;
using DG.Tweening;

namespace _Project.Sсripts{
    public class EnemyPortal : Effect
    {
        private readonly EnemyJumper _enemyJumper;

        public EnemyPortal(EnemyJumper enemyJumper)
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