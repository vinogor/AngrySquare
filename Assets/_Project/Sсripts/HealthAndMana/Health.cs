using System;
using _Project.Sсripts.DamageAndDefence;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Sсripts.HealthAndMana
{
    public class Health
    {
        private readonly Defence _defence;

        public Health(int value, int maxValue, Defence defence)
        {
            Validate(value, maxValue);
            Assert.IsNotNull(defence);
            Value = value;
            MaxValue = maxValue;
            _defence = defence;
        }

        public event Action<int> Changed;
        public event Action DamageReceived;
        public event Action Replenished;
        public event Action Died;

        public int Value { get; private set; }

        public int MaxValue { get; private set; }

        public bool IsAlive => Value > 0;

        public void TakeDamage(int damage)
        {
            Debug.Log("TakeDamage " + damage);

            if (damage <= 0)
                throw new ArgumentOutOfRangeException(nameof(damage), "Damage must be greater than zero");

            int passedDamage = damage - _defence.Value;

            if (passedDamage <= 0)
                return;

            Value -= passedDamage;

            if (Value <= 0)
                Value = 0;

            Changed?.Invoke(Value);
            DamageReceived?.Invoke();

            if (Value <= 0)
                Died?.Invoke();
        }

        public void ReplenishToMax()
        {
            if (Value == MaxValue)
                return;

            Value = MaxValue;

            Replenished?.Invoke();
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