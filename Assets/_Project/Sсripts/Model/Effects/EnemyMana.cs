using System;
using _Project.Sсripts.Movement;

namespace _Project.Sсripts.Model.Effects
{
    public class EnemyMana : Effect
    {
        private EnemyJumper _enemyJumper;

        public EnemyMana(EnemyJumper enemyJumper)
        {
            _enemyJumper = enemyJumper;
        }

        protected override void Execute(Action onComplete)
        {
            _enemyJumper.EnemyJumpToTargetCell(
                () => _enemyJumper.EnemyJumpBackToBase(onComplete.Invoke));
        }
    }
}