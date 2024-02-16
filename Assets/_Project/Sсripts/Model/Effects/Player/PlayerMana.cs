using System;
using _Project.Sсripts.HealthAndMana;
using _Project.Sсripts.Movement;
using DG.Tweening;

namespace _Project.Sсripts.Model.Effects.Player
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
            sequence.Append(_playerJumper.PlayerJumpInPlace());
            sequence.AppendCallback(onComplete.Invoke);
            sequence.Play();
        }
    }
}