using UnityEngine;

namespace GgAccel
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [SerializeField] private AudioSource musicSource, effectSource;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public static void PlaySound(AudioClip clip, float volumeScale = 1)
        {
            Instance.effectSource.PlayOneShot(clip, Instance.effectSource.volume * volumeScale);
        }

        public static void ChangeMasterVolume(float value)
        {
            AudioListener.volume = value;
        }

        public static void PlayPauseAudio()
        {
            Instance._PlayPauseAudio();
        }

        private void _PlayPauseAudio()
        {
            if (musicSource.isPlaying)
            {
                musicSource.Pause();
            }
            else
            {
                musicSource.Play();
            }
        }

        public static void ToggleSoundEffect()
        {
            Instance.effectSource.mute = !Instance.effectSource.mute;
        }

        public static void ToggleAudio()
        {
            Instance.musicSource.mute = !Instance.musicSource.mute;
        }
    }
}