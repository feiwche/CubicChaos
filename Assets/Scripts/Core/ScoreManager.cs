using System;
using UnityEngine;

namespace BlockBrawl.Core
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private int pointsPerClearedCell = 10;
        [SerializeField] private int pointsPerPlacedCell = 1;
        [SerializeField] private int comboBonusPerStep = 5;

        public event Action<int> ScoreChanged;

        public int Score { get; private set; }
        public int ComboCount { get; private set; }

        public void AddPlacedCells(int cellCount)
        {
            Score += cellCount * pointsPerPlacedCell;
            ScoreChanged?.Invoke(Score);
        }

        public void AddClearedCells(int cellCount)
        {
            ComboCount++;
            int comboBonus = (ComboCount - 1) * comboBonusPerStep;
            Score += cellCount * pointsPerClearedCell + comboBonus;
            ScoreChanged?.Invoke(Score);
        }

        public void ResetCombo()
        {
            ComboCount = 0;
        }
    }
}
