using Services;
using UnityEngine.Assertions;
using View;

namespace Controllers.Sound
{
    public class SoundButtonPresenter : IPresenter
    {
        private readonly SoundButtonView _soundButtonView;
        private readonly GameSoundsPresenter _gameSoundsPresenter;
        private bool _isEnabled;

        public SoundButtonPresenter(SoundButtonView soundButtonView, GameSoundsPresenter gameSoundsPresenter)
        {
            Assert.IsNotNull(soundButtonView);
            Assert.IsNotNull(gameSoundsPresenter);

            _soundButtonView = soundButtonView;
            _gameSoundsPresenter = gameSoundsPresenter;
        }

        public void Enable()
        {
            _isEnabled = true;
            _soundButtonView.SetOn();
            _soundButtonView.Clicked += OnButtonClick;
        }

        public void Disable()
        {
            _soundButtonView.Clicked -= OnButtonClick;
        }

        private void OnButtonClick()
        {
            _isEnabled = !_isEnabled;

            if (_isEnabled)
                _soundButtonView.SetOn();
            else
                _soundButtonView.SetOff();

            _gameSoundsPresenter.SwitchByButton(_isEnabled);
        }
    }
}