using _Project.Controllers.Sound;
using Agava.WebUtility;
using UnityEngine;

namespace _Project.SDK
{
    public class FocusTracking : MonoBehaviour
    {
        private GameSounds _gameSounds;

        public void Initialize(GameSounds gameSounds)
        {
            _gameSounds = gameSounds;
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
            MuteAudio(inApp);
        }

        private void OnInBackgroundChangeWeb(bool inBackground)
        {
            Debug.Log("FocusTracking - OnInBackgroundChangeWeb - " + inBackground);
            MuteAudio(!inBackground);
        }

        private void MuteAudio(bool value)
        {
            _gameSounds.SwitchByFocus(value);
        }
    }
}