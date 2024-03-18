using Controllers.Sound;
using Services;
using UnityEngine.Assertions;

namespace Controllers.StateMachine.States
{
    public class PlayerWinFsmState : FsmState
    {
        private readonly PopUpNotificationController _popUpPlayerWin;
        private readonly LevelRestarter _levelRestarter;
        private readonly GameSoundsPresenter _gameSoundsPresenter;

        public PlayerWinFsmState(FiniteStateMachine finiteStateMachine,
            PopUpNotificationController popUpPlayerWin, LevelRestarter levelRestarter,
            GameSoundsPresenter gameSoundsPresenter) : base(finiteStateMachine)
        {
            Assert.IsNotNull(finiteStateMachine);
            Assert.IsNotNull(popUpPlayerWin);
            Assert.IsNotNull(levelRestarter);
            Assert.IsNotNull(gameSoundsPresenter);

            _popUpPlayerWin = popUpPlayerWin;
            _levelRestarter = levelRestarter;
            _gameSoundsPresenter = gameSoundsPresenter;
        }

        public override void Enter()
        {
            base.Enter();
            _gameSoundsPresenter.PlayPlayerWin();
            _popUpPlayerWin.Show(Handle);
        }

        private void Handle()
        {
            _levelRestarter.RestartAfterWin();
            FiniteStateMachine.SetState<PlayerTurnSpellFsmState>();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}