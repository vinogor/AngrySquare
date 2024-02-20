using System;
using _Project.Sсripts.Domain;
using _Project.Sсripts.Scriptable;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Services.Spells{
    public class UpDefenceSpell : Spell
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