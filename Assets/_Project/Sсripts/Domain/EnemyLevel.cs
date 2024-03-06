using System;

namespace _Project.Domain
{
    public class EnemyLevel
    {
        private readonly int _defaultValue;

        public EnemyLevel()
        {
            _defaultValue = 1;
            Value = _defaultValue;
        }

        public event Action Changed;

        public int Value { get; private set; }

        public void Increase()
        {
            Value++;
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