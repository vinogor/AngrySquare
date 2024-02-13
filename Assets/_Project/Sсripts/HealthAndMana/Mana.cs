using System;
using UnityEngine;

namespace _Project.Sсripts.HealthAndMana
{
    public class Mana
    {
        public event Action<int> Changed;
        public event Action ManaReplenished;

        public Mana(int value, int maxValue)
        {
            Validate(value, maxValue);
            Value = value;
            MaxValue = maxValue;
        }

        public int Value { get; private set; }

        public int MaxValue { get; private set; }

        public bool IsEnough(int spellCost)
        {
            return Value >= spellCost;
        }

        public void Spend(int spellCost)
        {
            Debug.Log("Spend mana " + spellCost);

            if (spellCost <= 0)
                throw new ArgumentOutOfRangeException(nameof(spellCost), "SpellCost must be greater than zero");

            if (IsEnough(spellCost) == false)
            {
                throw new ArgumentOutOfRangeException(nameof(spellCost), "Not enough mana to spend");
            }

            Value -= spellCost;
            Changed?.Invoke(Value);
        }

        public void ReplenishToMax()
        {
            if (Value == MaxValue)
                return;

            Value = MaxValue;

            ManaReplenished?.Invoke();
            Changed?.Invoke(Value);
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