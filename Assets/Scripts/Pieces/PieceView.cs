using UnityEngine;

namespace BlockBrawl.Pieces
{
    public class PieceView : MonoBehaviour
    {
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private float cellSize = 0.4f;

        public PieceShape Shape { get; private set; }

        public void Initialize(PieceShape shape)
        {
            Shape = shape;
            SpawnCells();
        }

        private void SpawnCells()
        {
            foreach (Vector2Int cell in Shape.cells)
            {
                Vector3 localPosition = new Vector3(cell.x * cellSize, cell.y * cellSize, 0f);
                Instantiate(cellPrefab, transform.position + localPosition, Quaternion.identity, transform);
            }
        }
    }
}
