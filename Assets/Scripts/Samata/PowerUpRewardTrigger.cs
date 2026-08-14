using UnityEngine;
using BlockBrawl.Core;

namespace BlockBrawl.Samata
{
    public class PowerUpRewardTrigger : MonoBehaviour
    {
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private PowerUpInventory inventory;
        [SerializeField] private int comboRewardInterval = 4;
        [SerializeField] private int scoreRewardInterval = 350;

        private int lastCheckedComboCount;
        private int lastRewardedScoreTier;

        private void OnEnable()
        {
            scoreManager.ScoreChanged += HandleScoreChanged;
        }

        private void OnDisable()
        {
            scoreManager.ScoreChanged -= HandleScoreChanged;
        }

        private void HandleScoreChanged(int score)
        {
            CheckScoreReward(score);
            CheckComboReward(scoreManager.ComboCount);
        }

        private void CheckScoreReward(int score)
        {
            int scoreTier = score / scoreRewardInterval;

            if (scoreTier > lastRewardedScoreTier)
            {
                lastRewardedScoreTier = scoreTier;
                GrantRandomPowerUp();
            }
        }

        private void CheckComboReward(int comboCount)
        {
            if (comboCount == lastCheckedComboCount)
            {
                return;
            }

            lastCheckedComboCount = comboCount;

            if (comboCount > 0 && comboCount % comboRewardInterval == 0)
            {
                GrantRandomPowerUp();
            }
        }

        private void GrantRandomPowerUp()
        {
            inventory.Add(inventory.GetRandomType());
        }
    }
}
