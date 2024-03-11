using System;
using Controllers.PopupChoice;
using DG.Tweening;
using Domain.Movement;
using UnityEngine.Assertions;

namespace Domain.Effects.Player
{
    public class PlayerQuestion : Effect
    {
        private readonly PlayerJumper _playerJumper;
        private readonly PopUpChoiceEffectPresenter _popupPresenter;

        public PlayerQuestion(PlayerJumper playerJumper, PopUpChoiceEffectPresenter popupPresenter)
        {
            Assert.IsNotNull(playerJumper);
            Assert.IsNotNull(popupPresenter);

            _playerJumper = playerJumper;
            _popupPresenter = popupPresenter;
        }

        protected override void Execute(Action onComplete)
        {
            Sequence = DOTween.Sequence()
                .Append(_playerJumper.JumpInPlace())
                .AppendCallback(_popupPresenter.ShowEffects)
                .Play();
        }
    }
}