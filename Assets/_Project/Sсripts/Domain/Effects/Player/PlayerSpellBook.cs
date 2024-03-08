using System;
using System.Threading.Tasks;
using Controllers.PopupChoice;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Domain.Movement;
using UnityEngine.Assertions;

namespace Domain.Effects.Player{
    public class PlayerSpellBook : Effect
    {
        private readonly PlayerJumper _playerJumper;
        private readonly PopUpChoiceSpellController _popUpController;

        public PlayerSpellBook(PlayerJumper playerJumper, PopUpChoiceSpellController popUpController)
        {
            Assert.IsNotNull(playerJumper);
            Assert.IsNotNull(popUpController);

            _playerJumper = playerJumper;
            _popUpController = popUpController;
        }

        protected override async void Execute(Action onComplete)
        {
            await Execute();
            onComplete.Invoke();
        }

        private async UniTask Execute()
        {
            await Jump();
            await _popUpController.ShowSpells();
        }

        private async UniTask Jump()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_playerJumper.JumpInPlace());
            await sequence.AsyncWaitForCompletion();
        }
    }
}