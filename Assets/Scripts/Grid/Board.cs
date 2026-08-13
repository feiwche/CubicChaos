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

        public bool CanPlaceShapeAnywhere(IEnumerable<Vector2Int> relativeCells)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (CanPlaceCells(relativeCells, x, y))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public List<int> GetFullRows()
        {
            List<int> fullRows = new List<int>();

            for (int y = 0; y < Height; y++)
            {
                if (IsRowFull(y))
                {
                    fullRows.Add(y);
                }
            }

            return fullRows;
        }

        public List<int> GetFullColumns()
        {
            List<int> fullColumns = new List<int>();

            for (int x = 0; x < Width; x++)
            {
                if (IsColumnFull(x))
                {
                    fullColumns.Add(x);
                }
            }

            return fullColumns;
        }

        private bool IsRowFull(int y)
        {
            for (int x = 0; x < Width; x++)
            {
                if (!IsCellOccupied(x, y))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsColumnFull(int x)
        {
            for (int y = 0; y < Height; y++)
            {
                if (!IsCellOccupied(x, y))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
