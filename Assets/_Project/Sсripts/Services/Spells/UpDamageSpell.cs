using System;
using _Project.Sсripts.Domain;
using _Project.Sсripts.Scriptable;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Services.Spells{
    public class UpDamageSpell : Spell
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