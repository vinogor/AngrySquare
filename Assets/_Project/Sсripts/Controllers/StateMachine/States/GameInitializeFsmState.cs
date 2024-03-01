using _Project.Sсripts.SDK;

namespace _Project.Sсripts.Controllers.StateMachine.States
{
    public class GameInitializeFsmState : FsmState
    {
        private readonly Advertising _advertising;

        public GameInitializeFsmState(FiniteStateMachine finiteStateMachine, Advertising advertising) : base(
            finiteStateMachine)
        {
            _advertising = advertising;
        }

        public override async void Enter()
        {
            base.Enter();

            await _advertising.ShowAd();

            GoToNextState();
        }

        private void GoToNextState()
        {
            FiniteStateMachine.SetState<PlayerTurnSpellFsmState>();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}