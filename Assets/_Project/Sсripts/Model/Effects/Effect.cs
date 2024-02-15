using System;
using UnityEngine;

namespace _Project.Sсripts.Model.Effects
{
    public abstract class Effect
    {
        public void Activate(Action onComplete)
        {
            Debug.Log($"Effect - {GetType().Name} - Activate");
            Execute(onComplete);
        }

        protected abstract void Execute(Action onComplete);
    }
}