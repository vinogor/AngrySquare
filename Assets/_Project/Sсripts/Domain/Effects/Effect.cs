using System;
using DG.Tweening;
using UnityEngine;

namespace Domain.Effects{
    public abstract class Effect
    {
        protected Sequence Sequence;
        
        public void Activate(Action onComplete)
        {
            Debug.Log($"Effect - {GetType().Name} - Activate");
            Execute(onComplete);
        }

        protected abstract void Execute(Action onComplete);

        public void ForceStop()
        {
            Sequence.Kill();
        }
    }
}