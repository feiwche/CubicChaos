using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using BlockBrawl.Core;
using BlockBrawl.Grid;
using BlockBrawl.Pieces;

namespace BlockBrawl.Samata
{
    public class PowerUpActivator : MonoBehaviour
    {
        [SerializeField] private PowerUpInventory inventory;
        [SerializeField] private ChaosMeter chaosMeter;
        [SerializeField] private PieceTray pieceTray;
        [SerializeField] private ScoreManager scoreManager;
        [SerializeField] private BoardView boardView;

        [SerializeField] private Button bombaButton;
        [SerializeField] private Button dondurmaButton;
        [SerializeField] private Button karistirmaButton;
        [SerializeField] private Button ciftPuanButton;

        [SerializeField] private float dondurmaDuration = 8f;
        [SerializeField] private float ciftPuanDuration = 10f;

        private bool waitingForBombTarget;

        private void Awake()
        {
            bombaButton.onClick.AddListener(ActivateBomba);
            dondurmaButton.onClick.AddListener(ActivateDondurma);
            karistirmaButton.onClick.AddListener(ActivateKaristirma);
            ciftPuanButton.onClick.AddListener(ActivateCiftPuan);
        }

        private void Update()
        {
            if (!waitingForBombTarget || Mouse.current == null)
            {
                return;
            }

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Vector3 worldPoint = GetPointerWorldPosition();
                Vector2Int gridPosition = boardView.WorldToGridPosition(worldPoint);
                boardView.DestroyCellAt(gridPosition.x, gridPosition.y);
                waitingForBombTarget = false;
            }
        }

        public void ActivateBomba()
        {
            if (inventory.TrySpend(PowerUpType.Bomba))
            {
                waitingForBombTarget = true;
            }
        }

        public void ActivateDondurma()
        {
            if (inventory.TrySpend(PowerUpType.Dondurma))
            {
                chaosMeter.Freeze(dondurmaDuration);
            }
        }

        public void ActivateKaristirma()
        {
            if (inventory.TrySpend(PowerUpType.Karistirma))
            {
                pieceTray.ForceRefillAll();
            }
        }

        public void ActivateCiftPuan()
        {
            if (inventory.TrySpend(PowerUpType.CiftPuan))
            {
                scoreManager.ActivateDoubleScore(ciftPuanDuration);
            }
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
