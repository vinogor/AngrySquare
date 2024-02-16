using System;
using _Project.Sсripts.DamageAndDefence;
using _Project.Sсripts.HealthAndMana;
using _Project.Sсripts.Movement;

namespace _Project.Sсripts.Model.Effects.Player
{
    public class PlayerSwords : Effect
    {
        private readonly PlayerJumper _playerJumper;
        private readonly Health _enemyHealth;
        private readonly Damage _playerDamage;

        public PlayerSwords(PlayerJumper playerJumper, Health enemyHealth, Damage playerDamage)
        {
            _playerJumper = playerJumper;
            _enemyHealth = enemyHealth;
            _playerDamage = playerDamage;
        }

        protected override void Execute(Action onComplete)
        {
            _playerJumper.PlayerJumpOnEnemy(() =>
            {
                _enemyHealth.TakeDamage(_playerDamage.Value);
                _playerJumper.PlayerJumpBackToCell(onComplete);
            });
        }
    }
}