using System;
using System.Collections.Generic;
using System.Linq;

namespace HexCore
{
    [Serializable]
    public struct Coordinate2D
    {
        public int X, Y;
        public OffsetTypes OffsetType;

        public Coordinate3D To3D()
        {
            int x, y, z;
            switch (OffsetType)
            {
                case OffsetTypes.OddRowsRight:
                    x = X - (Y - Y % 2) / 2;
                    z = Y;
                    y = -x - z;
                    return new Coordinate3D(x, y, z);
                case OffsetTypes.EvenRowsRight:
                    x = X - (Y + Y % 2) / 2;
                    z = Y;
                    y = -x - z;
                    return new Coordinate3D(x, y, z);
                case OffsetTypes.OddColumnsDown:
                    x = X;
                    z = Y - (X - X % 2) / 2;
                    y = -x - z;
                    return new Coordinate3D(x, y, z);
                case OffsetTypes.EvenColumnsDown:
                    x = X;
                    z = Y - (X + X % 2) / 2;
                    y = -x - z;
                    return new Coordinate3D(x, y, z);
                default:
                    throw new ArgumentOutOfRangeException(nameof(OffsetType), OffsetType, null);
            }
        }

        public static List<Coordinate3D> To3D(IEnumerable<Coordinate2D> coordinate2Ds)
        {
            return coordinate2Ds.Select(coordinate2D => coordinate2D.To3D()).ToList();
        }

        public Coordinate2D(int x, int y, OffsetTypes offsetType)
        {
            X = x;
            Y = y;
            OffsetType = offsetType;
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}
