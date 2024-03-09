using System;
using Controllers.PopupChoice;
using DG.Tweening;
using Domain.Movement;
using UnityEngine.Assertions;

namespace Domain.Effects.Player{
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
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_playerJumper.JumpInPlace());
            sequence.AppendCallback(_popupPresenter.ShowEffects);
            sequence.Play();
        }
    }
}