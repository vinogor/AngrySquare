using System;

namespace _Project.Sсripts.Model.Effects
{
    public class EnemyPortal : Effect
    {
        public override void Activate(Action onComplete)
        {
            base.Activate(onComplete);

            onComplete.Invoke();
        }
    }
}