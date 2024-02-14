using System;
using _Project.Sсripts.Movement;

namespace _Project.Sсripts.Model.Effects
{
    public class EnemyHealth : Effect
    {
        private EnemyJumper _enemyJumper;

        public EnemyHealth(EnemyJumper enemyJumper)
        {
            _enemyJumper = enemyJumper;
        }

        public override void Activate(Action onComplete)
        {
            Log();
            
            // TODO: какой будет эффект для Противника? (пока просто прыгает)
            _enemyJumper.EnemyJumpToTargetCell(
                () => _enemyJumper.EnemyJumpBackToBase(onComplete.Invoke));
        }
    }
}