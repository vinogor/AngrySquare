using System;
using UnityEngine;

namespace _Project.Sсripts{
    public abstract class Spell : Effect
    {
        public override void Activate(Action onComplete)
        {
            Debug.Log($"Spell - {GetType().Name} - Activate");
            Execute(onComplete);
        }
    }
}