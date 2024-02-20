using System;
using _Project.Sсripts.Domain;
using _Project.Sсripts.Services.Spells;
using _Project.Sсripts.View;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Controllers
{
    public class SpellBarController
    {
        private readonly AvailableSpells _availableSpells;
        private readonly SpellBarView _spellBarView;
        private readonly Mana _mana;
        private readonly SpellActivator _spellActivator;

        public SpellBarController(AvailableSpells availableSpells, SpellBarView spellBarView, Mana mana, SpellActivator spellActivator)
        {
            Assert.IsNotNull(availableSpells);
            Assert.IsNotNull(spellBarView);
            Assert.IsNotNull(mana);
            Assert.IsNotNull(spellActivator);

            _availableSpells = availableSpells;
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
            _availableSpells.Remove(spellIndex);
            _spellActivator.Activate(spellName, () => SpellCompleted?.Invoke());
        }

        private void OnSpellSkipped()
        {
            Debug.Log("Skip spell");
            SpellCompleted?.Invoke();
        }

        public void TakeSpell(SpellName spellName)
        {
            _availableSpells.Add(spellName);
        }

        public void Enable()
        {
            if (_availableSpells.IsEmpty)
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