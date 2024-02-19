using System;
using _Project.Sсripts.Model.Effects;
using UnityEngine;

namespace _Project.Sсripts.Model.Spells
{
    public abstract class Spell : Effect
    {
        public override void Activate(Action onComplete)
        {
            Debug.Log($"Spell - {GetType().Name} - Activate");
            Execute(onComplete);
        }
    }
}