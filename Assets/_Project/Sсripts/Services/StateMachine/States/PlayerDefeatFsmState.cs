using _Project.Sсripts.UI.PopUpNotification;
using UnityEngine.Assertions;

namespace _Project.Sсripts{
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

            // TODO: set new state - game over
            // _popUpController.OnRestartGameSelected += () => FiniteStateMachine.SetState<>();
            _popUpController.Show();
        }

        public override void Exit()
        {
            base.Exit();
            _popUpController.Hide();
        }
    }
}