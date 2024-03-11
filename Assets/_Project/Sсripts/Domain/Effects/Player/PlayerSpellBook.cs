using System;
using Controllers.PopupChoice;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Domain.Movement;
using UnityEngine.Assertions;

namespace Domain.Effects.Player
{
    public class PlayerSpellBook : Effect
    {
        private readonly PlayerJumper _playerJumper;
        private readonly PopUpChoiceSpellPresenter _popUpPresenter;

        public PlayerSpellBook(PlayerJumper playerJumper, PopUpChoiceSpellPresenter popUpPresenter)
        {
            Assert.IsNotNull(playerJumper);
            Assert.IsNotNull(popUpPresenter);

            _playerJumper = playerJumper;
            _popUpPresenter = popUpPresenter;
        }

        protected override async void Execute(Action onComplete)
        {
            await Execute();
            onComplete.Invoke();
        }

        private async UniTask Execute()
        {
            await Jump();
            await _popUpPresenter.ShowSpells();
        }

        private async UniTask Jump()
        {
            Sequence = DOTween.Sequence()
                .Append(_playerJumper.JumpInPlace());
            await Sequence.AsyncWaitForCompletion();
        }
    }
}