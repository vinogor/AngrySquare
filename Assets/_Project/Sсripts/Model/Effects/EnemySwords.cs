using System;
using _Project.Sсripts.Dmg;
using _Project.Sсripts.HealthAndMana;
using _Project.Sсripts.Movement;

namespace _Project.Sсripts.Model.Effects
{
    public class EnemySwords : Effect
    {
        private EnemyJumper _enemyJumper;
        private Health _playerHealth;
        private Damage _enemyDamage;

        public EnemySwords(EnemyJumper enemyJumper, Health playerHealth, Damage enemyDamage)
        {
            _enemyJumper = enemyJumper;
            _playerHealth = playerHealth;
            _enemyDamage = enemyDamage;
        }

        public override void Activate(Action onComplete)
        {
            Log();
            
            _enemyJumper.EnemyJumpToTargetCell(
                () => _enemyJumper.EnemyJumpOnPlayer(() =>
                {
                    _playerHealth.TakeDamage(_enemyDamage.Value);
                    _enemyJumper.EnemyJumpBackToBase(onComplete.Invoke);
                }));
        }
    }
}