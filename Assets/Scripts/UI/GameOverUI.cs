using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using CubicChaos.Core;

namespace CubicChaos.UI
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private Button restartButton;
        [SerializeField] private TMP_Text coinRewardText;

        private void Awake()
        {
            gameOverPanel.SetActive(false);
            restartButton.onClick.AddListener(Restart);
        }

        public void Show(int finalScore)
        {
            gameOverPanel.SetActive(true);
            AudioManager.GameOver();
            AwardCoins(finalScore);
        }

        private void AwardCoins(int finalScore)
        {
            int reward = Mathf.Max(1, finalScore / 10);
            CurrencyManager.Add(reward);

            if (coinRewardText == null)
            {
                return;
            }

            int displayed = 0;
            coinRewardText.text = "+0";
            DOTween.To(() => displayed, x =>
            {
                displayed = x;
                coinRewardText.text = $"+{displayed}";
            }, reward, 0.6f).SetEase(Ease.OutQuad);
        }

        private void Restart()
        {
            AudioManager.ButtonClick();
            Scene currentScene = SceneManager.GetActiveScene();
            SceneTransitionFade.Load(currentScene.name);
        }
    }
}
