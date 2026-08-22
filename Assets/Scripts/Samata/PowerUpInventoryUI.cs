using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CubicChaos.Samata
{
    public class PowerUpInventoryUI : MonoBehaviour
    {
        [SerializeField] private PowerUpInventory inventory;
        [SerializeField] private TMP_Text bombaBadge;
        [SerializeField] private TMP_Text dondurmaBadge;
        [SerializeField] private TMP_Text karistirmaBadge;
        [SerializeField] private TMP_Text ciftPuanBadge;
        [SerializeField] private Transform bombaButton;
        [SerializeField] private Transform dondurmaButton;
        [SerializeField] private Transform karistirmaButton;
        [SerializeField] private Transform ciftPuanButton;

        private void OnEnable()
        {
            inventory.InventoryChanged += HandleInventoryChanged;
            RefreshBadges();
        }

        private void OnDisable()
        {
            inventory.InventoryChanged -= HandleInventoryChanged;
        }

        private void HandleInventoryChanged(PowerUpType type, int count)
        {
            RefreshBadges();
            PlayPickupAnimation(type);
        }

        private void PlayPickupAnimation(PowerUpType type)
        {
            Transform target = type switch
            {
                PowerUpType.Bomba => bombaButton,
                PowerUpType.Dondurma => dondurmaButton,
                PowerUpType.Karistirma => karistirmaButton,
                PowerUpType.CiftPuan => ciftPuanButton,
                _ => null
            };

            target?.DOPunchScale(Vector3.one * 0.35f, 0.4f, 6, 0.5f);
        }

        private void RefreshBadges()
        {
            bombaBadge.text = inventory.GetCount(PowerUpType.Bomba).ToString();
            dondurmaBadge.text = inventory.GetCount(PowerUpType.Dondurma).ToString();
            karistirmaBadge.text = inventory.GetCount(PowerUpType.Karistirma).ToString();
            ciftPuanBadge.text = inventory.GetCount(PowerUpType.CiftPuan).ToString();
        }
    }
}
