using System;
using _Project.Sсripts.Hp;
using _Project.Sсripts.Movement;

namespace _Project.Sсripts.Model.Effects
{
    public class PlayerHealth : Effect
    {
        private Health _playerHealth;
        private PlayerJumper _playerJumper;

        public PlayerHealth(Health playerHealth, PlayerJumper playerJumper)
        {
            _playerHealth = playerHealth;
            _playerJumper = playerJumper;
        }

        public override void Activate(Action onComplete)
        {
            base.Activate(onComplete);

            _playerHealth.ReplenishToMax();
            _playerJumper.PlayerJumpInPlace(onComplete.Invoke);
        }
    }
}