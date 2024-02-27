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

        public GameSounds(SoundSettings soundSettings)
        {
            _sounds = soundSettings.Configs.ToDictionary(key => key.SoundName, value => value.AudioClip);
        }

        public void Switch(bool isEnabled)
        {
            _isEnabled = isEnabled;
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
                AudioSource.PlayClipAtPoint(_sounds[soundName], Vector3.zero);
        }
    }

    public enum SoundName
    {
        PlayerStep,
        EnemyStep,
        DiceDrop,
        PlayerWin,
        PlayerDefeat,
        SwordsAttack,
        HealthReplenish,
        ManaReplenish,
        Teleport,
        PopUp,
        SpellCast,
        ClickButton
    }
}