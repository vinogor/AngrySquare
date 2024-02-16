using System;
using _Project.Sсripts.DamageAndDefence;
using _Project.Sсripts.HealthAndMana;
using _Project.Sсripts.Movement;

namespace _Project.Sсripts.Model.Effects.Enemy
{
    public class EnemySwords : Effect
    {
        private readonly EnemyJumper _enemyJumper;
        private readonly Health _playerHealth;
        private readonly Damage _enemyDamage;

        public EnemySwords(EnemyJumper enemyJumper, Health playerHealth, Damage enemyDamage)
        {
            _enemyJumper = enemyJumper;
            _playerHealth = playerHealth;
            _enemyDamage = enemyDamage;
        }

        protected override void Execute(Action onComplete)
        {
            _enemyJumper.EnemyJumpToTargetCell(
                () => _enemyJumper.EnemyJumpOnPlayer(() =>
                {
                    _playerHealth.TakeDamage(_enemyDamage.Value);
                    _enemyJumper.EnemyJumpBackToBase(onComplete.Invoke);
                }));
        }
    }
}