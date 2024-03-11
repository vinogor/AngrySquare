using System;
using Controllers.Sound;
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
        private readonly SpellBarFameScaler _spellBarFameScaler;
        private readonly GameSoundsPresenter _gameSoundsPresenter;

        private Action _onSpellCompleted;

        public SpellBarController(AvailableSpells availableSpells, SpellBarView spellBarView, Mana mana,
            SpellActivator spellActivator, SpellBarFameScaler spellBarFameScaler, GameSoundsPresenter gameSoundsPresenter)
        {
            Assert.IsNotNull(availableSpells);
            Assert.IsNotNull(spellBarView);
            Assert.IsNotNull(mana);
            Assert.IsNotNull(spellActivator);
            Assert.IsNotNull(spellBarFameScaler);
            Assert.IsNotNull(gameSoundsPresenter);

            _availableSpells = availableSpells;
            _spellBarView = spellBarView;
            _mana = mana;
            _spellActivator = spellActivator;
            _spellBarFameScaler = spellBarFameScaler;
            _gameSoundsPresenter = gameSoundsPresenter;

            _spellBarView.Disable();
            _spellBarFameScaler.Disable();
        }

        public void TakeSpell(SpellName spellName)
        {
            _availableSpells.Add(spellName);
        }

        public void Enable(Action onSpellCompleted)
        {
            _onSpellCompleted = onSpellCompleted;
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
                _onSpellCompleted?.Invoke();
                return;
            }

            _spellBarFameScaler.Enable();
            _spellBarView.SpellsActivated += OnSpellActivated;
            _spellBarView.Skipped += OnSpellSkipped;
            _spellBarView.Enable();
        }

        public void Disable()
        {
            _spellBarFameScaler.Disable();
            _spellBarView.SpellsActivated -= OnSpellActivated;
            _spellBarView.Skipped -= OnSpellSkipped;
            _spellBarView.Disable();
        }

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
            _spellActivator.Activate(spellName, () => _onSpellCompleted?.Invoke());
        }

        private void OnSpellSkipped()
        {
            Debug.Log("Skip spell");
            _gameSoundsPresenter.PlayClickButton();
            _onSpellCompleted?.Invoke();
        }
    }
}