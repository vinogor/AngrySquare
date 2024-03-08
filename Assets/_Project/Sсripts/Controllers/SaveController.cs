using Controllers.StateMachine;
using NaughtyAttributes;
using Services.Save;
using UnityEngine;

namespace Controllers
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
        
        [Button]
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
            Debug.Log("SaveController - OnStateChanged - _saveService.Save()");
            _saveService.Save();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                Debug.Log("SaveController - OnApplicationPause - _saveService.Save()");
                _saveService.Save();
            }
                
        }

        private void OnApplicationQuit()
        {
            Debug.Log("SaveController - OnApplicationQuit - _saveService.Save()");
            _saveService.Save();
        }
    }
}