using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _Project.Sсripts.Model.Effects.Player;
using _Project.Sсripts.Scriptable;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Sсripts.UI.PopupChoice
{
    public class PopUpChoiceSpellController : PopUpChoiceAbstractController
    {
        private readonly List<SpellName> _availableSpellNames;
        private readonly SpellBarController _spellBarController;
        private readonly SpellsSettings _spellsSettings;
        
        private bool _isSpellSelected;

        public PopUpChoiceSpellController(PopUpChoice popUpChoice, List<SpellName> availableSpellNames,
            SpellBarController spellBarController, SpellsSettings spellsSettings) : base(popUpChoice)
        {
            Assert.IsNotNull(availableSpellNames);
            Assert.IsNotNull(spellBarController);
            Assert.IsNotNull(spellsSettings);

            _availableSpellNames = availableSpellNames;
            _spellBarController = spellBarController;
            _spellsSettings = spellsSettings;
        }

        public IEnumerator ShowSpells()
        {
            PrepareSpellButtons();
            PopUpChoice.Show();
            
            yield return new WaitUntil(() => _isSpellSelected);
        }
        
        public async Task ShowSpells1()
        {
            PrepareSpellButtons();  
            PopUpChoice.Show();
            
            await TaskUtils.WaitUntil(() => _isSpellSelected);
        }

        private void PrepareSpellButtons()
        {
            _isSpellSelected = false;
            List<SpellName> spellNames = SelectRandomItems(_availableSpellNames);
            Sprite[] sprites = spellNames.Select(name => _spellsSettings.GetSprite(name)).ToArray();
            PopUpChoice.SetSprites(sprites);

            for (var i = 0; i < PopUpChoice.ButtonsOnClick.Length; i++)
            {
                SpellName effectName = spellNames[i];
                PopUpChoice.ButtonsOnClick[i].AddListener(() => PushSpellToBar(effectName));
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