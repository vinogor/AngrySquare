using System.Collections.Generic;
using System.Linq;
using Config;
using Controllers.Sound;
using Cysharp.Threading.Tasks;
using Domain.Spells;
using UnityEngine;
using UnityEngine.Assertions;
using View;

namespace Controllers.PopupChoice
{
    public class PopUpChoiceSpellPresenter : PopUpChoiceAbstractPresenter
    {
        private readonly List<SpellName> _availableSpellNames;
        private readonly SpellBarController _spellBarController;
        private readonly SpellsSettings _spellsSettings;
        private readonly GameSoundsPresenter _gameSoundsPresenter;

        private bool _isSpellSelected;

        public PopUpChoiceSpellPresenter(PopUpChoiceView popUpChoiceView, List<SpellName> availableSpellNames,
            SpellBarController spellBarController, SpellsSettings spellsSettings,
            GameSoundsPresenter gameSoundsPresenter) : base(popUpChoiceView)
        {
            Assert.IsNotNull(availableSpellNames);
            Assert.IsNotNull(spellBarController);
            Assert.IsNotNull(spellsSettings);
            Assert.IsNotNull(gameSoundsPresenter);

            _availableSpellNames = availableSpellNames;
            _spellBarController = spellBarController;
            _spellsSettings = spellsSettings;
            _gameSoundsPresenter = gameSoundsPresenter;
        }

        public async UniTask ShowSpells()
        {
            PrepareSpellButtons();
            _gameSoundsPresenter.PlayPopUpShowed();
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
            _gameSoundsPresenter.PlayClickButton();
            _spellBarController.TakeSpell(spellName);
            _isSpellSelected = true;
        }
    }
}