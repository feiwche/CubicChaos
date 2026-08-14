using UnityEngine;
using TMPro;
using CubicChaos.Core;

namespace CubicChaos.UI
{
    public class ScoreUI : MonoBehaviour
    {
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private TMP_Text scoreText;

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
        }
    }
}
