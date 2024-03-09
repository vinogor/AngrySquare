using NaughtyAttributes;
using UnityEngine;

namespace View
{
    public class SoundsView : MonoBehaviour
    {
        [SerializeField] [Required] private AudioSource _soundsAudioSource;
        [SerializeField] [Required] private AudioSource _musicAudioSource;

        private void OnEnable()
        {
            _musicAudioSource.loop = true;
            _musicAudioSource.Play();
        }

        public void StopAll()
        {
            _soundsAudioSource.Stop();
            _musicAudioSource.Pause();
        }

        public void UnPauseMusic()
        {
            _musicAudioSource.UnPause();
        }

        public void PlayOneShot(AudioClip audioClip)
        {
            _soundsAudioSource.PlayOneShot(audioClip);
        }
    }
}