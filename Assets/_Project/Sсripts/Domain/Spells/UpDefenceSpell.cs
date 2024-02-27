using System;
using _Project.Sсripts.Config;
using _Project.Sсripts.Controllers.Sound;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Domain.Spells
{
    public class UpDefenceSpell : Spell
    {
        private readonly Defence _defence;
        private readonly Coefficients _coefficients;

        public UpDefenceSpell(GameSounds gameSounds, Defence defence, Coefficients coefficients) : base(gameSounds)
        {
            Assert.IsNotNull(defence);
            Assert.IsNotNull(coefficients);

            _defence = defence;
            _coefficients = coefficients;
        }

        protected override void Execute(Action onComplete)
        {
            _defence.Increase(_coefficients.DefenceIncreaseValue);
            onComplete.Invoke();
        }
    }
}