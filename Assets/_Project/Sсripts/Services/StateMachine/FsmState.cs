using UnityEngine;
using UnityEngine.Assertions;

namespace _Project.Sсripts{
    public abstract class FsmState
    {
        protected readonly FiniteStateMachine FiniteStateMachine;

        protected FsmState(FiniteStateMachine finiteStateMachine)
        {
            Assert.IsNotNull(finiteStateMachine);
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