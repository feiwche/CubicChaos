using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BlockBrawl.Core
{
    public class ReturnToMenuButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private string mainMenuSceneName = "MainMenu";

        private void Awake()
        {
            button.onClick.AddListener(ReturnToMenu);
        }

        private void ReturnToMenu()
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }
}
