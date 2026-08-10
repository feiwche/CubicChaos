using UnityEngine;

namespace BlockBrawl.Pieces
{
    public class PieceView : MonoBehaviour
    {
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private float cellSize = 1f;

        public PieceShape Shape { get; private set; }

        public void Initialize(PieceShape shape)
        {
            Shape = shape;
            SpawnCells();
        }

        private void SpawnCells()
        {
            Vector2 centerOffset = GetCenterOffset();

            foreach (Vector2Int cell in Shape.cells)
            {
                GameObject cellObject = Instantiate(cellPrefab, transform);
                cellObject.transform.localPosition = new Vector3(
                    (cell.x - centerOffset.x) * cellSize,
                    (cell.y - centerOffset.y) * cellSize,
                    0f);
            }
        }

        private Vector2 GetCenterOffset()
        {
            int minX = Shape.cells[0].x;
            int maxX = Shape.cells[0].x;
            int minY = Shape.cells[0].y;
            int maxY = Shape.cells[0].y;

            foreach (Vector2Int cell in Shape.cells)
            {
                minX = Mathf.Min(minX, cell.x);
                maxX = Mathf.Max(maxX, cell.x);
                minY = Mathf.Min(minY, cell.y);
                maxY = Mathf.Max(maxY, cell.y);
            }

            return new Vector2((minX + maxX) / 2f, (minY + maxY) / 2f);
        }
    }
}
