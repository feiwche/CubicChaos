using System;
using UnityEngine;

namespace CubicChaos.Core
{
    public static class SkinManager
    {
        public const string DefaultSkinId = "klasik";

        private const string SelectedSkinKey = "CubicChaos.SelectedSkin";
        private const string UnlockedKeyPrefix = "CubicChaos.SkinUnlocked.";

        public static event Action SkinChanged;

        public static string SelectedSkinId => PlayerPrefs.GetString(SelectedSkinKey, DefaultSkinId);

        public static bool IsUnlocked(string skinId)
        {
            return skinId == DefaultSkinId || PlayerPrefs.GetInt(UnlockedKeyPrefix + skinId, 0) == 1;
        }

        public static void Unlock(string skinId)
        {
            PlayerPrefs.SetInt(UnlockedKeyPrefix + skinId, 1);
        }

        public static void Select(string skinId)
        {
            if (!IsUnlocked(skinId))
            {
                return;
            }

            PlayerPrefs.SetString(SelectedSkinKey, skinId);
            SkinChanged?.Invoke();
        }
    }
}
