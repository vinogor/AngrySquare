using UnityEngine;

namespace _Project.Sсripts.StateMachine
{
    public abstract class FsmState
    {
        protected readonly FiniteStateMachine FiniteStateMachine;

        protected FsmState(FiniteStateMachine finiteStateMachine)
        {
            FiniteStateMachine = finiteStateMachine;
        }

        public virtual void Enter()
        {
            Debug.Log($"{GetType().Name} - ENTERED");
        }
        
        public virtual void Exit()
        {
            Debug.Log($"{GetType().Name} - EXITED");
        }
    }
}