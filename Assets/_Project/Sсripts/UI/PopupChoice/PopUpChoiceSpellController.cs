using System.Linq;
using _Project.Sсripts.Scriptable;
using _Project.Sсripts.Utility;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace _Project.Sсripts.UI.PopupChoice
{
    public class PopUpChoiceSpellController : PopUpChoiceAbstractController
    {
        private readonly SpellName[] _availableSpellNames;
        private readonly SpellBarController _spellBarController;
        private readonly SpellsSettings _spellsSettings;

        public PopUpChoiceSpellController(PopUpChoice popUpChoice, SpellName[] availableSpellNames,
            SpellBarController spellBarController, SpellsSettings spellsSettings) : base(popUpChoice)
        {
            Assert.IsNotNull(availableSpellNames);
            Assert.IsNotNull(spellBarController);
            Assert.IsNotNull(spellsSettings);

            _availableSpellNames = availableSpellNames;
            _spellBarController = spellBarController;
            _spellsSettings = spellsSettings;
        }

        public void ShowSpells()
        {
            PrepareSpellButtons();
            PopUpChoice.Show();
        }

        private void PrepareSpellButtons()
        {
            SpellName[] effectNames = SelectRandomSpells();
            Button[] buttons = PopUpChoice.Buttons;
            Sprite[] sprites = effectNames.Select(name => _spellsSettings.GetSprite(name)).ToArray();
            PopUpChoice.SetSprites(sprites);

            for (var i = 0; i < buttons.Length; i++)
            {
                Button button = buttons[i];
                SpellName effectName = effectNames[i];
                button.onClick.AddListener(() => PushSpellToBar(effectName));
            }
        }

        private void PushSpellToBar(SpellName spellName)
        {
            HidePopup();
            _spellBarController.TakeSpell(spellName);
        }

        private SpellName[] SelectRandomSpells()
        {
            return _availableSpellNames.Shuffle().Take(AmountItems).ToArray();
        }
    }
}