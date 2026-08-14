using UnityEngine;
using BlockBrawl.Grid;
using BlockBrawl.Pieces;

namespace BlockBrawl.Samata
{
    public class BlockInjector : MonoBehaviour
    {
        [SerializeField] private BoardView boardView;
        [SerializeField] private ChaosMeter chaosMeter;
        [SerializeField] private PieceTray pieceTray;

        [SerializeField] private float sakinInterval = 20f;
        [SerializeField] private float ortaInterval = 12f;
        [SerializeField] private float kaosInterval = 6f;

        [SerializeField] private int sakinBlockCount = 2;
        [SerializeField] private int ortaBlockCount = 3;
        [SerializeField] private int kaosBlockCount = 5;

        private float timer;

        private void Update()
        {
            if (chaosMeter.IsFrozen)
            {
                return;
            }

            timer += Time.deltaTime;

            if (timer >= GetCurrentInterval())
            {
                timer = 0f;
                InjectBlocks();
            }
        }

        private void InjectBlocks()
        {
            boardView.InjectRandomBlocks(GetCurrentBlockCount());
            pieceTray.RecheckGameOver();
        }

        private float GetCurrentInterval()
        {
            switch (chaosMeter.CurrentPhase)
            {
                case ChaosPhase.Orta:
                    return ortaInterval;
                case ChaosPhase.Kaos:
                    return kaosInterval;
                default:
                    return sakinInterval;
            }
        }

        private int GetCurrentBlockCount()
        {
            switch (chaosMeter.CurrentPhase)
            {
                case ChaosPhase.Orta:
                    return ortaBlockCount;
                case ChaosPhase.Kaos:
                    return kaosBlockCount;
                default:
                    return sakinBlockCount;
            }
        }
    }
}
