using System;
using _Project.Sсripts.Config;
using _Project.Sсripts.Controllers.Sound;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Domain.Spells
{
    public class UpMaxManaSpell : Spell
    {
        private readonly Mana _mana;
        private readonly Coefficients _coefficients;

        public UpMaxManaSpell(GameSounds gameSounds, Mana mana, Coefficients coefficients) : base(gameSounds)
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