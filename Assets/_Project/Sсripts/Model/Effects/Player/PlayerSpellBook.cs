using System;
using _Project.Sсripts.Movement;
using DG.Tweening;

namespace _Project.Sсripts.Model.Effects.Player
{
    public class PlayerSpellBook : Effect
    {
        private readonly PlayerJumper _playerJumper;

        public PlayerSpellBook(PlayerJumper playerJumper)
        {
            _playerJumper = playerJumper;
        }

        protected override void Execute(Action onComplete)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_playerJumper.PlayerJumpInPlace());
            sequence.AppendCallback(onComplete.Invoke);
            sequence.Play();
        }
    }
}