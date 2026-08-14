using TMPro;
using UnityEngine;

namespace CubicChaos.Samata
{
    public class PowerUpInventoryUI : MonoBehaviour
    {
        [SerializeField] private PowerUpInventory inventory;
        [SerializeField] private TMP_Text inventoryText;

        private void OnEnable()
        {
            inventory.InventoryChanged += HandleInventoryChanged;
            RefreshText();
        }

        private void OnDisable()
        {
            inventory.InventoryChanged -= HandleInventoryChanged;
        }

        private void HandleInventoryChanged(PowerUpType type, int count)
        {
            RefreshText();
        }

        private void RefreshText()
        {
            int bomba = inventory.GetCount(PowerUpType.Bomba);
            int dondurma = inventory.GetCount(PowerUpType.Dondurma);
            int karistirma = inventory.GetCount(PowerUpType.Karistirma);
            int ciftPuan = inventory.GetCount(PowerUpType.CiftPuan);

            inventoryText.text = $"B:{bomba} D:{dondurma} K:{karistirma} 2x:{ciftPuan}";
        }
    }
}
