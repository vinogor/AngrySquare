using System;
using _Project.Sсripts.Config;
using _Project.Sсripts.Controllers.Sound;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Domain.Spells
{
    public class UpMaxHealthSpell : Spell
    {
        private readonly Health _health;
        private readonly Coefficients _coefficients;

        public UpMaxHealthSpell(GameSounds gameSounds, Health health, Coefficients coefficients) : base(gameSounds)
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