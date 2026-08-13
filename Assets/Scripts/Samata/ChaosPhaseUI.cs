using TMPro;
using UnityEngine;

namespace BlockBrawl.Samata
{
    public class ChaosPhaseUI : MonoBehaviour
    {
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
        }
    }
}
