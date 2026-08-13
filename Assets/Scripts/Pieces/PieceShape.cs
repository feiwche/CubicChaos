using UnityEngine;

namespace BlockBrawl.Pieces
{
    [CreateAssetMenu(fileName = "NewPieceShape", menuName = "BlockBrawl/Piece Shape")]
    public class PieceShape : ScriptableObject
    {
        public Vector2Int[] cells;
        public int weight = 10;
    }
}
