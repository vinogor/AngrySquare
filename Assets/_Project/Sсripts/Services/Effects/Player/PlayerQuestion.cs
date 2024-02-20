using System;
using _Project.Sсripts.Controllers.PopupChoice;
using _Project.Sсripts.Services.Movement;
using DG.Tweening;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Services.Effects.Player{
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

            // onComplete - никуда не передаём, т.к. он будет вызван с выбраннного в попапе эффекта
        }
    }
}