using _Project.Sсripts.Controllers;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Services.StateMachine.States
{
    public class PlayerDefeatFsmState : FsmState
    {
        private readonly PopUpNotificationController _popUpController;

        public PlayerDefeatFsmState(FiniteStateMachine finiteStateMachine,
            PopUpNotificationController popUpController) :
            base(finiteStateMachine)
        {
            Assert.IsNotNull(popUpController);
            _popUpController = popUpController;
        }

        public override void Enter()
        {
            base.Enter();
            _popUpController.OnClose += NextState;

            _popUpController.Show();
        }

        private void NextState()
        {
            FiniteStateMachine.SetState<RestartFsmState>();
        }

        public override void Exit()
        {
            base.Exit();
            _popUpController.OnClose -= NextState;
            _popUpController.Hide();
        }
    }
}