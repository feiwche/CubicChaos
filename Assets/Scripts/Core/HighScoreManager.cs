using UnityEngine;

namespace CubicChaos.Core
{
    public static class HighScoreManager
    {
        private const string KeyPrefix = "CubicChaos.HighScore.";

        public static int GetBest(string modeKey)
        {
            return PlayerPrefs.GetInt(KeyPrefix + modeKey, 0);
        }

        public static bool TrySetBest(string modeKey, int score)
        {
            if (score <= GetBest(modeKey))
            {
                return false;
            }

            PlayerPrefs.SetInt(KeyPrefix + modeKey, score);
            return true;
        }
    }
}
