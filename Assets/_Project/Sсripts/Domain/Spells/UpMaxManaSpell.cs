using System;
using Config;
using Domain.Effects;
using UnityEngine.Assertions;

namespace Domain.Spells
{
    public class UpMaxManaSpell : Effect
    {
        private readonly Mana _mana;
        private readonly Coefficients _coefficients;

        public UpMaxManaSpell(Mana mana, Coefficients coefficients)
        {
            Assert.IsNotNull(mana);
            Assert.IsNotNull(coefficients);
            _mana = mana;
            _coefficients = coefficients;
        }

        protected override void Execute(Action onComplete)
        {
            _mana.IncreaseMaxValue(_coefficients.MaxManaIncreaseValue);
            onComplete.Invoke();
        }
    }
}