using System;
using _Project.Config;
using _Project.Domain.Effects;
using UnityEngine.Assertions;

namespace _Project.Domain.Spells
{
    public class UpDefenceSpell : Effect
    {
        private readonly Defence _defence;
        private readonly Coefficients _coefficients;

        public UpDefenceSpell(Defence defence, Coefficients coefficients)
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