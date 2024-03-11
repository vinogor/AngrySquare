using System;
using Config;
using DG.Tweening;
using Domain.Movement;
using UnityEngine.Assertions;

namespace Domain.Effects.Player
{
    public class PlayerMana : Effect
    {
        private readonly Mana _playerMana;
        private readonly PlayerJumper _playerJumper;
        private readonly Coefficients _coefficients;

        public PlayerMana(Mana playerMana, PlayerJumper playerJumper, Coefficients coefficients)
        {
            Assert.IsNotNull(playerMana);
            Assert.IsNotNull(playerJumper);
            Assert.IsNotNull(coefficients);

            _playerMana = playerMana;
            _playerJumper = playerJumper;
            _coefficients = coefficients;
        }

        protected override void Execute(Action onComplete)
        {
            Sequence = DOTween.Sequence()
                .Append(_playerJumper.JumpInPlace());

            if (_playerMana.Value != _playerMana.MaxValue)
            {
                Sequence.AppendCallback(() => _playerMana.ReplenishToMax());
                Sequence.AppendInterval(_coefficients.DelayAfterVfxSeconds);
            }

            Sequence.AppendCallback(onComplete.Invoke);
            Sequence.Play();
        }
    }
}