using System;

namespace Domain
{
    public class Damage
    {
        private readonly int _defaultValue;

        public Damage(int value)
        {
            if (value < 1)
                throw new ArgumentOutOfRangeException(nameof(value), "value cant be less then 1");

            Value = value;
            _defaultValue = value;
        }

        public event Action Changed;

        public int Value { get; private set; }

        public void Increase(int increaseValue)
        {
            if (increaseValue < 1)
                throw new ArgumentOutOfRangeException(nameof(increaseValue), "value cant be less then 1");

            Value += increaseValue;
            Changed?.Invoke();
        }

        public void SetToDefault()
        {
            Value = _defaultValue;
            Changed?.Invoke();
        }

        public void SetNewValue(int value)
        {
            Value = value;
            Changed?.Invoke();
        }
    }
}