using System;
using System.Collections.Generic;
using _Project.Domain.Spells;

namespace _Project.Domain
{
    public class AvailableSpells
    {
        private readonly List<SpellName> _spellNames = new();

        private readonly int _maxSpellBarSize = 5;

        public event Action Updated;
        public List<SpellName> SpellNames => _spellNames;
        
        public bool IsEmpty => _spellNames.Count == 0;

        public void Add(SpellName spellName)
        {
            _spellNames.Insert(0, spellName);

            if (_spellNames.Count > _maxSpellBarSize)
                _spellNames.RemoveAt(_spellNames.Count - 1);

            Updated?.Invoke();
        }

        public void Remove(int spellIndex)
        {
            _spellNames.RemoveAt(spellIndex);
            Updated?.Invoke();
        }

        public void Clear()
        {
            _spellNames.Clear();
            Updated?.Invoke();
        }
    }
}