using System;
using UnityEngine;

namespace BlockBrawl.Core
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private int pointsPerClearedCell = 10;

        public event Action<int> ScoreChanged;

        public int Score { get; private set; }

        public void AddClearedCells(int cellCount)
        {
            Score += cellCount * pointsPerClearedCell;
            ScoreChanged?.Invoke(Score);
        }
    }
}
