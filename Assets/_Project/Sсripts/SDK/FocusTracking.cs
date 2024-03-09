using Agava.WebUtility;
using Controllers.Sound;
using UnityEngine;
using UnityEngine.Assertions;

namespace SDK
{
    public class FocusTracking : MonoBehaviour
    {
        private GameSoundsPresenter _gameSoundsPresenter;

        public void Initialize(GameSoundsPresenter gameSoundsPresenter)
        {
            Assert.IsNotNull(gameSoundsPresenter);
            _gameSoundsPresenter = gameSoundsPresenter;
        }

        private void OnEnable()
        {
            Application.focusChanged += OnInBackgroundChangeApp;
            WebApplication.InBackgroundChangeEvent += OnInBackgroundChangeWeb;
        }

        private void OnDisable()
        {
            Application.focusChanged -= OnInBackgroundChangeApp;
            WebApplication.InBackgroundChangeEvent -= OnInBackgroundChangeWeb;
        }

        private void OnInBackgroundChangeApp(bool inApp)
        {
            Debug.Log("FocusTracking - OnInBackgroundChangeApp - " + inApp);
            _gameSoundsPresenter.SwitchByFocus(inApp);
        }

        private void OnInBackgroundChangeWeb(bool inBackground)
        {
            Debug.Log("FocusTracking - OnInBackgroundChangeWeb - " + inBackground);
            _gameSoundsPresenter.SwitchByFocus(!inBackground);
        }
    }
}