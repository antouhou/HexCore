using System;

namespace HexCore.DataStructures
{
    [Serializable]
    public struct Coordinate3D
    {
        public readonly int X, Y, Z;

        public Coordinate3D(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Coordinate3D operator +(Coordinate3D a, Coordinate3D b)
        {
            return new Coordinate3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Coordinate3D operator -(Coordinate3D a, Coordinate3D b)
        {
            return new Coordinate3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }
    }
}