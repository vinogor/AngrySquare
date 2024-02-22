using System;
using _Project.Sсripts.Config;
using _Project.Sсripts.Domain.Movement;
using DG.Tweening;

namespace _Project.Sсripts.Domain.Effects.Player
{
    public class PlayerHealth : Effect
    {
        private readonly Health _playerHealth;
        private readonly PlayerJumper _playerJumper;
        private readonly Coefficients _coefficients;

        public PlayerHealth(Health playerHealth, PlayerJumper playerJumper, Coefficients coefficients)
        {
            _playerHealth = playerHealth;
            _playerJumper = playerJumper;
            _coefficients = coefficients;
        }

        protected override void Execute(Action onComplete)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_playerJumper.JumpInPlace());
            sequence.AppendCallback(() => _playerHealth.ReplenishToMax());
            sequence.AppendInterval(_coefficients.DelayAfterVfxSeconds);
            sequence.AppendCallback(onComplete.Invoke);
            sequence.Play();
        }
    }
}