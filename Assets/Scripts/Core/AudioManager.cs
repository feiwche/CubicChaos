using UnityEngine;

namespace CubicChaos.Core
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioSource musicSource;

        public static AudioManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public void PlaySfx(AudioClip clip)
        {
            if (clip == null || sfxSource == null || !SettingsManager.SoundEnabled)
            {
                return;
            }

            sfxSource.PlayOneShot(clip);
        }

        public void PlayMusic(AudioClip clip)
        {
            if (musicSource == null)
            {
                return;
            }

            musicSource.clip = clip;

            if (clip == null || !SettingsManager.MusicEnabled)
            {
                musicSource.Stop();
                return;
            }

            musicSource.loop = true;
            musicSource.Play();
        }
    }
}
