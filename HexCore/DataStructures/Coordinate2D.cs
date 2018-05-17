namespace HexCore.DataStructures
{
    [System.Serializable]
    public struct Coordinate2D
    {
        public int X, Y;

        public Coordinate2D(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}