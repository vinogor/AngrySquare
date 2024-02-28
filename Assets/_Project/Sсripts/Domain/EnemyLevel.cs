using System;

namespace _Project.Sсripts.Domain
{
    public class EnemyLevel
    {
        private readonly int _defaultValue;

        public EnemyLevel()
        {
            Value = 1;
            _defaultValue = Value;
        }

        public event Action Increased;
        public event Action SetDefault;

        public int Value { get; set; }

        public void Increase()
        {
            Value++;
            Increased?.Invoke();
        }

        public void SetToDefault()
        {
            Value = _defaultValue;
            SetDefault?.Invoke();
        }
    }
}