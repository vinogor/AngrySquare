using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _Project.Sсripts.Config;
using _Project.Sсripts.Domain.Spells;
using _Project.Sсripts.View;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Controllers.PopupChoice
{
    public class PopUpChoiceSpellController : PopUpChoiceAbstractController
    {
        private readonly List<SpellName> _availableSpellNames;
        private readonly SpellBarController _spellBarController;
        private readonly SpellsSettings _spellsSettings;

        private bool _isSpellSelected;

        public PopUpChoiceSpellController(PopUpChoiceView popUpChoiceView, List<SpellName> availableSpellNames,
            SpellBarController spellBarController, SpellsSettings spellsSettings) : base(popUpChoiceView)
        {
            Assert.IsNotNull(availableSpellNames);
            Assert.IsNotNull(spellBarController);
            Assert.IsNotNull(spellsSettings);

            _availableSpellNames = availableSpellNames;
            _spellBarController = spellBarController;
            _spellsSettings = spellsSettings;
        }

        public async Task ShowSpells()
        {
            PrepareSpellButtons();
            PopUpChoiceView.Show();

            await UniTask.WaitUntil(() => _isSpellSelected);
        }

        private void PrepareSpellButtons()
        {
            _isSpellSelected = false;
            SpellName[] spellNames = SelectRandomItems(_availableSpellNames);
            Sprite[] sprites = spellNames.Select(name => _spellsSettings.GetSprite(name)).ToArray();
            PopUpChoiceView.SetSprites(sprites);

            for (var i = 0; i < PopUpChoiceView.ButtonsOnClick.Length; i++)
            {
                SpellName effectName = spellNames[i];
                PopUpChoiceView.ButtonsOnClick[i].AddListener(() => PushSpellToBar(effectName));
            }
        }

        private void PushSpellToBar(SpellName spellName)
        {
            HidePopup();
            _spellBarController.TakeSpell(spellName);
            _isSpellSelected = true;
        }
    }
}