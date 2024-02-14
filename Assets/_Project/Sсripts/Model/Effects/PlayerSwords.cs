using System;
using _Project.Sсripts.Dmg;
using _Project.Sсripts.HealthAndMana;
using _Project.Sсripts.Movement;

namespace _Project.Sсripts.Model.Effects
{
    public class PlayerSwords : Effect
    {
        private PlayerJumper _playerJumper;
        private Health _enemyHealth;
        private Damage _playerDamage;

        public PlayerSwords(PlayerJumper playerJumper, Health enemyHealth, Damage playerDamage)
        {
            _playerJumper = playerJumper;
            _enemyHealth = enemyHealth;
            _playerDamage = playerDamage;
        }

        public override void Activate(Action onComplete)
        {
            Log();

            _playerJumper.PlayerJumpOnEnemy(() =>
            {
                _enemyHealth.TakeDamage(_playerDamage.Value);
                _playerJumper.PlayerJumpBackToCell(onComplete);
            });
        }
    }
}