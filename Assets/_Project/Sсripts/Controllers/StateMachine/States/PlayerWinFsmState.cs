using System;
using _Project.Services;
using UnityEngine.Assertions;

namespace _Project.Controllers.StateMachine.States
{
    public class PlayerWinFsmState : FsmState
    {
        private readonly PopUpNotificationController _popUpController;
        private readonly LevelRestarter _levelRestarter;

        public PlayerWinFsmState(FiniteStateMachine finiteStateMachine,
            PopUpNotificationController popUpController, LevelRestarter levelRestarter) : base(finiteStateMachine)
        {
            Assert.IsNotNull(finiteStateMachine);
            Assert.IsNotNull(popUpController);
            Assert.IsNotNull(levelRestarter);
            _popUpController = popUpController;
            _levelRestarter = levelRestarter;
        }

        public event Action Win;

        public override void Enter()
        {
            base.Enter();
            Win?.Invoke();
            _popUpController.OnClose += Handle;
            _popUpController.Show();
        }

        private void Handle()
        {
            _levelRestarter.RestartAfterWin();
            FiniteStateMachine.SetState<PlayerTurnSpellFsmState>();
        }

        public override void Exit()
        {
            base.Exit();
            _popUpController.OnClose -= Handle;
            _popUpController.Hide();
        }
    }
}