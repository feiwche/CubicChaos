using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CubicChaos.UI
{
    public class SceneTransitionFade : MonoBehaviour
    {
        [SerializeField] private Image fadeImage;
        [SerializeField] private float fadeDuration = 0.3f;

        public static SceneTransitionFade Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }

        private void Start()
        {
            if (fadeImage == null)
            {
                return;
            }

            // Sahne açılışında siyahtan oyuna doğru yumuşak geçiş.
            SetAlpha(1f);
            fadeImage.raycastTarget = true;
            fadeImage.DOFade(0f, fadeDuration)
                .OnComplete(() => fadeImage.raycastTarget = false);
        }

        /// <summary>
        /// Ekranı karartıp sahneyi yükler. Fade katmanı yoksa doğrudan yükler.
        /// </summary>
        public void LoadScene(string sceneName)
        {
            if (fadeImage == null)
            {
                SceneManager.LoadScene(sceneName);
                return;
            }

            // Geçiş sırasında tıklamaları yut ki çift yükleme olmasın.
            fadeImage.raycastTarget = true;
            fadeImage.DOKill();
            fadeImage.DOFade(1f, fadeDuration)
                .OnComplete(() => SceneManager.LoadScene(sceneName));
        }

        /// <summary>
        /// Fade katmanı varsa onunla, yoksa doğrudan sahneyi yükler.
        /// Çağıranların Instance null kontrolü yapmasına gerek kalmaz.
        /// </summary>
        public static void Load(string sceneName)
        {
            if (Instance != null)
            {
                Instance.LoadScene(sceneName);
            }
            else
            {
                SceneManager.LoadScene(sceneName);
            }
        }

        private void SetAlpha(float alpha)
        {
            Color color = fadeImage.color;
            color.a = alpha;
            fadeImage.color = color;
        }
    }
}
