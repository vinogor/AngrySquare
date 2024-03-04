using System.Collections.Generic;
using System.Linq;
using _Project.Config;
using UnityEngine;

namespace _Project.Controllers.Sound
{
    public class GameSounds
    {
        private readonly Dictionary<SoundName, AudioClip> _sounds;
        private bool _isEnabled = true;
        private readonly AudioSource _audioSource;
        private readonly AudioSource _backgroundAudioSource;

        public GameSounds(SoundSettings soundSettings, AudioSource audioSource, AudioSource backgroundAudioSource)
        {
            _sounds = soundSettings.Configs.ToDictionary(key => key.SoundName, value => value.AudioClip);
            _audioSource = audioSource;
            _backgroundAudioSource = backgroundAudioSource;

            _backgroundAudioSource.loop = true;
            _backgroundAudioSource.Play();
        }

        public void Switch(bool isEnabled)
        {
            _isEnabled = isEnabled;

            if (_isEnabled == false)
            {
                _audioSource.Stop();
                _backgroundAudioSource.Pause();
            }
            else
            {
                _backgroundAudioSource.Play();
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
            if (_isEnabled)
                _audioSource.PlayOneShot(_sounds[soundName]);
        }
    }
}