using System;
using UnityEngine;

namespace BlockBrawl.Samata
{
    public enum ChaosPhase
    {
        Sakin,
        Orta,
        Kaos
    }

    public class ChaosMeter : MonoBehaviour
    {
        [SerializeField] private float ortaPhaseStartTime = 30f;
        [SerializeField] private float kaosPhaseStartTime = 90f;

        public event Action<ChaosPhase> PhaseChanged;

        public ChaosPhase CurrentPhase { get; private set; } = ChaosPhase.Sakin;
        public float ElapsedTime { get; private set; }

        private void Update()
        {
            ElapsedTime += Time.deltaTime;
            UpdatePhase();
        }

        private void UpdatePhase()
        {
            ChaosPhase newPhase = DeterminePhase();

            if (newPhase != CurrentPhase)
            {
                CurrentPhase = newPhase;
                PhaseChanged?.Invoke(CurrentPhase);
            }
        }

        private ChaosPhase DeterminePhase()
        {
            if (ElapsedTime >= kaosPhaseStartTime)
            {
                return ChaosPhase.Kaos;
            }

            if (ElapsedTime >= ortaPhaseStartTime)
            {
                return ChaosPhase.Orta;
            }

            return ChaosPhase.Sakin;
        }
    }
}
