using UnityEngine;

namespace CubicChaos.Core
{
    [CreateAssetMenu(fileName = "NewBlockSkin", menuName = "CubicChaos/Block Skin")]
    public class BlockSkinDefinition : ScriptableObject
    {
        public string skinId;
        public string displayName;
        public int coinCost;
        public Color emptyCellColor = new Color(0.2901961f, 0.43137255f, 0.5411765f);
        public Color blockCellColor = new Color(0.8509804f, 0.4784314f, 0.24705882f);
    }
}
