using System.Collections.Generic;
using UnityEngine;
using BlockBrawl.Core;
using BlockBrawl.Grid;
using BlockBrawl.UI;

namespace BlockBrawl.Pieces
{
    public class PieceTray : MonoBehaviour
    {
        [SerializeField] private BoardView boardView;
        [SerializeField] private GameOverUI gameOverUI;
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private PieceShape[] availableShapes;
        [SerializeField] private GameObject piecePrefab;
        [SerializeField] private int slotCount = 3;
        [SerializeField] private float slotSpacing = 2.5f;
        [SerializeField] private float traySlotY = -5f;
        [SerializeField] private float traySlotScale = 0.4f;
        [SerializeField] private float lineClearWeightMultiplier = 4f;
        [SerializeField] private float fitBonusPerEdge = 0.6f;
        [SerializeField] private float difficultyBonusPerCell = 0.4f;
        [SerializeField] private int difficultyRampScore = 1000;

        private bool[] slotEmpty;
        private PieceShape[] slotShapes;
        private GameObject[] slotPieceObjects;

        public event System.Action<int> SlotRefilled;

        private void Start()
        {
            slotEmpty = new bool[slotCount];
            slotShapes = new PieceShape[slotCount];
            slotPieceObjects = new GameObject[slotCount];
            RefillTray();
        }

        private void RefillTray()
        {
            for (int i = 0; i < slotCount; i++)
            {
                SpawnPieceAtSlot(i);
                slotEmpty[i] = false;
            }
        }

        private void SpawnPieceAtSlot(int slotIndex)
        {
            PieceShape shape = ChooseShape();
            slotShapes[slotIndex] = shape;

            Vector3 slotPosition = GetSlotPosition(slotIndex);
            GameObject pieceObject = Instantiate(piecePrefab, slotPosition, Quaternion.identity, transform);
            pieceObject.transform.localScale = Vector3.one * traySlotScale;
            slotPieceObjects[slotIndex] = pieceObject;

            PieceView pieceView = pieceObject.GetComponent<PieceView>();
            pieceView.Initialize(shape);

            PieceDragHandler dragHandler = pieceObject.GetComponent<PieceDragHandler>();
            dragHandler.Initialize(boardView, this, slotIndex);

            SlotRefilled?.Invoke(slotIndex);
        }

        public void NotifyPieceUsed(int slotIndex)
        {
            slotEmpty[slotIndex] = true;
            slotShapes[slotIndex] = null;
            slotPieceObjects[slotIndex] = null;

            if (AreAllSlotsEmpty())
            {
                RefillTray();
            }

            CheckGameOver();
        }

        public bool IsSlotEmpty(int slotIndex)
        {
            return slotEmpty[slotIndex];
        }

        public GameObject GetSlotPieceObject(int slotIndex)
        {
            return slotPieceObjects[slotIndex];
        }

        public void RecheckGameOver()
        {
            CheckGameOver();
        }

        public void ForceReplaceSlot(int slotIndex)
        {
            if (slotEmpty[slotIndex])
            {
                return;
            }

            if (slotPieceObjects[slotIndex] != null)
            {
                Destroy(slotPieceObjects[slotIndex]);
            }

            SpawnPieceAtSlot(slotIndex);
        }

        private bool AreAllSlotsEmpty()
        {
            foreach (bool empty in slotEmpty)
            {
                if (!empty)
                {
                    return false;
                }
            }

            return true;
        }

        private PieceShape ChooseShape()
        {
            Board board = boardView.Board;
            List<PieceShape> placeableShapes = GetPlaceableShapes(board);
            IReadOnlyList<PieceShape> pool = placeableShapes.Count > 0 ? placeableShapes : availableShapes;

            float fillRatio = board.CountOccupiedCells() / (float)(Board.Width * Board.Height);
            float difficultyFactor = Mathf.Clamp01(scoreManager.Score / (float)difficultyRampScore);

            return WeightedRandomShape(pool, board, fillRatio, difficultyFactor);
        }

        private List<PieceShape> GetPlaceableShapes(Board board)
        {
            List<PieceShape> placeable = new List<PieceShape>();

            foreach (PieceShape shape in availableShapes)
            {
                if (board.CanPlaceShapeAnywhere(shape.cells))
                {
                    placeable.Add(shape);
                }
            }

            return placeable;
        }

        private PieceShape WeightedRandomShape(IReadOnlyList<PieceShape> pool, Board board, float fillRatio, float difficultyFactor)
        {
            int[] effectiveWeights = new int[pool.Count];
            int totalWeight = 0;

            for (int i = 0; i < pool.Count; i++)
            {
                effectiveWeights[i] = GetEffectiveWeight(pool[i], board, fillRatio, difficultyFactor);
                totalWeight += effectiveWeights[i];
            }

            int roll = Random.Range(0, totalWeight);
            int cumulative = 0;

            for (int i = 0; i < pool.Count; i++)
            {
                cumulative += effectiveWeights[i];
                if (roll < cumulative)
                {
                    return pool[i];
                }
            }

            return pool[pool.Count - 1];
        }

        private int GetEffectiveWeight(PieceShape shape, Board board, float fillRatio, float difficultyFactor)
        {
            float weight = shape.weight;

            // Oyun ilerledikçe (skor arttıkça) büyük parçalar kademeli olarak biraz daha sık gelir.
            weight += difficultyFactor * shape.cells.Length * difficultyBonusPerCell;

            // Tahta dolulaştıkça büyük parçalara karşı bir koruma uygulanır; zorluk arttıkça bu koruma zayıflar
            // (erken oyunda daha cömert, geç oyunda daha az cömert - "insani" bir zorlaşma eğrisi).
            float protectionFactor = Mathf.Lerp(0.5f, 0.85f, difficultyFactor);

            if (fillRatio > 0.5f && shape.cells.Length >= 5)
            {
                weight *= protectionFactor;
            }

            if (fillRatio > 0.7f && shape.cells.Length >= 4)
            {
                weight *= protectionFactor;
            }

            // Tahtadaki boşluklara/kenarlara iyi oturan şekiller biraz daha sık gelir (akıcılık).
            int fitScore = board.GetBestFitScore(shape.cells);
            weight += fitScore * fitBonusPerEdge;

            // Şu an gerçekten bir satır/sütun tamamlayabiliyorsa belirgin bonus verilir.
            if (board.CanShapeCompleteLineAnywhere(shape.cells))
            {
                weight *= lineClearWeightMultiplier;
            }

            return Mathf.Max(1, Mathf.RoundToInt(weight));
        }

        private void CheckGameOver()
        {
            for (int i = 0; i < slotCount; i++)
            {
                bool slotHasPlaceablePiece = !slotEmpty[i] && boardView.Board.CanPlaceShapeAnywhere(slotShapes[i].cells);

                if (slotHasPlaceablePiece)
                {
                    return;
                }
            }

            gameOverUI.Show();
        }

        private Vector3 GetSlotPosition(int slotIndex)
        {
            float x = (slotIndex - (slotCount - 1) / 2f) * slotSpacing;
            return new Vector3(x, traySlotY, 0f);
        }
    }
}
