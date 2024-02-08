using System;
using UnityEngine;

namespace Sсripts.Hp
{
    public class Health : MonoBehaviour
    {
        public event Action<int> HealthChanged;
        public event Action Died;

        [field: SerializeField] public int Value { get; private set; }
        [field: SerializeField] public int MaxValue { get; private set; }

        [SerializeField] private ParticleSystem _particleSystem;

        public bool IsAlive => Value > 0;

        private void OnValidate()
        {
            if (Value < 0)
                Value = 0;

            if (Value > MaxValue)
                Value = MaxValue;

            if (MaxValue <= 1)
                MaxValue = 1;
        }

        public void TakeDamage(int damage)
        {
            Debug.Log("TakeDamage " + damage);

            if (damage <= 0)
                throw new ArgumentOutOfRangeException(nameof(damage), "Damage must be greater than zero");

            Value -= damage;

            _particleSystem.Play();

            if (Value <= 0)
                Value = 0;

            HealthChanged?.Invoke(Value);

            if (Value <= 0)
                Died?.Invoke();
        }

        public void ReplenishToMax()
        {
            Value = MaxValue;
            HealthChanged?.Invoke(Value);
        }
    }
}