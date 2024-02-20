using _Project.Sсripts.UI.PopUpNotification;
using UnityEngine.Assertions;

namespace _Project.Sсripts{
    public class PlayerWinFsmState : FsmState
    {
        private readonly PopUpNotificationController _popUpController;

        public PlayerWinFsmState(FiniteStateMachine finiteStateMachine,
            PopUpNotificationController popUpController) : base(finiteStateMachine)
        {
            Assert.IsNotNull(popUpController);
            _popUpController = popUpController;
        }

        public override void Enter()
        {
            base.Enter();

            // TODO: set new state - spawn new enemy
            // _popUpController.OnSpawnNewEnemySelected += () => FiniteStateMachine.SetState<>();
            _popUpController.Show();
        }

        public override void Exit()
        {
            base.Exit();
            _popUpController.Hide();
        }
    }
}