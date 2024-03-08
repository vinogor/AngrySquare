using System;
using Domain;
using Domain.Spells;
using UnityEngine;
using UnityEngine.Assertions;
using View;

namespace Controllers
{
    public class SpellBarController
    {
        private readonly AvailableSpells _availableSpells;
        private readonly SpellBarView _spellBarView;
        private readonly Mana _mana;
        private readonly SpellActivator _spellActivator;
        private readonly SpellBarShaker _spellBarShaker;

        public SpellBarController(AvailableSpells availableSpells, SpellBarView spellBarView, Mana mana,
            SpellActivator spellActivator, SpellBarShaker spellBarShaker)
        {
            Assert.IsNotNull(availableSpells);
            Assert.IsNotNull(spellBarView);
            Assert.IsNotNull(mana);
            Assert.IsNotNull(spellActivator);
            Assert.IsNotNull(spellBarShaker);

            _availableSpells = availableSpells;
            _spellBarView = spellBarView;
            _mana = mana;
            _spellActivator = spellActivator;
            _spellBarShaker = spellBarShaker;

            _spellBarView.Disable();
        }

        public event Action SpellCompleted;
        public event Action SpellSkipped;

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
            SpellSkipped?.Invoke();
            SpellCompleted?.Invoke();
        }

        public void TakeSpell(SpellName spellName)
        {
            _availableSpells.Add(spellName);
        }

        public void Enable()
        {
            bool isEnoughManaForAnySpell = false;
            foreach (var spellName in _availableSpells.SpellNames)
            {
                if (_mana.IsEnough(spellName))
                {
                    isEnoughManaForAnySpell = true;
                }
            }

            if (isEnoughManaForAnySpell == false)
            {
                Debug.Log("Skip spell");
                SpellCompleted?.Invoke();
                return;
            }

            _spellBarShaker.Enable(_availableSpells.SpellNames.Count);
            _spellBarView.SpellsActivated += OnSpellActivated;
            _spellBarView.Skipped += OnSpellSkipped;
            _spellBarView.Enable();
        }

        public void Disable()
        {
            _spellBarShaker.Disable();
            _spellBarView.SpellsActivated -= OnSpellActivated;
            _spellBarView.Skipped -= OnSpellSkipped;
            _spellBarView.Disable();
        }
    }
}