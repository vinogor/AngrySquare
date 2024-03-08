using System;
using Agava.WebUtility;
using UnityEngine;

namespace SDK
{
    public class FocusTracking : MonoBehaviour
    {
        public event Action<bool> FocusSwitched;

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
            FocusSwitched?.Invoke(inApp);
        }

        private void OnInBackgroundChangeWeb(bool inBackground)
        {
            Debug.Log("FocusTracking - OnInBackgroundChangeWeb - " + inBackground);
            FocusSwitched?.Invoke(!inBackground);
        }
    }
}