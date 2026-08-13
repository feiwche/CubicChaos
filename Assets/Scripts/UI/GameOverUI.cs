using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BlockBrawl.UI
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private Button restartButton;

        private void Awake()
        {
            gameOverPanel.SetActive(false);
            restartButton.onClick.AddListener(Restart);
        }

        public void Show()
        {
            gameOverPanel.SetActive(true);
        }

        private void Restart()
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }
    }
}
