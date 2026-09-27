using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CubicChaos.Samata
{
    public class ChaosPhaseUI : MonoBehaviour
    {
        private static readonly Color SakinColor = new Color(0.3f, 0.7f, 0.5f);
        private static readonly Color OrtaColor = new Color(1f, 0.65f, 0.2f);
        private static readonly Color KaosColor = new Color(0.9f, 0.25f, 0.25f);

        [SerializeField] private ChaosMeter chaosMeter;
        [SerializeField] private TMP_Text phaseText;
        [SerializeField] private Transform bannerRoot;
        [SerializeField] private TMP_Text bannerText;

        private bool isFirstUpdate = true;

        private void OnEnable()
        {
            chaosMeter.PhaseChanged += UpdatePhaseText;
            isFirstUpdate = true;
            UpdatePhaseText(chaosMeter.CurrentPhase);
        }

        private void OnDisable()
        {
            chaosMeter.PhaseChanged -= UpdatePhaseText;
        }

        private void UpdatePhaseText(ChaosPhase phase)
        {
            phaseText.text = $"Faz: {phase}";
            phaseText.color = phase switch
            {
                ChaosPhase.Orta => OrtaColor,
                ChaosPhase.Kaos => KaosColor,
                _ => SakinColor
            };

            if (!isFirstUpdate)
            {
                ShowBanner(phase);
            }

            isFirstUpdate = false;
        }

        private void ShowBanner(ChaosPhase phase)
        {
            if (bannerRoot == null || bannerText == null || phase == ChaosPhase.Sakin)
            {
                return;
            }

            CubicChaos.Core.AudioManager.PhaseChanged();

            bannerText.text = phase == ChaosPhase.Kaos ? "KAOS BAŞLADI!" : "ORTA FAZ!";
            bannerText.color = phase == ChaosPhase.Kaos ? KaosColor : OrtaColor;

            bannerRoot.gameObject.SetActive(true);
            bannerRoot.localScale = Vector3.zero;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(bannerRoot.DOScale(1f, 0.3f).SetEase(Ease.OutBack));
            sequence.AppendInterval(1f);
            sequence.Append(bannerRoot.DOScale(0f, 0.2f).SetEase(Ease.InBack));
            sequence.OnComplete(() => bannerRoot.gameObject.SetActive(false));
        }
    }
}
