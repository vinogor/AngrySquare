using System;
using _Project.Sсripts.Config;
using _Project.Sсripts.Domain.Movement;
using DG.Tweening;

namespace _Project.Sсripts.Domain.Effects.Player
{
    public class PlayerMana : Effect
    {
        private readonly Mana _playerMana;
        private readonly PlayerJumper _playerJumper;
        private readonly Coefficients _coefficients;

        public PlayerMana(Mana playerMana, PlayerJumper playerJumper, Coefficients coefficients)
        {
            _playerMana = playerMana;
            _playerJumper = playerJumper;
            _coefficients = coefficients;
        }

        protected override void Execute(Action onComplete)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_playerJumper.JumpInPlace());

            if (_playerMana.Value != _playerMana.MaxValue)
            {
                sequence.AppendCallback(() => _playerMana.ReplenishToMax());
                sequence.AppendInterval(_coefficients.DelayAfterVfxSeconds);
            }

            sequence.AppendCallback(onComplete.Invoke);
            sequence.Play();
        }
    }
}