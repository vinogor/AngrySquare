using System;
using UnityEngine;

namespace Domain.Effects{
    public abstract class Effect
    {
        public virtual void Activate(Action onComplete)
        {
            Debug.Log($"Effect - {GetType().Name} - Activate");
            Execute(onComplete);
        }

        protected abstract void Execute(Action onComplete);
    }
}