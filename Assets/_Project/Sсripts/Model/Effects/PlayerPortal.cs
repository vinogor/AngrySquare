using System;

namespace _Project.Sсripts.Model.Effects
{
    public class PlayerPortal : Effect
    {
        
        // TODO: надо знать ссылки на обе клетки с порталами 
        
        public override void Activate(Action onComplete)
        {
            base.Activate(onComplete);

            onComplete.Invoke();
        }
    }
}