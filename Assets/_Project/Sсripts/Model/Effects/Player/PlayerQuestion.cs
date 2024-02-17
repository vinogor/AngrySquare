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
        private readonly PopUpChoiceOfThreeController _popUpChoiceOfThreeController;

        public PlayerQuestion(PlayerJumper playerJumper, PopUpChoiceOfThreeController popUpChoiceOfThreeController)
        {
            Assert.IsNotNull(playerJumper);
            Assert.IsNotNull(popUpChoiceOfThreeController);
            
            _playerJumper = playerJumper;
            _popUpChoiceOfThreeController = popUpChoiceOfThreeController;
        }

        protected override void Execute(Action onComplete)
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_playerJumper.JumpInPlace());
            sequence.AppendCallback(_popUpChoiceOfThreeController.ShowEffects);
            sequence.Play();

            // onComplete - никуда не передаём, т.к. он будет вызван с выбраннного в попапе эффекта
        }
    }
}