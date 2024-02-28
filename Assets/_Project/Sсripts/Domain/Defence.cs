using System;

namespace _Project.Sсripts.Domain
{
    public class Defence
    {
        private readonly int _defaultValue;

        public Defence(int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "value cant be less then 0");
            _defaultValue = value;
            Value = value;
        }

        public event Action Changed;

        public int Value { get; set; }

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
    }
}