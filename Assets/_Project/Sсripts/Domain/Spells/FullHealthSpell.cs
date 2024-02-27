using System;
using _Project.Sсripts.Controllers.Sound;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Domain.Spells
{
    public class FullHealthSpell : Spell
    {
        private readonly Health _health;

        public FullHealthSpell(GameSounds gameSounds, Health health) : base(gameSounds)
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