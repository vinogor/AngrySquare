using System.Linq;
using _Project.Sсripts.Movement;
using _Project.Sсripts.Scriptable;
using _Project.Sсripts.Utility;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace _Project.Sсripts.UI
{
    public class PopUpChoiceOfThreeController
    {
        private readonly PopUpChoiceOfThree _popUpChoiceOfThree;
        private readonly CellsAndSpellsSettings _cellsAndSpellsSettings;
        private readonly EffectName[] _availableEffectNames;
        private readonly SpellName[] _availableSpellNames;
        private readonly PlayerMovement _playerMovement;
        private readonly SpellBarController _spellBarController;

        private EffectName[] _activeEffects;
        private const int AmountItems = 3;

        public PopUpChoiceOfThreeController(PopUpChoiceOfThree popUpChoiceOfThree,
            CellsAndSpellsSettings cellsAndSpellsSettings, EffectName[] availableEffectNames,
            SpellName[] availableSpellNames, PlayerMovement playerMovement, SpellBarController spellBarController)
        {
            Assert.IsNotNull(popUpChoiceOfThree);
            Assert.IsNotNull(cellsAndSpellsSettings);
            Assert.IsNotNull(availableEffectNames);
            Assert.IsNotNull(playerMovement);
            Assert.IsNotNull(spellBarController);

            _popUpChoiceOfThree = popUpChoiceOfThree;
            _cellsAndSpellsSettings = cellsAndSpellsSettings;
            _availableEffectNames = availableEffectNames;
            _availableSpellNames = availableSpellNames;
            _playerMovement = playerMovement;
            _spellBarController = spellBarController;
        }

        // TODO: вынести общее в дженерики

        public void ShowEffects()
        {
            PrepareEffectButtons();
            _popUpChoiceOfThree.Show();
        }

        public void ShowSpells()
        {
            PrepareSpellButtons();
            _popUpChoiceOfThree.Show();
        }

        private void PrepareEffectButtons()
        {
            EffectName[] effects = SelectRandomEffects();
            Button[] buttons = _popUpChoiceOfThree.Buttons;

            for (var i = 0; i < buttons.Length; i++)
            {
                Button button = buttons[i];
                EffectName effectName = effects[i];
                _popUpChoiceOfThree.SetSprite(i, _cellsAndSpellsSettings.GetCellSprite(effectName));
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => PlayEffect(effectName));
            }
        }

        private void PrepareSpellButtons()
        {
            SpellName[] effects = SelectRandomSpells();
            Button[] buttons = _popUpChoiceOfThree.Buttons;

            for (var i = 0; i < buttons.Length; i++)
            {
                Button button = buttons[i];
                SpellName effectName = effects[i];
                _popUpChoiceOfThree.SetSprite(i, _cellsAndSpellsSettings.GetSpellSprite(effectName));
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => PushSpellToBar(effectName));
            }
        }

        private void PlayEffect(EffectName effectName)
        {
            _popUpChoiceOfThree.Hide();
            Debug.Log("BUTTON = Playing effect: " + effectName);
            _playerMovement.ActivateEffect(effectName);
        }

        private void PushSpellToBar(SpellName spellName)
        {
            _spellBarController.TakeSpell(spellName);
            _popUpChoiceOfThree.Hide();
            Debug.Log("BUTTON = Take spell: " + spellName);
        }

        private EffectName[] SelectRandomEffects()
        {
            return _availableEffectNames.Shuffle().Take(AmountItems).ToArray();
        }

        private SpellName[] SelectRandomSpells()
        {
            return _availableSpellNames.Shuffle().Take(AmountItems).ToArray();
        }
    }
}