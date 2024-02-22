using System;
using _Project.Sсripts.Domain.Effects;
using UnityEngine;

namespace _Project.Sсripts.Domain.Spells{
    public abstract class Spell : Effect
    {
        public override void Activate(Action onComplete)
        {
            Debug.Log($"Spell - {GetType().Name} - Activate");
            Execute(onComplete);
        }
    }
}