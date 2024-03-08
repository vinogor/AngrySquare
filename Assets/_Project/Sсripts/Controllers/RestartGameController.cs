using System;
using Controllers.StateMachine;
using Controllers.StateMachine.States;
using Services;
using View;

namespace Controllers
{
    public class RestartGameController : IDisposable
    {
        private readonly RestartGameView _restartGameView;
        private readonly LevelRestarter _levelRestarter;
        private readonly FiniteStateMachine _finiteStateMachine;

        public RestartGameController(RestartGameView restartGameView, LevelRestarter levelRestarter,
            FiniteStateMachine finiteStateMachine)
        {
            _restartGameView = restartGameView;
            _levelRestarter = levelRestarter;
            _finiteStateMachine = finiteStateMachine;

            _restartGameView.ButtonClickedEvent.AddListener(OnClick);
        }

        private void OnClick()
        {
            _levelRestarter.RestartAfterDefeat();
            _finiteStateMachine.SetState<GameInitializeFsmState>();
        }

        public void Dispose() => _restartGameView.ButtonClickedEvent.RemoveListener(OnClick);
    }
}