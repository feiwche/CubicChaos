using System.Collections.Generic;
using UnityEngine;
using BlockBrawl.Grid;
using BlockBrawl.UI;

namespace BlockBrawl.Pieces
{
    public class PieceTray : MonoBehaviour
    {
        [SerializeField] private BoardView boardView;
        [SerializeField] private GameOverUI gameOverUI;
        [SerializeField] private PieceShape[] availableShapes;
        [SerializeField] private GameObject piecePrefab;
        [SerializeField] private int slotCount = 3;
        [SerializeField] private float slotSpacing = 2.5f;
        [SerializeField] private float traySlotY = -5f;
        [SerializeField] private float traySlotScale = 0.4f;

        private bool[] slotEmpty;
        private PieceShape[] slotShapes;

        private void Start()
        {
            slotEmpty = new bool[slotCount];
            slotShapes = new PieceShape[slotCount];
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

            PieceView pieceView = pieceObject.GetComponent<PieceView>();
            pieceView.Initialize(shape);

            PieceDragHandler dragHandler = pieceObject.GetComponent<PieceDragHandler>();
            dragHandler.Initialize(boardView, this, slotIndex);
        }

        public void NotifyPieceUsed(int slotIndex)
        {
            slotEmpty[slotIndex] = true;
            slotShapes[slotIndex] = null;

            if (AreAllSlotsEmpty())
            {
                RefillTray();
            }

            CheckGameOver();
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
            List<PieceShape> placeableShapes = GetPlaceableShapes();
            IReadOnlyList<PieceShape> pool = placeableShapes.Count > 0 ? placeableShapes : availableShapes;
            return WeightedRandomShape(pool);
        }

        private List<PieceShape> GetPlaceableShapes()
        {
            List<PieceShape> placeable = new List<PieceShape>();

            foreach (PieceShape shape in availableShapes)
            {
                if (boardView.Board.CanPlaceShapeAnywhere(shape.cells))
                {
                    placeable.Add(shape);
                }
            }

            return placeable;
        }

        private PieceShape WeightedRandomShape(IReadOnlyList<PieceShape> pool)
        {
            int totalWeight = 0;
            foreach (PieceShape shape in pool)
            {
                totalWeight += shape.weight;
            }

            int roll = Random.Range(0, totalWeight);
            int cumulative = 0;

            foreach (PieceShape shape in pool)
            {
                cumulative += shape.weight;
                if (roll < cumulative)
                {
                    return shape;
                }
            }

            return pool[pool.Count - 1];
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
