using System;
using Controllers.PopupChoice;
using DG.Tweening;
using Domain.Movement;
using UnityEngine.Assertions;

namespace Domain.Effects.Player{
    public class PlayerQuestion : Effect
    {
        private readonly PlayerJumper _playerJumper;
        private readonly PopUpChoiceEffectController _popupController;

        public PlayerQuestion(PlayerJumper playerJumper, PopUpChoiceEffectController popupController)
        {
            Assert.IsNotNull(playerJumper);
            Assert.IsNotNull(popupController);

            _playerJumper = playerJumper;
            _popupController = popupController;
        }

        protected override void Execute(Action onComplete)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_playerJumper.JumpInPlace());
            sequence.AppendCallback(_popupController.ShowEffects);
            sequence.Play();
        }
    }
}