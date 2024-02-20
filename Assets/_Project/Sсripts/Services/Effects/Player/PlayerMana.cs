using System;
using _Project.Sсripts.Domain;
using _Project.Sсripts.Services.Movement;
using DG.Tweening;

namespace _Project.Sсripts.Services.Effects.Player
{
    public class PlayerMana : Effect
    {
        private readonly Mana _playerMana;
        private readonly PlayerJumper _playerJumper;

        public PlayerMana(Mana playerMana, PlayerJumper playerJumper)
        {
            _playerMana = playerMana;
            _playerJumper = playerJumper;
        }

        protected override void Execute(Action onComplete)
        {
            _playerMana.ReplenishToMax();

            Sequence sequence = DOTween.Sequence();
            sequence.Append(_playerJumper.JumpInPlace());
            sequence.AppendCallback(onComplete.Invoke);
            sequence.Play();
        }
    }
}