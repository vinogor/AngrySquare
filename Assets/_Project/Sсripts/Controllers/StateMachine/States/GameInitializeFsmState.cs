using _Project.Sсripts.SDK;

namespace _Project.Sсripts.Controllers.StateMachine.States
{
    public class GameInitializeFsmState : FsmState
    {
        private readonly PopUpTutorialController _popUpTutorialController;
        private readonly Advertising _advertising;

        public GameInitializeFsmState(FiniteStateMachine finiteStateMachine, PopUpTutorialController popUpTutorialController, Advertising advertising) : base(
            finiteStateMachine)
        {
            _popUpTutorialController = popUpTutorialController;
            _advertising = advertising;
        }

        public override async void Enter()
        {
            base.Enter();

            await _advertising.ShowAd();

            _popUpTutorialController.Enable();
            
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