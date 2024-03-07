using System;
using _Project.Config;
using _Project.Domain.Effects;
using UnityEngine.Assertions;

namespace _Project.Domain.Spells
{
    public class UpMaxHealthSpell : Effect
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