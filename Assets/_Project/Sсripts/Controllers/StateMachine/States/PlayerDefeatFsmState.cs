using _Project.Sсripts.Controllers.Sound;
using _Project.Sсripts.Services;
using UnityEngine.Assertions;

namespace _Project.Sсripts.Controllers.StateMachine.States
{
    public class PlayerDefeatFsmState : FsmState
    {
        private readonly PopUpNotificationController _popUpController;
        private readonly LevelRestarter _levelRestarter;
        private readonly GameSounds _gameSounds;

        public PlayerDefeatFsmState(FiniteStateMachine finiteStateMachine,
            PopUpNotificationController popUpController, LevelRestarter levelRestarter, GameSounds gameSounds) :
            base(finiteStateMachine)
        {
            Assert.IsNotNull(popUpController);
            Assert.IsNotNull(levelRestarter);
            Assert.IsNotNull(gameSounds);
            _popUpController = popUpController;
            _levelRestarter = levelRestarter;
            _gameSounds = gameSounds;
        }

        public override void Enter()
        {
            base.Enter();
            _gameSounds.PlayPlayerDefeat();
            _popUpController.OnClose += Handle;
            _popUpController.Show();
        }

        private void Handle()
        {
            _levelRestarter.RestartAfterDefeat();
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