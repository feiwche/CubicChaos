using UnityEngine;
using CubicChaos.Core;

namespace CubicChaos.Samata
{
    public class PhaseBuffGranter : MonoBehaviour
    {
        [SerializeField] private ChaosMeter chaosMeter;
        [SerializeField] private PowerUpInventory inventory;
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private float kaosDoubleScoreDuration = 5f;

        private void OnEnable()
        {
            chaosMeter.PhaseChanged += HandlePhaseChanged;
        }

        private void OnDisable()
        {
            chaosMeter.PhaseChanged -= HandlePhaseChanged;
        }

        private void HandlePhaseChanged(ChaosPhase phase)
        {
            if (phase == ChaosPhase.Orta)
            {
                inventory.Add(inventory.GetRandomType());
                AudioManager.PowerUpEarned();
            }
            else if (phase == ChaosPhase.Kaos)
            {
                inventory.Add(inventory.GetRandomType());
                inventory.Add(inventory.GetRandomType());
                scoreManager.ActivateDoubleScore(kaosDoubleScoreDuration);
                AudioManager.PowerUpEarned();
            }
        }
    }
}
