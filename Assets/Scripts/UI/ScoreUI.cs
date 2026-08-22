using DG.Tweening;
using UnityEngine;
using TMPro;
using CubicChaos.Core;

namespace CubicChaos.UI
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text bestText;
        [SerializeField] private string modeKey = "Normal";

        private void OnEnable()
        {
            scoreManager.ScoreChanged += UpdateScoreText;
            UpdateScoreText(scoreManager.Score);
        }

        private void OnDisable()
        {
            scoreManager.ScoreChanged -= UpdateScoreText;
        }

        private void UpdateScoreText(int score)
        {
            scoreText.text = $"Skor: {score}";
            HighScoreManager.TrySetBest(modeKey, score);
            bestText.text = $"Rekor: {HighScoreManager.GetBest(modeKey)}";

            if (scoreManager.ComboCount >= 2)
            {
                scoreText.transform.DOPunchScale(Vector3.one * 0.15f, 0.25f, 4, 0.5f);
            }
        }
    }
}
