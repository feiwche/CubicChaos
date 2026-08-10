using System.Collections.Generic;
using UnityEngine;

namespace BlockBrawl.Grid
{
    public class Board
    {
        public const int Width = 8;
        public const int Height = 8;

        private readonly bool[,] occupied = new bool[Width, Height];

        public bool IsInsideBoard(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }

        public bool IsCellOccupied(int x, int y)
        {
            return occupied[x, y];
        }

        public void SetOccupied(int x, int y, bool value)
        {
            occupied[x, y] = value;
        }

        public bool CanPlaceCells(IEnumerable<Vector2Int> relativeCells, int originX, int originY)
        {
            foreach (Vector2Int cell in relativeCells)
            {
                int x = originX + cell.x;
                int y = originY + cell.y;

                if (!IsInsideBoard(x, y) || IsCellOccupied(x, y))
                {
                    return false;
                }
            }

            return true;
        }

        public void PlaceCells(IEnumerable<Vector2Int> relativeCells, int originX, int originY)
        {
            foreach (Vector2Int cell in relativeCells)
            {
                SetOccupied(originX + cell.x, originY + cell.y, true);
            }
        }
    }
}
