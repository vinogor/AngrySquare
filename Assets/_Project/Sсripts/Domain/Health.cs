using System;
using _Project.Controllers.Sound;
using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Domain
{
    public class Health
    {
        private readonly Defence _defence;
        private readonly GameSounds _gameSounds;

        private readonly int _defaultValue;
        private readonly int _defaultMaxValue;

        public Health(int value, int maxValue, Defence defence, GameSounds gameSounds)
        {
            Validate(value, maxValue);
            Assert.IsNotNull(defence);
            Assert.IsNotNull(gameSounds);
            Value = value;
            MaxValue = maxValue;
            _defence = defence;
            _defaultValue = value;
            _defaultMaxValue = maxValue;
            _gameSounds = gameSounds;
        }

        public event Action<int> ValueChanged;
        public event Action<int> MaxValueChanged;
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

            _gameSounds.PlaySwordsAttack();
            Value -= passedDamage;

            if (Value <= 0)
                Value = 0;

            ValueChanged?.Invoke(Value);
            DamageReceived?.Invoke();

            if (Value <= 0)
                Died?.Invoke();
        }

        public void TakeTripleDamage(int damage)
        {
            TakeDamage(damage);
            TakeDamage(damage);
            TakeDamage(damage);
        }

        public void ReplenishToMax()
        {
            if (Value == MaxValue)
                return;

            Value = MaxValue;

            _gameSounds.PlayHealthReplenish();
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