using System;
using UnityEngine;

namespace Sсripts
{
    // TODO: ??? сейчас пока все классы наследуются от MonoBehaviour ?
    public class Health : MonoBehaviour
    {
        public Health(int value)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Health must be greater than zero");

            Value = value;
        }

        // TODO: ??? а как будем различать это Игрок или Противник прислал? 
        public event Action<int> HealthChanged;
        public event Action Died;

        public int Value { get; private set; }

        public bool IsAlive => Value > 0;

        public void TakeDamage(int damage)
        {
            if (damage <= 0)
                throw new ArgumentOutOfRangeException(nameof(damage), "Damage must be greater than zero");

            Value -= damage;

            if (Value <= 0)
                Value = 0;

            HealthChanged?.Invoke(Value);

            if (Value <= 0)
                Died?.Invoke();
        }
    }
}