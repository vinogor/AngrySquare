using System;
using System.Threading.Tasks;
using Controllers.PopupChoice;
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

        private async Task Execute()
        {
            await Jump();
            await _popUpController.ShowSpells();
        }

        private async Task Jump()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_playerJumper.JumpInPlace());
            await sequence.AsyncWaitForCompletion();
        }
    }
}