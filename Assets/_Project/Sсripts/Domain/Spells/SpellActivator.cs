using System;
using System.Collections.Generic;
using _Project.Domain.Effects;

namespace _Project.Domain.Spells
{
    public class SpellActivator
    {
        private readonly Dictionary<SpellName, Effect> _playerSpells;

        public SpellActivator(Dictionary<SpellName, Effect> playerSpells)
        {
            _playerSpells = playerSpells;
        }

        public event Action SpellCast;

        public void Activate(SpellName spellName, Action onComplete)
        {
            SpellCast?.Invoke();
            _playerSpells[spellName].Activate(onComplete);
        }
    }
}