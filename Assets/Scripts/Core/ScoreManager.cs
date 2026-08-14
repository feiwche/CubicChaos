using System;
using System.Collections;
using UnityEngine;

namespace CubicChaos.Core
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private int pointsPerClearedCell = 10;
        [SerializeField] private int pointsPerPlacedCell = 1;
        [SerializeField] private int comboBonusPerStep = 5;

        public event Action<int> ScoreChanged;

        public int Score { get; private set; }
        public int ComboCount { get; private set; }
        public bool IsDoubleScoreActive { get; private set; }

        private Coroutine doubleScoreRoutine;

        public void AddPlacedCells(int cellCount)
        {
            Score += ApplyMultiplier(cellCount * pointsPerPlacedCell);
            ScoreChanged?.Invoke(Score);
        }

        public void AddClearedCells(int cellCount)
        {
            ComboCount++;
            int comboBonus = (ComboCount - 1) * comboBonusPerStep;
            Score += ApplyMultiplier(cellCount * pointsPerClearedCell + comboBonus);
            ScoreChanged?.Invoke(Score);
        }

        public void ResetCombo()
        {
            ComboCount = 0;
        }

        public void ApplyPenalty(int points)
        {
            Score = Mathf.Max(0, Score - points);
            ScoreChanged?.Invoke(Score);
        }

        public void ActivateDoubleScore(float duration)
        {
            if (doubleScoreRoutine != null)
            {
                StopCoroutine(doubleScoreRoutine);
            }

            doubleScoreRoutine = StartCoroutine(DoubleScoreRoutine(duration));
        }

        private IEnumerator DoubleScoreRoutine(float duration)
        {
            IsDoubleScoreActive = true;
            yield return new WaitForSeconds(duration);
            IsDoubleScoreActive = false;
            doubleScoreRoutine = null;
        }

        private int ApplyMultiplier(int points)
        {
            return IsDoubleScoreActive ? points * 2 : points;
        }
    }
}
