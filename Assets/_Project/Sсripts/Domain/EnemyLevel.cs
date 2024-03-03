using System;

namespace _Project.Domain
{
    public class EnemyLevel
    {
        private readonly int _defaultValue;

        public EnemyLevel()
        {
            Value = 1;
            _defaultValue = Value;
        }

        public event Action Changed;
        public event Action SetDefault;

        public int Value { get; private set; }

        public void Increase()
        {
            Value++;
            Changed?.Invoke();
        }

        public void SetToDefault()
        {
            Value = _defaultValue;
            SetDefault?.Invoke();
        }
        
        public void SetNewValue(int value)
        {
            Value = value;
            Changed?.Invoke();
        }
    }
}