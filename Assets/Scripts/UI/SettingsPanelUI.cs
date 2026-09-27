using TMPro;
using UnityEngine;
using UnityEngine.UI;
using CubicChaos.Core;

namespace CubicChaos.UI
{
    /// <summary>
    /// Ses / Müzik / Titreşim ayarlarını gösteren panel.
    /// Toggle yerine metin butonları kullanılır (Açık/Kapalı) — böylece
    /// ek bir Toggle prefab'ına ihtiyaç duymadan sahne YAML'ıyla kurulabiliyor.
    /// </summary>
    public class SettingsPanelUI : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private Button openButton;
        [SerializeField] private Button closeButton;

        [SerializeField] private Button soundButton;
        [SerializeField] private TMP_Text soundLabel;
        [SerializeField] private Button musicButton;
        [SerializeField] private TMP_Text musicLabel;
        [SerializeField] private Button vibrationButton;
        [SerializeField] private TMP_Text vibrationLabel;

        private static readonly Color OnColor = new Color(0.184f, 0.788f, 0.753f);
        private static readonly Color OffColor = new Color(0.435f, 0.463f, 0.549f);

        private void Awake()
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }

            if (openButton != null)
            {
                openButton.onClick.AddListener(Open);
            }

            if (closeButton != null)
            {
                closeButton.onClick.AddListener(Close);
            }

            if (soundButton != null)
            {
                soundButton.onClick.AddListener(ToggleSound);
            }

            if (musicButton != null)
            {
                musicButton.onClick.AddListener(ToggleMusic);
            }

            if (vibrationButton != null)
            {
                vibrationButton.onClick.AddListener(ToggleVibration);
            }
        }

        private void Open()
        {
            AudioManager.ButtonClick();
            Refresh();

            if (panel != null)
            {
                panel.SetActive(true);
            }
        }

        private void Close()
        {
            AudioManager.ButtonClick();

            if (panel != null)
            {
                panel.SetActive(false);
            }
        }

        private void ToggleSound()
        {
            SettingsManager.SoundEnabled = !SettingsManager.SoundEnabled;
            Refresh();
            // Sesi yeni açtıysa duyulsun diye tıklama sesi güncellemeden sonra çalınır.
            AudioManager.ButtonClick();
        }

        private void ToggleMusic()
        {
            SettingsManager.MusicEnabled = !SettingsManager.MusicEnabled;
            Refresh();
            AudioManager.ButtonClick();

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.RefreshMusic();
            }
        }

        private void ToggleVibration()
        {
            SettingsManager.VibrationEnabled = !SettingsManager.VibrationEnabled;
            Refresh();
            AudioManager.ButtonClick();
        }

        private void Refresh()
        {
            Apply(soundLabel, "Ses", SettingsManager.SoundEnabled);
            Apply(musicLabel, "Müzik", SettingsManager.MusicEnabled);
            Apply(vibrationLabel, "Titreşim", SettingsManager.VibrationEnabled);
        }

        private void Apply(TMP_Text label, string title, bool enabled)
        {
            if (label == null)
            {
                return;
            }

            label.text = $"{title}: {(enabled ? "Açık" : "Kapalı")}";
            label.color = enabled ? OnColor : OffColor;
        }
    }
}
