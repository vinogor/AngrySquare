using System;
using _Project.Config;
using _Project.Domain.Movement;
using DG.Tweening;
using UnityEngine.Assertions;

namespace _Project.Domain.Effects.Player
{
    public class PlayerHealth : Effect
    {
        private readonly Health _playerHealth;
        private readonly PlayerJumper _playerJumper;
        private readonly Coefficients _coefficients;

        public PlayerHealth(Health playerHealth, PlayerJumper playerJumper, Coefficients coefficients)
        {
            Assert.IsNotNull(playerHealth);
            Assert.IsNotNull(playerJumper);
            Assert.IsNotNull(coefficients);
            
            _playerHealth = playerHealth;
            _playerJumper = playerJumper;
            _coefficients = coefficients;
        }

        protected override void Execute(Action onComplete)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_playerJumper.JumpInPlace());

            if (_playerHealth.Value != _playerHealth.MaxValue)
            {
                sequence.AppendCallback(() => _playerHealth.ReplenishToMax());
                sequence.AppendInterval(_coefficients.DelayAfterVfxSeconds);
            }
            
            sequence.AppendCallback(onComplete.Invoke);
            sequence.Play();
        }
    }
}