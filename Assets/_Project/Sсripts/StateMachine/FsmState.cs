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
        }
        
        public virtual void Exit()
        {
        }
    }
}