using System;
using _Project.Sсripts.Movement;
using _Project.Sсripts.UI;
using _Project.Sсripts.UI.PopupChoice;
using DG.Tweening;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Model.Effects.Player
{
    public class PlayerSpellBook : Effect
    {
        private readonly PlayerJumper _playerJumper;
        private readonly PopUpChoiceSpellController _popUpController;
        private readonly SpellBarController _spellBarController;

        private Action _onComplete;

        public PlayerSpellBook(PlayerJumper playerJumper,  PopUpChoiceSpellController popUpController,
            SpellBarController spellBarController)
        {
            Assert.IsNotNull(playerJumper);
            Assert.IsNotNull(popUpController);
            Assert.IsNotNull(spellBarController);

            _playerJumper = playerJumper;
            _popUpController = popUpController;
            _spellBarController = spellBarController;
        }

        protected override void Execute(Action onComplete)
        {
            _onComplete = onComplete;
            _spellBarController.Took += OnSpellTook;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(_playerJumper.JumpInPlace());
            sequence.AppendCallback(() => _popUpController.ShowSpells());
            sequence.Play();
        }

        private void OnSpellTook()
        {
            _spellBarController.Took -= OnSpellTook;
            _onComplete.Invoke();
        }
    }
}