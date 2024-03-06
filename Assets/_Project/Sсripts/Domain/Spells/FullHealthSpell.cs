using System;
using UnityEngine.Assertions;

namespace _Project.Domain.Spells
{
    public class FullHealthSpell : Spell
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