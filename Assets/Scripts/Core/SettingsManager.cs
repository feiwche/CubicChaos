using System;
using UnityEngine;

namespace CubicChaos.Core
{
    public static class SettingsManager
    {
        private const string SoundKey = "CubicChaos.SoundEnabled";
        private const string MusicKey = "CubicChaos.MusicEnabled";
        private const string VibrationKey = "CubicChaos.VibrationEnabled";

        public static event Action SettingsChanged;

        public static bool SoundEnabled
        {
            get => PlayerPrefs.GetInt(SoundKey, 1) == 1;
            set
            {
                PlayerPrefs.SetInt(SoundKey, value ? 1 : 0);
                SettingsChanged?.Invoke();
            }
        }

        public static bool MusicEnabled
        {
            get => PlayerPrefs.GetInt(MusicKey, 1) == 1;
            set
            {
                PlayerPrefs.SetInt(MusicKey, value ? 1 : 0);
                SettingsChanged?.Invoke();
            }
        }

        public static bool VibrationEnabled
        {
            get => PlayerPrefs.GetInt(VibrationKey, 1) == 1;
            set
            {
                PlayerPrefs.SetInt(VibrationKey, value ? 1 : 0);
                SettingsChanged?.Invoke();
            }
        }
    }
}
