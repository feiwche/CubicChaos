using UnityEngine;

namespace CubicChaos.Core
{
    /// <summary>
    /// Oyundaki tüm ses efektlerinin tek giriş noktası.
    /// Klip alanları şimdilik boş — gerçek ses dosyaları eklenene kadar
    /// her çağrı sessizce hiçbir şey yapmaz (hata vermez).
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioSource musicSource;

        [Header("Oynanış")]
        [SerializeField] private AudioClip piecePlaced;
        [SerializeField] private AudioClip invalidPlacement;
        [SerializeField] private AudioClip lineCleared;
        [SerializeField] private AudioClip comboClear;

        [Header("Şamata Modu")]
        [SerializeField] private AudioClip phaseChanged;
        [SerializeField] private AudioClip blocksInjected;
        [SerializeField] private AudioClip powerUpEarned;
        [SerializeField] private AudioClip powerUpUsed;

        [Header("Arayüz")]
        [SerializeField] private AudioClip buttonClick;
        [SerializeField] private AudioClip gameOver;
        [SerializeField] private AudioClip rewardCollected;
        [SerializeField] private AudioClip purchase;

        [Header("Müzik")]
        [SerializeField] private AudioClip backgroundMusic;

        public static AudioManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        private void Start()
        {
            PlayMusic(backgroundMusic);
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

            if (clip == null || !SettingsManager.MusicEnabled)
            {
                musicSource.Stop();
                return;
            }

            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();
        }

        // --- Statik kısayollar: çağıranların Instance null kontrolü yapmasına gerek yok ---

        private static void Play(AudioClip clip)
        {
            if (Instance != null)
            {
                Instance.PlaySfx(clip);
            }
        }

        public static void PiecePlaced() => Play(Instance != null ? Instance.piecePlaced : null);
        public static void InvalidPlacement() => Play(Instance != null ? Instance.invalidPlacement : null);
        public static void LineCleared() => Play(Instance != null ? Instance.lineCleared : null);
        public static void ComboClear() => Play(Instance != null ? Instance.comboClear : null);
        public static void PhaseChanged() => Play(Instance != null ? Instance.phaseChanged : null);
        public static void BlocksInjected() => Play(Instance != null ? Instance.blocksInjected : null);
        public static void PowerUpEarned() => Play(Instance != null ? Instance.powerUpEarned : null);
        public static void PowerUpUsed() => Play(Instance != null ? Instance.powerUpUsed : null);
        public static void ButtonClick() => Play(Instance != null ? Instance.buttonClick : null);
        public static void GameOver() => Play(Instance != null ? Instance.gameOver : null);
        public static void RewardCollected() => Play(Instance != null ? Instance.rewardCollected : null);
        public static void Purchase() => Play(Instance != null ? Instance.purchase : null);

        /// <summary>
        /// Ayarlar değiştiğinde müziği anında aç/kapat.
        /// </summary>
        public void RefreshMusic()
        {
            PlayMusic(backgroundMusic);
        }
    }
}
