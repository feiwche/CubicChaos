using UnityEngine;
using UnityEngine.UI;
using CubicChaos.UI;

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
            AudioManager.ButtonClick();
            SceneTransitionFade.Load(normalModeSceneName);
        }

        private void LoadSamataMode()
        {
            AudioManager.ButtonClick();
            SceneTransitionFade.Load(samataModeSceneName);
        }
    }
}
