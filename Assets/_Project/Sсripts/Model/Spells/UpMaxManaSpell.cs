using System;
using _Project.Sсripts.HealthAndMana;
using _Project.Sсripts.Scriptable;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Model.Spells
{
    public class UpMaxManaSpell : Spell
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