using System;
using _Project.Sсripts.Services.Effects;
using UnityEngine;

namespace _Project.Sсripts.Services.Spells{
    public abstract class Spell : Effect
    {
        public override void Activate(Action onComplete)
        {
            Debug.Log($"Spell - {GetType().Name} - Activate");
            Execute(onComplete);
        }
    }
}