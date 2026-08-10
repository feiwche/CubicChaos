using UnityEngine;

namespace BlockBrawl.Pieces
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class PieceView : MonoBehaviour
    {
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private float cellSize = 1f;

        private BoxCollider2D boxCollider;

        public PieceShape Shape { get; private set; }

        private void Awake()
        {
            boxCollider = GetComponent<BoxCollider2D>();
        }

        public void Initialize(PieceShape shape)
        {
            Shape = shape;
            SpawnCells();
        }

        private void SpawnCells()
        {
            (int minX, int maxX, int minY, int maxY) = GetBounds();
            Vector2 centerOffset = new Vector2((minX + maxX) / 2f, (minY + maxY) / 2f);

            foreach (Vector2Int cell in Shape.cells)
            {
                GameObject cellObject = Instantiate(cellPrefab, transform);
                cellObject.transform.localPosition = new Vector3(
                    (cell.x - centerOffset.x) * cellSize,
                    (cell.y - centerOffset.y) * cellSize,
                    0f);
            }

            int width = maxX - minX + 1;
            int height = maxY - minY + 1;
            boxCollider.size = new Vector2(width * cellSize, height * cellSize);
        }

        private (int minX, int maxX, int minY, int maxY) GetBounds()
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

            return (minX, maxX, minY, maxY);
        }
    }
}
