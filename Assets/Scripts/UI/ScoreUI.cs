using UnityEngine;
using TMPro;
using BlockBrawl.Core;

namespace BlockBrawl.UI
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
