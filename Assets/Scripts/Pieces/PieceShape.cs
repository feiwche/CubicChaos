using UnityEngine;

namespace CubicChaos.Pieces
{
    [CreateAssetMenu(fileName = "NewPieceShape", menuName = "CubicChaos/Piece Shape")]
    public class PieceShape : ScriptableObject
    {
        public Vector2Int[] cells;
        public int weight = 10;
    }
}
