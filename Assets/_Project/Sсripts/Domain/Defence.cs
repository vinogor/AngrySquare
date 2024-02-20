using System;

namespace _Project.Sсripts.Domain
{
    public class Defence
    {
        public Defence(int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "value cant be less then 0");

            Value = value;
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
    }
}