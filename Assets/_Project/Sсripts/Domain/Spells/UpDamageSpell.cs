using System;
using _Project.Sсripts.Config;
using _Project.Sсripts.Controllers.Sound;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Domain.Spells
{
    public class UpDamageSpell : Spell
    {
        private readonly Damage _damage;
        private readonly Coefficients _coefficients;

        public UpDamageSpell(GameSounds gameSounds, Damage damage, Coefficients coefficients) : base(gameSounds)
        {
            Assert.IsNotNull(damage);
            Assert.IsNotNull(coefficients);

            _damage = damage;
            _coefficients = coefficients;
        }

        protected override void Execute(Action onComplete)
        {
            _damage.Increase(_coefficients.DamageIncreaseValue);
            onComplete.Invoke();
        }
    }
}