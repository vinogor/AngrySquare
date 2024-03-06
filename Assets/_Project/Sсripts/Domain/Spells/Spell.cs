using System;
using _Project.Domain.Effects;
using UnityEngine;

namespace _Project.Domain.Spells
{
    public abstract class Spell : Effect
    {
        public event Action SpellCast;

        public override void Activate(Action onComplete)
        {
            Debug.Log($"Spell - {GetType().Name} - Activate");
            SpellCast?.Invoke();
            Execute(onComplete);
        }
    }
}