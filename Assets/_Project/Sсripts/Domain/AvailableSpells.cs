using System;
using System.Collections.Generic;
using Domain.Spells;
using UnityEngine;

namespace Domain
{
    public class AvailableSpells
    {
        private List<SpellName> _spellNames = new();

        private readonly int _maxSpellBarSize = 5;

        public event Action Updated;
        public List<SpellName> SpellNames => _spellNames;

        public bool IsEmpty => _spellNames.Count == 0;

        public void Add(SpellName spellName)
        {
            _spellNames.Insert(0, spellName);
            Debug.Log($"add spellName {spellName}, (size _spellNames = {_spellNames.Count})");

            if (_spellNames.Count > _maxSpellBarSize)
                _spellNames.RemoveAt(_spellNames.Count - 1);

            Updated?.Invoke();
        }

        public void Remove(int spellIndex)
        {
            Debug.Log($"try remove spell (size _spellNames = {_spellNames.Count}) by index {spellIndex}");
            _spellNames.RemoveAt(spellIndex);
            Updated?.Invoke();
        }

        public void Clear()
        {
            _spellNames = new();
            Updated?.Invoke();
        }
    }
}