using UnityEngine;

namespace BlockBrawl.Pieces
{
    public class PieceTray : MonoBehaviour
    {
        [SerializeField] private PieceShape[] availableShapes;
        [SerializeField] private GameObject piecePrefab;
        [SerializeField] private int slotCount = 3;
        [SerializeField] private float slotSpacing = 2.5f;
        [SerializeField] private float traySlotY = -5f;

        private void Start()
        {
            RefillTray();
        }

        private void RefillTray()
        {
            for (int i = 0; i < slotCount; i++)
            {
                SpawnPieceAtSlot(i);
            }
        }

        private void SpawnPieceAtSlot(int slotIndex)
        {
            PieceShape shape = availableShapes[Random.Range(0, availableShapes.Length)];
            Vector3 slotPosition = GetSlotPosition(slotIndex);
            GameObject pieceObject = Instantiate(piecePrefab, slotPosition, Quaternion.identity, transform);
            PieceView pieceView = pieceObject.GetComponent<PieceView>();
            pieceView.Initialize(shape);
        }

        private Vector3 GetSlotPosition(int slotIndex)
        {
            float x = (slotIndex - (slotCount - 1) / 2f) * slotSpacing;
            return new Vector3(x, traySlotY, 0f);
        }
    }
}
