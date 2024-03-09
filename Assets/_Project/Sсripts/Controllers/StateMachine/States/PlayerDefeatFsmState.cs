using Controllers.Sound;
using SDK.Leader;
using Services;
using UnityEngine.Assertions;

namespace Controllers.StateMachine.States
{
    public class PlayerDefeatFsmState : FsmState
    {
        private readonly PopUpNotificationController _popUpController;
        private readonly LevelRestarter _levelRestarter;
        private readonly YandexLeaderBoard _yandexLeaderBoard;
        private readonly GameSoundsPresenter _gameSoundsPresenter;

        public PlayerDefeatFsmState(FiniteStateMachine finiteStateMachine,
            PopUpNotificationController popUpController, LevelRestarter levelRestarter,
            YandexLeaderBoard yandexLeaderBoard, GameSoundsPresenter gameSoundsPresenter) :
            base(finiteStateMachine)
        {
            Assert.IsNotNull(finiteStateMachine);
            Assert.IsNotNull(popUpController);
            Assert.IsNotNull(levelRestarter);
            Assert.IsNotNull(yandexLeaderBoard);
            Assert.IsNotNull(gameSoundsPresenter);

            _popUpController = popUpController;
            _levelRestarter = levelRestarter;
            _yandexLeaderBoard = yandexLeaderBoard;
            _gameSoundsPresenter = gameSoundsPresenter;
        }

        public override void Enter()
        {
            base.Enter();
            _gameSoundsPresenter.PlayPlayerDefeat();
            _yandexLeaderBoard.Defeat();
            _popUpController.Show(HandlePopUpClosed);
        }

        private void HandlePopUpClosed()
        {
            _levelRestarter.RestartAfterDefeat();
            FiniteStateMachine.SetState<GameInitializeFsmState>();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}