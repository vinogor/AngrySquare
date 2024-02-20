using System;
using System.Collections.Generic;

namespace _Project.Sсripts.Services.Spells{
    public class SpellActivator
    {
        private readonly Dictionary<SpellName, Spell> _playerSpells;

        public SpellActivator(Dictionary<SpellName, Spell> playerSpells)
        {
            _playerSpells = playerSpells;
        }

        public void Activate(SpellName spellName, Action onComplete)
        {
            _playerSpells[spellName].Activate(onComplete);
        }
    }
}