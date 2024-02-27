using _Project.Sсripts.Controllers.Sound;
using _Project.Sсripts.View;

namespace _Project.Sсripts.Controllers
{
    public class SoundController
    {
        private readonly SoundView _soundView;
        private readonly GameSounds _gameSounds;
        private bool _isEnabled;

        public SoundController(SoundView soundView, GameSounds gameSounds)
        {
            _soundView = soundView;
            _gameSounds = gameSounds;

            _isEnabled = true;
            _soundView.ButtonOnClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            _isEnabled = !_isEnabled;

            if (_isEnabled)
                _soundView.SetOn();
            else
                _soundView.SetOff();
            
            _gameSounds.Switch(_isEnabled);
        }
        
        // TODO: remove listener ?
    }
}