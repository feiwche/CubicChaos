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
    }
}
