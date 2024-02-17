using System;
using _Project.Sсripts.Scriptable;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Sсripts.UI
{
    public class SpellBarController
    {
        private readonly SpellBar _spellBar;
        private readonly CellsAndSpellsSettings _cellsAndSpellsSettings;

        public SpellBarController(SpellBar spellBar, CellsAndSpellsSettings cellsAndSpellsSettings)
        {
            Assert.IsNotNull(spellBar);
            Assert.IsNotNull(cellsAndSpellsSettings);
            
            _spellBar = spellBar;
            _cellsAndSpellsSettings = cellsAndSpellsSettings;
            
            _spellBar.Clean(_cellsAndSpellsSettings.GetSpellSprite(SpellName.Empty));
            _spellBar.Disable();
        }

        public event Action Took;

        public void TakeSpell(SpellName spellName)
        {
            Sprite sprite = _cellsAndSpellsSettings.GetSpellSprite(spellName);
            int manaCost = _cellsAndSpellsSettings.GetSpellManaCost(spellName);

            _spellBar.AddSpell(spellName, sprite, manaCost);
            
            // TODO: возможно как то можно красивее завершить ход 
            Took?.Invoke();
        }
    }
}