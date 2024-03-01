using _Project.Sсripts.Controllers.StateMachine;
using _Project.Sсripts.Controllers.StateMachine.States;
using _Project.Sсripts.Services;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Sсripts.View
{
    public class RestartGameButton : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private LevelRestarter _levelRestarter;
        private FiniteStateMachine _finiteStateMachine;
        private SaveService _saveService;

        public void initialize(LevelRestarter levelRestarter, FiniteStateMachine finiteStateMachine,
            SaveService saveService)
        {
            _levelRestarter = levelRestarter;
            _finiteStateMachine = finiteStateMachine;
            _saveService = saveService;
            
            _button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            _levelRestarter.RestartAfterDefeat();
            _finiteStateMachine.SetState<PlayerTurnSpellFsmState>();
            _saveService.Save();
        }
    }
}