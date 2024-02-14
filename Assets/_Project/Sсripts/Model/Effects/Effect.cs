using System;
using UnityEngine;

namespace _Project.Sсripts.Model.Effects
{
    public abstract class Effect
    {
        public abstract void Activate(Action onComplete);

        protected void Log()
        {
            Debug.Log($"Effect - {GetType().Name} - Activate");
        }
    }
}