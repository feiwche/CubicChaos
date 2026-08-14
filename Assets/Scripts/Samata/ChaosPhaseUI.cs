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

        private void OnEnable()
        {
            chaosMeter.PhaseChanged += UpdatePhaseText;
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
        }
    }
}
