using UnityEngine;

namespace BlockBrawl.Core
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private int pointsPerClearedCell = 10;

        public int Score { get; private set; }

        public void AddClearedCells(int cellCount)
        {
            Score += cellCount * pointsPerClearedCell;
            Debug.Log($"Satır/sütun temizlendi: {cellCount} hücre, +{cellCount * pointsPerClearedCell} puan. Toplam skor: {Score}");
        }
    }
}
