using System;

namespace Sсripts
{
    public class Health
    {
        private int _value;

        public Health(int value)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Health must be greater than zero");

            _value = value;
        }

        // TODO: ??? а как будем различать это Игрок или Противник прислал? 
        public event Action<int> HealthChanged;
        public event Action Died;

        public bool IsAlive => _value > 0;

        public void TakeDamage(int damage)
        {
            if (damage <= 0)
                throw new ArgumentOutOfRangeException(nameof(damage), "Damage must be greater than zero");

            _value -= damage;
            HealthChanged?.Invoke(_value);

            if (_value <= 0)
                Died?.Invoke();
        }
    }
}