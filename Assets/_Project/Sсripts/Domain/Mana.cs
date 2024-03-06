using System;
using _Project.Config;
using _Project.Domain.Spells;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Domain
{
    public class Mana
    {
        private readonly SpellsSettings _spellsSettings;
        private readonly int _defaultValue;
        private readonly int _defaultMaxValue;
        public event Action<int> ValueChanged;
        public event Action<int> MaxValueChanged;
        public event Action Replenished;

        public Mana(int value, int maxValue, SpellsSettings spellsSettings)
        {
            Validate(value, maxValue);
            Assert.IsNotNull(spellsSettings);
            _spellsSettings = spellsSettings;
            Value = value;
            MaxValue = maxValue;
            _defaultValue = value;
            _defaultMaxValue = maxValue;
        }

        public int Value { get; private set; }

        public int MaxValue { get; private set; }

        public bool IsEnough(SpellName spellName)
        {
            int spellCost = _spellsSettings.GetManaCost(spellName);
            return Value >= spellCost;
        }

        public void Spend(SpellName spellName)
        {
            int spellCost = _spellsSettings.GetManaCost(spellName);

            Debug.Log("Spend mana " + spellCost);

            if (spellCost <= 0)
                throw new ArgumentOutOfRangeException(nameof(spellCost), "SpellCost must be greater than zero");

            if (IsEnough(spellName) == false)
            {
                throw new ArgumentOutOfRangeException(nameof(spellCost), "Not enough mana to spend");
            }

            Value -= spellCost;
            ValueChanged?.Invoke(Value);
        }

        public void ReplenishToMax()
        {
            if (Value == MaxValue)
                return;

            Value = MaxValue;

            Replenished?.Invoke();
            ValueChanged?.Invoke(Value);
        }

        public void IncreaseMaxValue(int increaseValue)
        {
            if (increaseValue < 1)
                throw new ArgumentOutOfRangeException(nameof(increaseValue), "value cant be less then 1");

            MaxValue += increaseValue;

            MaxValueChanged?.Invoke(MaxValue);
        }

        public void SetToDefault()
        {
            Value = _defaultValue;
            MaxValue = _defaultMaxValue;
            MaxValueChanged?.Invoke(MaxValue);
            ValueChanged?.Invoke(Value);
        }

        public void SetNewValues(int value, int maxValue)
        {
            Value = value;
            MaxValue = maxValue;
            MaxValueChanged?.Invoke(MaxValue);
            ValueChanged?.Invoke(Value);
        }

        private void Validate(int value, int maxValue)
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "value cant be 0 or less");
            }

            if (maxValue < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(maxValue), "maxValue cant be less then 1");
            }

            if (value > maxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "value cant be bigger then maxValue");
            }
        }
    }
}