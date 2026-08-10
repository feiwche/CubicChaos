using System.Collections.Generic;
using UnityEngine;

namespace BlockBrawl.Grid
{
    public class BoardView : MonoBehaviour
    {
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private GameObject blockCellPrefab;
        [SerializeField] private float cellSize = 1f;

        [SerializeField] [Range(0f, 1f)] private float prefilledChance = 0.3f;
        [SerializeField] private int normalMinCells = 4;
        [SerializeField] private int normalMaxCells = 7;
        [SerializeField] [Range(0f, 1f)] private float denseTierChance = 0.15f;
        [SerializeField] private int denseMinCells = 10;
        [SerializeField] private int denseMaxCells = 13;

        private Board board;

        public Board Board => board;

        private void Awake()
        {
            board = new Board();
            SpawnCells();
            SeedRandomBlocks();
        }

        private void SpawnCells()
        {
            for (int y = 0; y < Board.Height; y++)
            {
                for (int x = 0; x < Board.Width; x++)
                {
                    Vector3 worldPosition = GridToWorldPosition(x, y);
                    Instantiate(cellPrefab, worldPosition, Quaternion.identity, transform);
                }
            }
        }

        private void SeedRandomBlocks()
        {
            if (Random.value > prefilledChance)
            {
                return;
            }

            bool isDenseTier = Random.value < denseTierChance;
            int blockCount = isDenseTier
                ? Random.Range(denseMinCells, denseMaxCells + 1)
                : Random.Range(normalMinCells, normalMaxCells + 1);

            HashSet<Vector2Int> chosenCells = new HashSet<Vector2Int>();

            while (chosenCells.Count < blockCount)
            {
                int x = Random.Range(0, Board.Width);
                int y = Random.Range(0, Board.Height);
                chosenCells.Add(new Vector2Int(x, y));
            }

            foreach (Vector2Int cell in chosenCells)
            {
                board.SetOccupied(cell.x, cell.y, true);
                Vector3 worldPosition = GridToWorldPosition(cell.x, cell.y);
                Instantiate(blockCellPrefab, worldPosition, Quaternion.identity, transform);
            }
        }

        public Vector3 GridToWorldPosition(int x, int y)
        {
            float worldX = (x - Board.Width / 2f + 0.5f) * cellSize;
            float worldY = (y - Board.Height / 2f + 0.5f) * cellSize;
            return new Vector3(worldX, worldY, 0f);
        }
    }
}
