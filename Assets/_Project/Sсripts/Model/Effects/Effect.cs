using System;
using UnityEngine;

namespace _Project.Sсripts.Model.Effects
{
    public abstract class Effect
    {
        public virtual void Activate(Action onComplete)
        {
            Debug.Log($"Effect - {GetType().Name} - Activate");
        }
    }
}