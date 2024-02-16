using System;
using _Project.Sсripts.Movement;
using _Project.Sсripts.UI;
using DG.Tweening;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Model.Effects.Player
{
    public class PlayerQuestion : Effect
    {
        private readonly PlayerJumper _playerJumper;
        private readonly PopUpQuestionController _popUpQuestionController;

        public PlayerQuestion(PlayerJumper playerJumper, PopUpQuestionController popUpQuestionController)
        {
            Assert.IsNotNull(playerJumper);
            Assert.IsNotNull(popUpQuestionController);
            
            _playerJumper = playerJumper;
            _popUpQuestionController = popUpQuestionController;
        }

        protected override void Execute(Action onComplete)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_playerJumper.PlayerJumpInPlace());
            sequence.AppendCallback(_popUpQuestionController.Show);
            sequence.Play();

            // onComplete - никуда не передаём, т.к. он будет вызван с выбраннного в попапе эффекта
        }
    }
}