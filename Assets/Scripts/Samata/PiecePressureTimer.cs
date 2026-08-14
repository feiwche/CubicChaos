using UnityEngine;
using CubicChaos.Core;
using CubicChaos.Pieces;

namespace CubicChaos.Samata
{
    public class PiecePressureTimer : MonoBehaviour
    {
        [SerializeField] private PieceTray pieceTray;
        [SerializeField] private ChaosMeter chaosMeter;
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private int slotCount = 3;
        [SerializeField] private float sakinTimeLimit = 12f;
        [SerializeField] private float ortaTimeLimit = 9f;
        [SerializeField] private float kaosTimeLimitStart = 6f;
        [SerializeField] private float kaosTimeLimitMin = 4f;
        [SerializeField] private float kaosRampDuration = 30f;
        [SerializeField] private int expiredPenaltyPoints = 15;

        private float[] slotTimers;

        private void Awake()
        {
            slotTimers = new float[slotCount];
        }

        private void OnEnable()
        {
            pieceTray.SlotRefilled += ResetSlotTimer;
        }

        private void OnDisable()
        {
            pieceTray.SlotRefilled -= ResetSlotTimer;
        }

        private void Update()
        {
            if (chaosMeter.IsFrozen)
            {
                return;
            }

            float limit = GetCurrentTimeLimit();

            for (int i = 0; i < slotCount; i++)
            {
                if (pieceTray.IsSlotEmpty(i))
                {
                    continue;
                }

                slotTimers[i] += Time.deltaTime;
                UpdateSlotTint(i, slotTimers[i] / limit);

                if (slotTimers[i] >= limit)
                {
                    ExpireSlot(i);
                }
            }
        }

        private void ExpireSlot(int slotIndex)
        {
            pieceTray.ForceReplaceSlot(slotIndex);
            scoreManager.ApplyPenalty(expiredPenaltyPoints);
        }

        private void ResetSlotTimer(int slotIndex)
        {
            slotTimers[slotIndex] = 0f;
        }

        private void UpdateSlotTint(int slotIndex, float ratio)
        {
            GameObject pieceObject = pieceTray.GetSlotPieceObject(slotIndex);
            if (pieceObject == null)
            {
                return;
            }

            Color tint = Color.Lerp(Color.white, Color.red, Mathf.Clamp01(ratio));
            foreach (SpriteRenderer spriteRenderer in pieceObject.GetComponentsInChildren<SpriteRenderer>())
            {
                spriteRenderer.color = tint;
            }
        }

        private float GetCurrentTimeLimit()
        {
            switch (chaosMeter.CurrentPhase)
            {
                case ChaosPhase.Orta:
                    return ortaTimeLimit;
                case ChaosPhase.Kaos:
                    return GetKaosTimeLimit();
                default:
                    return sakinTimeLimit;
            }
        }

        private float GetKaosTimeLimit()
        {
            float timeInKaos = chaosMeter.ElapsedTime - chaosMeter.KaosPhaseStartTime;
            float t = Mathf.Clamp01(timeInKaos / kaosRampDuration);
            return Mathf.Lerp(kaosTimeLimitStart, kaosTimeLimitMin, t);
        }
    }
}
