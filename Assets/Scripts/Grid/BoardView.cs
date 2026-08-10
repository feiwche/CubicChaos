using UnityEngine;

namespace BlockBrawl.Grid
{
    public class BoardView : MonoBehaviour
    {
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private float cellSize = 1f;

        private Board board;

        private void Awake()
        {
            board = new Board();
            SpawnCells();
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

        public Vector3 GridToWorldPosition(int x, int y)
        {
            float worldX = (x - Board.Width / 2f + 0.5f) * cellSize;
            float worldY = (y - Board.Height / 2f + 0.5f) * cellSize;
            return new Vector3(worldX, worldY, 0f);
        }
    }
}
