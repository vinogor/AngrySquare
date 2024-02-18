using System;
using System.Collections;
using System.Threading.Tasks;
using _Project.Sсripts.Movement;
using _Project.Sсripts.UI.PopupChoice;
using _Project.Sсripts.Utility;
using DG.Tweening;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Model.Effects.Player
{
    public static class TaskUtils
    {
        public static async Task WaitUntil(Func<bool> predicate)
        {
            while (!predicate())
            {
                await Task.Delay(50);
            }
        }
    }

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

        protected override void Execute(Action onComplete)
        {
            // т.к. класс не монобех 
            Coroutines.StartRoutine(ExecuteCo(onComplete));
        }

        private IEnumerator ExecuteCo(Action onComplete)
        {
            yield return Jump();
            yield return _popUpController.ShowSpells();
            onComplete.Invoke();
        }

        private async Task Execute1()
        {
            await Jump1();
            await _popUpController.ShowSpells1();
        }

        private IEnumerator Jump()
        {
            Sequence sequence = DOTween.Sequence();
            yield return sequence.Append(_playerJumper.JumpInPlace())
                .WaitForCompletion();
        }

        private async Task Jump1()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_playerJumper.JumpInPlace());
            sequence.Play();

            await TaskUtils.WaitUntil(() => sequence.IsComplete());
        }
    }
}