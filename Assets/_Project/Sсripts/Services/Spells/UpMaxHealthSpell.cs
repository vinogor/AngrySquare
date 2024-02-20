using System;
using _Project.Sсripts.Domain;
using _Project.Sсripts.Scriptable;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Services.Spells{
    public class UpMaxHealthSpell : Spell
    {
        private readonly Health _health;
        private readonly Coefficients _coefficients;

        public UpMaxHealthSpell(Health health, Coefficients coefficients)
        {
            Assert.IsNotNull(health);
            Assert.IsNotNull(coefficients);
            _health = health;
            _coefficients = coefficients;
        }

        protected override void Execute(Action onComplete)
        {
            _health.IncreaseMaxValue(_coefficients.MaxHealthIncreaseValue);
            onComplete.Invoke();
        }
    }
}