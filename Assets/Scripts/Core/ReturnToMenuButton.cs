using UnityEngine;
using UnityEngine.UI;
using CubicChaos.UI;

namespace CubicChaos.Core
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
            AudioManager.ButtonClick();
            SceneTransitionFade.Load(mainMenuSceneName);
        }
    }
}
