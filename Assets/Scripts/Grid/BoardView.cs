using System.Collections.Generic;
using UnityEngine;
using BlockBrawl.Core;

namespace BlockBrawl.Grid
{
    public class BoardView : MonoBehaviour
    {
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private GameObject blockCellPrefab;
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private float cellSize = 1f;

        [SerializeField] [Range(0f, 1f)] private float prefilledChance = 0.4f;
        [SerializeField] private int normalMinCells = 4;
        [SerializeField] private int normalMaxCells = 7;
        [SerializeField] [Range(0f, 1f)] private float denseTierChance = 0.25f;
        [SerializeField] private int denseMinCells = 10;
        [SerializeField] private int denseMaxCells = 13;
        [SerializeField] private float clearStaggerStep = 0.025f;

        private readonly GameObject[,] blockVisuals = new GameObject[Board.Width, Board.Height];

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

            HashSet<Vector2Int> chosenCells = GrowRandomCluster(blockCount, onlyEmptyCells: false);
            PlaceShapeBlocks(chosenCells, 0, 0, countsForScore: false);
        }

        public void InjectRandomBlocks(int blockCount)
        {
            HashSet<Vector2Int> chosenCells = GrowRandomCluster(blockCount, onlyEmptyCells: true);

            if (chosenCells.Count == 0)
            {
                return;
            }

            PlaceShapeBlocks(chosenCells, 0, 0, countsForScore: false);
        }

        private static readonly Vector2Int[] ClusterDirections =
        {
            Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
        };

        private HashSet<Vector2Int> GrowRandomCluster(int targetCount, bool onlyEmptyCells)
        {
            Vector2Int? startCell = onlyEmptyCells
                ? FindRandomEmptyCell()
                : new Vector2Int(Random.Range(0, Board.Width), Random.Range(0, Board.Height));

            if (startCell == null)
            {
                return new HashSet<Vector2Int>();
            }

            HashSet<Vector2Int> cluster = new HashSet<Vector2Int> { startCell.Value };

            int safetyLimit = targetCount * 30;

            for (int attempt = 0; cluster.Count < targetCount && attempt < safetyLimit; attempt++)
            {
                Vector2Int[] clusterCells = new Vector2Int[cluster.Count];
                cluster.CopyTo(clusterCells);

                Vector2Int fromCell = clusterCells[Random.Range(0, clusterCells.Length)];
                Vector2Int direction = ClusterDirections[Random.Range(0, ClusterDirections.Length)];
                Vector2Int candidate = fromCell + direction;

                bool insideBoard = candidate.x >= 0 && candidate.x < Board.Width && candidate.y >= 0 && candidate.y < Board.Height;
                bool cellAvailable = insideBoard && (!onlyEmptyCells || !board.IsCellOccupied(candidate.x, candidate.y));

                if (cellAvailable)
                {
                    cluster.Add(candidate);
                }
            }

            return cluster;
        }

        private Vector2Int? FindRandomEmptyCell()
        {
            List<Vector2Int> emptyCells = new List<Vector2Int>();

            for (int y = 0; y < Board.Height; y++)
            {
                for (int x = 0; x < Board.Width; x++)
                {
                    if (!board.IsCellOccupied(x, y))
                    {
                        emptyCells.Add(new Vector2Int(x, y));
                    }
                }
            }

            if (emptyCells.Count == 0)
            {
                return null;
            }

            return emptyCells[Random.Range(0, emptyCells.Count)];
        }

        public bool PlaceShapeBlocks(IEnumerable<Vector2Int> relativeCells, int originX, int originY, bool countsForScore = true)
        {
            List<Vector2Int> cells = new List<Vector2Int>(relativeCells);
            board.PlaceCells(cells, originX, originY);

            foreach (Vector2Int cell in cells)
            {
                int x = originX + cell.x;
                int y = originY + cell.y;
                Vector3 worldPosition = GridToWorldPosition(x, y);
                blockVisuals[x, y] = Instantiate(blockCellPrefab, worldPosition, Quaternion.identity, transform);
            }

            if (countsForScore && scoreManager != null)
            {
                scoreManager.AddPlacedCells(cells.Count);
            }

            return ClearFullLines(countsForScore);
        }

        private bool ClearFullLines(bool countsForScore)
        {
            List<int> fullRows = board.GetFullRows();
            List<int> fullColumns = board.GetFullColumns();

            int clearedCellCount = 0;

            foreach (int y in fullRows)
            {
                for (int x = 0; x < Board.Width; x++)
                {
                    if (ClearCellVisual(x, y, x * clearStaggerStep))
                    {
                        clearedCellCount++;
                    }
                }
            }

            foreach (int x in fullColumns)
            {
                for (int y = 0; y < Board.Height; y++)
                {
                    if (ClearCellVisual(x, y, y * clearStaggerStep))
                    {
                        clearedCellCount++;
                    }
                }
            }

            bool clearedAnyLine = fullRows.Count > 0 || fullColumns.Count > 0;

            if (countsForScore && scoreManager != null)
            {
                if (clearedAnyLine)
                {
                    scoreManager.AddClearedCells(clearedCellCount);
                }
                else
                {
                    scoreManager.ResetCombo();
                }
            }

            return clearedAnyLine;
        }

        private bool ClearCellVisual(int x, int y, float delay)
        {
            if (blockVisuals[x, y] == null)
            {
                return false;
            }

            GameObject visual = blockVisuals[x, y];
            blockVisuals[x, y] = null;
            board.SetOccupied(x, y, false);

            BlockCellEffect effect = visual.GetComponent<BlockCellEffect>();
            if (effect != null)
            {
                effect.PlayClearAndDestroy(delay);
            }
            else
            {
                Destroy(visual);
            }

            return true;
        }

        public Vector3 GridToWorldPosition(int x, int y)
        {
            float worldX = (x - Board.Width / 2f + 0.5f) * cellSize;
            float worldY = (y - Board.Height / 2f + 0.5f) * cellSize;
            return new Vector3(worldX, worldY, 0f);
        }

        public Vector2Int WorldToGridPosition(Vector3 worldPosition)
        {
            int x = Mathf.RoundToInt(worldPosition.x / cellSize + Board.Width / 2f - 0.5f);
            int y = Mathf.RoundToInt(worldPosition.y / cellSize + Board.Height / 2f - 0.5f);
            return new Vector2Int(x, y);
        }
    }
}
