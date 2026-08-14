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
