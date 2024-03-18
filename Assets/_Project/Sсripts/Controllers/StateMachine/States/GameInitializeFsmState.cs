using SDK;
using UnityEngine.Assertions;

namespace Controllers.StateMachine.States
{
    public class GameInitializeFsmState : FsmState
    {
        private readonly Advertising _advertising;

        public GameInitializeFsmState(FiniteStateMachine finiteStateMachine, Advertising advertising) : base(
            finiteStateMachine)
        {
            Assert.IsNotNull(finiteStateMachine);
            Assert.IsNotNull(advertising);
            _advertising = advertising;
        }

        public override async void Enter()
        {
            base.Enter();

            await _advertising.ShowInterstitialAd();

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