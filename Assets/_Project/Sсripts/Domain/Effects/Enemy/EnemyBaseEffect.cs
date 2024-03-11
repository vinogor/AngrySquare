using System;
using DG.Tweening;
using Domain.Movement;
using UnityEngine.Assertions;

namespace Domain.Effects.Enemy
{
    public abstract class EnemyBaseEffect : Effect
    {
        private readonly EnemyJumper _enemyJumper;

        protected EnemyBaseEffect(EnemyJumper enemyJumper)
        {
            Assert.IsNotNull(enemyJumper);
            _enemyJumper = enemyJumper;
        }

        protected override void Execute(Action onComplete)
        {
            Sequence = DOTween.Sequence()
                .Append(_enemyJumper.JumpToTargetCell())
                .Append(_enemyJumper.JumpBackToBase())
                .AppendCallback(onComplete.Invoke)
                .Play();
        }
    }
}