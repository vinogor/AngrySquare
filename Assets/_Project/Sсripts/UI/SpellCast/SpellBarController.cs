using System;
using _Project.Sсripts.HealthAndMana;
using _Project.Sсripts.Scriptable;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Sсripts.UI.SpellCast
{
    public class SpellBarController
    {
        private readonly Spells _spells;
        private readonly SpellBarView _spellBarView;
        private readonly Mana _mana;
        private readonly SpellActivator _spellActivator;

        public SpellBarController(Spells spells, SpellBarView spellBarView, Mana mana, SpellActivator spellActivator)
        {
            Assert.IsNotNull(spells);
            Assert.IsNotNull(spellBarView);
            Assert.IsNotNull(mana);
            Assert.IsNotNull(spellActivator);

            _spells = spells;
            _spellBarView = spellBarView;
            _mana = mana;
            _spellActivator = spellActivator;

            _spellBarView.Clean();
            _spellBarView.Disable();
        }

        public event Action SpellCompleted;

        private void OnSpellActivated(int spellIndex, SpellName spellName)
        {
            Debug.Log($"OnSpellsActivated {spellName}");

            if (_mana.IsEnough(spellName) == false)
            {
                Debug.Log("Not enough mana");
                return;
            }

            _mana.Spend(spellName);
            _spells.Remove(spellIndex);
            _spellActivator.Activate(spellName, () => SpellCompleted?.Invoke());
        }

        private void OnSpellSkipped()
        {
            Debug.Log("Skip spell");
            SpellCompleted?.Invoke();
        }

        public void TakeSpell(SpellName spellName)
        {
            _spells.Add(spellName);
        }

        public void Enable()
        {
            if (_spells.IsEmpty)
            {
                Debug.Log("Skip spell");
                SpellCompleted?.Invoke();
                return;
            }
            
            _spellBarView.SpellsActivated += OnSpellActivated;
            _spellBarView.Skipped += OnSpellSkipped;
            _spellBarView.Enable();
        }

        public void Disable()
        {
            _spellBarView.SpellsActivated -= OnSpellActivated;
            _spellBarView.Skipped -= OnSpellSkipped;
            _spellBarView.Disable();
        }
    }
}