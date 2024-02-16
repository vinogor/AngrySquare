using System;
using _Project.Sсripts.HealthAndMana;
using _Project.Sсripts.Movement;

namespace _Project.Sсripts.Model.Effects.Player
{
    public class PlayerHealth : Effect
    {
        private readonly Health _playerHealth;
        private readonly PlayerJumper _playerJumper;

        public PlayerHealth(Health playerHealth, PlayerJumper playerJumper)
        {
            _playerHealth = playerHealth;
            _playerJumper = playerJumper;
        }

        protected override void Execute(Action onComplete)
        {
            _playerHealth.ReplenishToMax();
            _playerJumper.PlayerJumpInPlace(onComplete.Invoke);
        }
    }
}