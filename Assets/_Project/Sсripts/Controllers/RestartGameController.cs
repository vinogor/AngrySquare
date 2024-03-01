using System;
using _Project.Sсripts.Controllers.StateMachine;
using _Project.Sсripts.Controllers.StateMachine.States;
using _Project.Sсripts.Services;
using _Project.Sсripts.View;

namespace _Project.Sсripts.Controllers
{
    public class RestartGameController : IDisposable
    {
        private readonly RestartGameView _restartGameView;
        private readonly LevelRestarter _levelRestarter;
        private readonly FiniteStateMachine _finiteStateMachine;
        private readonly SaveService _saveService;

        public RestartGameController(RestartGameView restartGameView, LevelRestarter levelRestarter, FiniteStateMachine finiteStateMachine, SaveService saveService)
        {
            _restartGameView = restartGameView;
            _levelRestarter = levelRestarter;
            _finiteStateMachine = finiteStateMachine;
            _saveService = saveService;

            _restartGameView.ButtonClickedEvent.AddListener(OnClick);
        }
        
        private void OnClick()
        {
            _levelRestarter.RestartAfterDefeat();
            _finiteStateMachine.SetState<GameInitializeFsmState>();
            _saveService.Save();
        }

        public void Dispose() => _restartGameView.ButtonClickedEvent.RemoveListener(OnClick);
    }
}