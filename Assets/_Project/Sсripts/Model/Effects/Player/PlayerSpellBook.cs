using System;
using _Project.Sсripts.Movement;
using _Project.Sсripts.UI;
using DG.Tweening;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Model.Effects.Player
{
    public class PlayerSpellBook : Effect
    {
        private readonly PlayerJumper _playerJumper;
        private readonly PopUpChoiceOfThreeController _popUp;
        private readonly SpellBarController _spellBarController;

        private Action _onComplete;

        public PlayerSpellBook(PlayerJumper playerJumper, PopUpChoiceOfThreeController popUp,
            SpellBarController spellBarController)
        {
            Assert.IsNotNull(playerJumper);
            Assert.IsNotNull(popUp);
            Assert.IsNotNull(spellBarController);

            _playerJumper = playerJumper;
            _popUp = popUp;
            _spellBarController = spellBarController;
        }

        protected override void Execute(Action onComplete)
        {
            _onComplete = onComplete;
            _spellBarController.Took += OnSpellTook;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(_playerJumper.JumpInPlace());
            sequence.AppendCallback(() => _popUp.ShowSpells());
            sequence.Play();
        }

        private void OnSpellTook()
        {
            _spellBarController.Took -= OnSpellTook;
            _onComplete.Invoke();
        }
    }
}