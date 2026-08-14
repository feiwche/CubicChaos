using System;
using System.Collections;
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
        public bool IsFrozen { get; private set; }

        private Coroutine freezeRoutine;

        private void Update()
        {
            if (IsFrozen)
            {
                return;
            }

            ElapsedTime += Time.deltaTime;
            UpdatePhase();
        }

        public void Freeze(float duration)
        {
            if (freezeRoutine != null)
            {
                StopCoroutine(freezeRoutine);
            }

            freezeRoutine = StartCoroutine(FreezeRoutine(duration));
        }

        private IEnumerator FreezeRoutine(float duration)
        {
            IsFrozen = true;
            yield return new WaitForSeconds(duration);
            IsFrozen = false;
            freezeRoutine = null;
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
