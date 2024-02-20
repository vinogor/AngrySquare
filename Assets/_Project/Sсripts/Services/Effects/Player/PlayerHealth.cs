using System;
using _Project.Sсripts.HealthAndMana;
using DG.Tweening;

namespace _Project.Sсripts{
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

            Sequence sequence = DOTween.Sequence();
            sequence.Append(_playerJumper.JumpInPlace());
            sequence.AppendCallback(onComplete.Invoke);
            sequence.Play();
        }
    }
}