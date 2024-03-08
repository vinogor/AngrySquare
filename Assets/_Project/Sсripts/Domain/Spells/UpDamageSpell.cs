using System;
using Config;
using Domain.Effects;
using UnityEngine.Assertions;

namespace Domain.Spells
{
    public class UpDamageSpell : Effect
    {
        private readonly Damage _damage;
        private readonly Coefficients _coefficients;

        public UpDamageSpell(Damage damage, Coefficients coefficients)
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