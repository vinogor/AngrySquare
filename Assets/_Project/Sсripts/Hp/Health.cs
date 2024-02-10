using System;
using UnityEngine;

namespace _Project.Sсripts.Hp
{
    public class Health
    {
        public event Action<int> Changed;
        public event Action DamageReceived;
        public event Action Died;

        public Health(int value, int maxValue)
        {
            Validate(value, maxValue);
            Value = value;
            MaxValue = maxValue;
        }

        public int Value { get; private set; }

        public int MaxValue { get; private set; }

        public bool IsAlive => Value > 0;

        public void TakeDamage(int damage)
        {
            Debug.Log("TakeDamage " + damage);

            if (damage <= 0)
                throw new ArgumentOutOfRangeException(nameof(damage), "Damage must be greater than zero");

            Value -= damage;

            if (Value <= 0)
                Value = 0;

            Changed?.Invoke(Value);
            DamageReceived?.Invoke();

            if (Value <= 0)
                Died?.Invoke();
        }

        public void ReplenishToMax()
        {
            Value = MaxValue;
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