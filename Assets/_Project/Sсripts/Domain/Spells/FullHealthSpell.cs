using System;
using Domain.Effects;
using UnityEngine.Assertions;

namespace Domain.Spells
{
    public class FullHealthSpell : Effect
    {
        private readonly Health _health;

        public FullHealthSpell(Health health)
        {
            Assert.IsNotNull(health);
            _health = health;
        }

        protected override void Execute(Action onComplete)
        {
            _health.ReplenishToMax();
            onComplete.Invoke();
        }
    }
}