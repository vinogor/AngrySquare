using System;
using View;

namespace Controllers
{
    public class SoundController : IDisposable
    {
        private readonly SoundView _soundView;
        private bool _isEnabled;

        public SoundController(SoundView soundView)
        {
            _soundView = soundView;

            _isEnabled = true;
            _soundView.ButtonOnClick.AddListener(OnButtonClick);
        }

        public event Action<bool> SwitchSound;

        public void Dispose() => _soundView.ButtonOnClick.RemoveListener(OnButtonClick);

        private void OnButtonClick()
        {
            _isEnabled = !_isEnabled;

            if (_isEnabled)
                _soundView.SetOn();
            else
                _soundView.SetOff();

            SwitchSound?.Invoke(_isEnabled);
        }
    }
}