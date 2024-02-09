namespace Sсripts.StateMachine
{
    public abstract class FsmState
    {
        protected readonly FiniteStateMachine _finiteStateMachine;

        protected FsmState(FiniteStateMachine finiteStateMachine)
        {
            _finiteStateMachine = finiteStateMachine;
        }

        public virtual void Enter()
        {
        }
        
        public virtual void Exit()
        {
        }
    }
}