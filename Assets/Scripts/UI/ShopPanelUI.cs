using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using CubicChaos.Core;

namespace CubicChaos.UI
{
    /// <summary>
    /// Blok teması mağazası. Her tema için hazır bir satır (buton + etiket +
    /// önizleme kutusu) sahnede duruyor; bu script onları skin listesine
    /// göre doldurur, satın alma / seçme akışını yürütür.
    /// </summary>
    public class ShopPanelUI : MonoBehaviour
    {
        [System.Serializable]
        public class SkinRow
        {
            public Button button;
            public TMP_Text nameLabel;
            public TMP_Text statusLabel;
            public Image previewBlock;
            public Image previewCell;
        }

        [SerializeField] private GameObject panel;
        [SerializeField] private Button openButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private TMP_Text coinLabel;
        [SerializeField] private BlockSkinDefinition[] skins;
        [SerializeField] private SkinRow[] rows;

        private static readonly Color SelectedColor = new Color(0.184f, 0.788f, 0.753f);
        private static readonly Color OwnedColor = new Color(0.961f, 0.969f, 0.98f);
        private static readonly Color LockedColor = new Color(0.91f, 0.761f, 0.239f);
        private static readonly Color TooExpensiveColor = new Color(0.62f, 0.4f, 0.4f);

        private void Awake()
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }

            if (openButton != null)
            {
                openButton.onClick.AddListener(Open);
            }

            if (closeButton != null)
            {
                closeButton.onClick.AddListener(Close);
            }

            BindRows();
        }

        private void BindRows()
        {
            if (rows == null || skins == null)
            {
                return;
            }

            for (int i = 0; i < rows.Length; i++)
            {
                if (rows[i] == null || rows[i].button == null)
                {
                    continue;
                }

                // Döngü değişkenini yakalamak için yerel kopya şart.
                int index = i;
                rows[i].button.onClick.AddListener(() => HandleRowClicked(index));
            }
        }

        private void Open()
        {
            AudioManager.ButtonClick();
            Refresh();

            if (panel != null)
            {
                panel.SetActive(true);
            }
        }

        private void Close()
        {
            AudioManager.ButtonClick();

            if (panel != null)
            {
                panel.SetActive(false);
            }
        }

        private void HandleRowClicked(int index)
        {
            if (skins == null || index < 0 || index >= skins.Length || skins[index] == null)
            {
                return;
            }

            BlockSkinDefinition skin = skins[index];

            if (SkinManager.IsUnlocked(skin.skinId))
            {
                SkinManager.Select(skin.skinId);
                AudioManager.ButtonClick();
            }
            else if (CurrencyManager.TrySpend(skin.coinCost))
            {
                SkinManager.Unlock(skin.skinId);
                SkinManager.Select(skin.skinId);
                AudioManager.Purchase();
            }
            else
            {
                // Yetersiz bakiye — sadece geri bildirim sesi.
                AudioManager.InvalidPlacement();
            }

            Refresh();
        }

        private void Refresh()
        {
            if (coinLabel != null)
            {
                coinLabel.text = $"Altın: {CurrencyManager.Coins}";
            }

            if (rows == null || skins == null)
            {
                return;
            }

            for (int i = 0; i < rows.Length; i++)
            {
                SkinRow row = rows[i];

                if (row == null)
                {
                    continue;
                }

                bool hasSkin = i < skins.Length && skins[i] != null;

                if (row.button != null)
                {
                    row.button.gameObject.SetActive(hasSkin);
                }

                if (!hasSkin)
                {
                    continue;
                }

                BlockSkinDefinition skin = skins[i];

                if (row.nameLabel != null)
                {
                    row.nameLabel.text = skin.displayName;
                }

                if (row.previewBlock != null)
                {
                    row.previewBlock.color = skin.blockCellColor;
                }

                if (row.previewCell != null)
                {
                    row.previewCell.color = skin.emptyCellColor;
                }

                if (row.statusLabel == null)
                {
                    continue;
                }

                bool unlocked = SkinManager.IsUnlocked(skin.skinId);
                bool selected = SkinManager.SelectedSkinId == skin.skinId;

                if (selected)
                {
                    row.statusLabel.text = "Seçili";
                    row.statusLabel.color = SelectedColor;
                }
                else if (unlocked)
                {
                    row.statusLabel.text = "Seç";
                    row.statusLabel.color = OwnedColor;
                }
                else
                {
                    row.statusLabel.text = $"{skin.coinCost} altın";
                    row.statusLabel.color = CurrencyManager.Coins >= skin.coinCost
                        ? LockedColor
                        : TooExpensiveColor;
                }
            }
        }
    }
}
