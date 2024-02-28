using System.Collections.Generic;
using System.Linq;
using _Project.Sсripts.Config;
using UnityEngine;

namespace _Project.Sсripts.Controllers.Sound
{
    public class GameSounds
    {
        private readonly Dictionary<SoundName, AudioClip> _sounds;
        private bool _isEnabled = true;
        private readonly AudioSource _audioSource;

        public GameSounds(SoundSettings soundSettings, AudioSource audioSource)
        {
            _sounds = soundSettings.Configs.ToDictionary(key => key.SoundName, value => value.AudioClip);
            _audioSource = audioSource;
        }

        public void Switch(bool isEnabled)
        {
            _isEnabled = isEnabled;
            
            if (_isEnabled == false) 
                _audioSource.Stop();
        }

        public void PlayPlayerStep() => PlaySound(SoundName.PlayerStep);

        public void PlayEnemyStep() => PlaySound(SoundName.EnemyStep);
        public void PlayDiceFall() => PlaySound(SoundName.DiceDrop);
        public void PlayPlayerWin() => PlaySound(SoundName.PlayerWin);
        public void PlayPlayerDefeat() => PlaySound(SoundName.PlayerDefeat);
        public void PlaySwordsAttack() => PlaySound(SoundName.SwordsAttack);
        public void PlayHealthReplenish() => PlaySound(SoundName.HealthReplenish);
        public void PlayManaReplenish() => PlaySound(SoundName.ManaReplenish);
        public void PlayTeleport() => PlaySound(SoundName.Teleport);
        public void PlayPopUp() => PlaySound(SoundName.PopUp);
        public void PlaySpellCast() => PlaySound(SoundName.SpellCast);
        public void PlayClickButton() => PlaySound(SoundName.ClickButton);

        private void PlaySound(SoundName soundName)
        {
            if (_isEnabled)
                _audioSource.PlayOneShot(_sounds[soundName]);
        }
    }
}