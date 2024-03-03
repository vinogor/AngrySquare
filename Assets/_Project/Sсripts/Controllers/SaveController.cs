using _Project.Sсripts.Controllers.StateMachine;
using _Project.Sсripts.Services.Save;
using NaughtyAttributes;
using UnityEngine;

namespace _Project.Sсripts.Controllers
{
    public class SaveController : MonoBehaviour
    {
        private SaveService _saveService;
        private FiniteStateMachine _finiteStateMachine;

        public void Initialize(SaveService saveService, FiniteStateMachine finiteStateMachine)
        {
            _saveService = saveService;
            _finiteStateMachine = finiteStateMachine;
            _finiteStateMachine.StateChanged += OnStateChanged;
        }

        [Button]
        private void Load()
        {
            _saveService.Load(() =>
            {
                Debug.Log("Loaded");
            });
        }

        [Button]
        private void Save()
        {
            _saveService.Save();
        }
        
        private void Clean()
        {
            _saveService.Clean();
        }

        private void OnDestroy()
        {
            _finiteStateMachine.StateChanged -= OnStateChanged;
        }

        private void OnStateChanged()
        {
            _saveService.Save();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
                _saveService.Save();
        }

        private void OnApplicationQuit()
        {
            _saveService.Save();
        }
    }
}