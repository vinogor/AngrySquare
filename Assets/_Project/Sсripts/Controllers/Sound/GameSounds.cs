using System.Collections.Generic;
using System.Linq;
using _Project.Config;
using UnityEngine;

namespace _Project.Controllers.Sound
{
    public class GameSounds
    {
        private readonly Dictionary<SoundName, AudioClip> _sounds;
        private readonly AudioSource _audioSource;
        private readonly AudioSource _backgroundAudioSource;

        private bool _isEnabledByButton = true;
        private bool _isEnabledByAdv = true;
        private bool _isEnabledByFocus = true;

        public GameSounds(SoundSettings soundSettings, AudioSource audioSource, AudioSource backgroundAudioSource)
        {
            _sounds = soundSettings.Configs.ToDictionary(key => key.SoundName, value => value.AudioClip);
            _audioSource = audioSource;
            _backgroundAudioSource = backgroundAudioSource;

            _backgroundAudioSource.loop = true;
            _backgroundAudioSource.Play();
        }

        public void SwitchByButton(bool isEnabled)
        {
            Debug.Log("GameSounds - SwitchByButton - isEnabled = " + isEnabled);

            _isEnabledByButton = isEnabled;

            Switch(_isEnabledByButton);

            Debug.Log(
                $"GameSounds - SwitchByButton - _isEnabledByButton = {_isEnabledByButton}, _isEnabledByAdv = {_isEnabledByAdv}, _isEnabledByFocus = {_isEnabledByFocus}");
        }

        public void SwitchByAdv(bool isEnabled)
        {
            Debug.Log("GameSounds - SwitchByAdv - isEnabled = " + isEnabled);

            if (_isEnabledByButton == false)
                return;

            _isEnabledByAdv = isEnabled;

            Switch(_isEnabledByAdv);

            Debug.Log(
                $"GameSounds - SwitchByAdv - _isEnabledByButton = {_isEnabledByButton}, _isEnabledByAdv = {_isEnabledByAdv}, _isEnabledByFocus = {_isEnabledByFocus}");
        }

        public void SwitchByFocus(bool isEnabled)
        {
            Debug.Log("GameSounds - SwitchByFocus - isEnabled = " + isEnabled);

            if (_isEnabledByButton == false || _isEnabledByAdv == false)
                return;

            _isEnabledByFocus = isEnabled;

            Switch(_isEnabledByFocus);

            Debug.Log(
                $"GameSounds - SwitchByFocus - _isEnabledByButton = {_isEnabledByButton}, _isEnabledByAdv = {_isEnabledByAdv}, _isEnabledByFocus = {_isEnabledByFocus}");
        }

        private void Switch(bool isEnabled)
        {
            if (isEnabled == false)
            {
                _audioSource.Stop();
                _backgroundAudioSource.Pause();
            }
            else
            {
                _backgroundAudioSource.UnPause();
            }
        }

        public void PlayPlayerStep() => PlayOneShot(SoundName.PlayerStep);

        public void PlayEnemyStep() => PlayOneShot(SoundName.EnemyStep);
        public void PlayDiceFall() => PlayOneShot(SoundName.DiceDrop);
        public void PlayPlayerWin() => PlayOneShot(SoundName.PlayerWin);
        public void PlayPlayerDefeat() => PlayOneShot(SoundName.PlayerDefeat);
        public void PlaySwordsAttack() => PlayOneShot(SoundName.SwordsAttack);
        public void PlayHealthReplenish() => PlayOneShot(SoundName.HealthReplenish);
        public void PlayManaReplenish() => PlayOneShot(SoundName.ManaReplenish);
        public void PlayTeleport() => PlayOneShot(SoundName.Teleport);
        public void PlayPopUp() => PlayOneShot(SoundName.PopUp);
        public void PlaySpellCast() => PlayOneShot(SoundName.SpellCast);
        public void PlayClickButton() => PlayOneShot(SoundName.ClickButton);

        private void PlayOneShot(SoundName soundName)
        {
            if (_isEnabledByButton && _isEnabledByAdv && _isEnabledByFocus)
                _audioSource.PlayOneShot(_sounds[soundName]);
        }
    }
}