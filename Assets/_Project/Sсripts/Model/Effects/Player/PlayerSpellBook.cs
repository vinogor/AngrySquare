using System;
using System.Threading.Tasks;
using _Project.Sсripts.Movement;
using _Project.Sсripts.UI.PopupChoice;
using DG.Tweening;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Model.Effects.Player
{
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
            await _popUpController.ShowSpells1();
        }

        private async Task Jump()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_playerJumper.JumpInPlace());
            await sequence.AsyncWaitForCompletion();
        }
    }
}