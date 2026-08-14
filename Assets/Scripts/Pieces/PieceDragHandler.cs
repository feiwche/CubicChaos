using UnityEngine;
using UnityEngine.InputSystem;
using CubicChaos.Grid;

namespace CubicChaos.Pieces
{
    [RequireComponent(typeof(PieceView))]
    public class PieceDragHandler : MonoBehaviour
    {
        private const float BoardScale = 1f;

        private PieceView pieceView;
        private BoardView boardView;
        private PieceTray tray;
        private int slotIndex;
        private Vector3 traySlotPosition;
        private Vector3 traySlotScale;
        private bool isDragging;

        private void Awake()
        {
            pieceView = GetComponent<PieceView>();
        }

        public void Initialize(BoardView board, PieceTray parentTray, int assignedSlotIndex)
        {
            boardView = board;
            tray = parentTray;
            slotIndex = assignedSlotIndex;
            traySlotPosition = transform.position;
            traySlotScale = transform.localScale;
        }

        private void Update()
        {
            if (Mouse.current == null)
            {
                return;
            }

            if (isDragging)
            {
                FollowPointer();

                if (Mouse.current.leftButton.wasReleasedThisFrame)
                {
                    EndDrag();
                }
            }
            else if (Mouse.current.leftButton.wasPressedThisFrame && IsPointerOverThisPiece())
            {
                StartDrag();
            }
        }

        private bool IsPointerOverThisPiece()
        {
            Vector3 worldPoint = GetPointerWorldPosition();
            Collider2D hit = Physics2D.OverlapPoint(worldPoint);
            return hit != null && hit.gameObject == gameObject;
        }

        private void StartDrag()
        {
            isDragging = true;
            transform.localScale = Vector3.one * BoardScale;
        }

        private void FollowPointer()
        {
            transform.position = GetPointerWorldPosition();
        }

        private void EndDrag()
        {
            isDragging = false;

            Vector3 shapeOriginWorldPosition = pieceView.GetShapeOriginWorldPosition();
            Vector2Int gridOrigin = boardView.WorldToGridPosition(shapeOriginWorldPosition);
            bool canPlace = boardView.Board.CanPlaceCells(pieceView.Shape.cells, gridOrigin.x, gridOrigin.y);

            if (canPlace)
            {
                boardView.PlaceShapeBlocks(pieceView.Shape.cells, gridOrigin.x, gridOrigin.y);
                Destroy(gameObject);
                tray.NotifyPieceUsed(slotIndex);
            }
            else
            {
                ReturnToTray();
            }
        }

        private void ReturnToTray()
        {
            transform.position = traySlotPosition;
            transform.localScale = traySlotScale;
        }

        private Vector3 GetPointerWorldPosition()
        {
            Vector2 screenPosition = Mouse.current.position.ReadValue();
            Vector3 screenPoint = new Vector3(screenPosition.x, screenPosition.y, -Camera.main.transform.position.z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPoint);
            worldPosition.z = 0f;
            return worldPosition;
        }
    }
}
