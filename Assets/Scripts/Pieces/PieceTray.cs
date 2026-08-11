using UnityEngine;
using BlockBrawl.Grid;

namespace BlockBrawl.Pieces
{
    public class PieceTray : MonoBehaviour
    {
        [SerializeField] private BoardView boardView;
        [SerializeField] private PieceShape[] availableShapes;
        [SerializeField] private GameObject piecePrefab;
        [SerializeField] private int slotCount = 3;
        [SerializeField] private float slotSpacing = 2.5f;
        [SerializeField] private float traySlotY = -5f;
        [SerializeField] private float traySlotScale = 0.4f;

        private bool[] slotEmpty;

        private void Start()
        {
            slotEmpty = new bool[slotCount];
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
            PieceShape shape = availableShapes[Random.Range(0, availableShapes.Length)];
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

            if (AreAllSlotsEmpty())
            {
                RefillTray();
            }
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

        private Vector3 GetSlotPosition(int slotIndex)
        {
            float x = (slotIndex - (slotCount - 1) / 2f) * slotSpacing;
            return new Vector3(x, traySlotY, 0f);
        }
    }
}
