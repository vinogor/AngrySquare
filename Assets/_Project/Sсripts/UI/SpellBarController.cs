using _Project.Sсripts.Scriptable;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Sсripts.UI
{
    public class SpellBarController
    {
        private readonly SpellBar _spellBar;
        private readonly SpellsSettings _spellsSettings;

        public SpellBarController(SpellBar spellBar, SpellsSettings spellsSettings)
        {
            Assert.IsNotNull(spellBar);
            Assert.IsNotNull(spellsSettings);

            _spellBar = spellBar;
            _spellsSettings = spellsSettings;

            _spellBar.Clean(_spellsSettings.GetSprite(SpellName.Empty));
            _spellBar.Disable();
        }

        public void TakeSpell(SpellName spellName)
        {
            Sprite sprite = _spellsSettings.GetSprite(spellName);
            int manaCost = _spellsSettings.GetManaCost(spellName);

            _spellBar.AddSpell(spellName, sprite, manaCost);
        }
    }
}