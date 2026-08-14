using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CubicChaos.Core
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private Button normalModeButton;
        [SerializeField] private Button samataModeButton;
        [SerializeField] private string normalModeSceneName = "SoloEndless";
        [SerializeField] private string samataModeSceneName = "SamataMode";

        private void Awake()
        {
            normalModeButton.onClick.AddListener(LoadNormalMode);
            samataModeButton.onClick.AddListener(LoadSamataMode);
        }

        private void LoadNormalMode()
        {
            SceneManager.LoadScene(normalModeSceneName);
        }

        private void LoadSamataMode()
        {
            SceneManager.LoadScene(samataModeSceneName);
        }
    }
}
