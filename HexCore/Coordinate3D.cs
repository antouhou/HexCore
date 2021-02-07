using System;
using System.Collections.Generic;

namespace HexCore
{
    [Serializable]
    public struct Coordinate3D : IEquatable<Coordinate3D>
    {
        public int X, Y, Z;

        public Coordinate3D(int x, int y, int z)
        {
            if (x + y + z != 0)
                throw new InvalidOperationException(
                    "Sum of all points in 3D coordinate should always be equal to zero");

            X = x;
            Y = y;
            Z = z;
        }

        public bool Equals(Coordinate3D other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        public Coordinate2D To2D(OffsetTypes offsetType)
        {
            int col, row;
            switch (offsetType)
            {
                case OffsetTypes.OddRowsRight:
                    col = X + (Z - Z % 2) / 2;
                    row = Z;
                    return new Coordinate2D(col, row, offsetType);
                case OffsetTypes.EvenRowsRight:
                    col = X + (Z + Z % 2) / 2;
                    row = Z;
                    return new Coordinate2D(col, row, offsetType);
                case OffsetTypes.OddColumnsDown:
                    col = X;
                    row = Z + (Z - Z % 2) / 2;
                    return new Coordinate2D(col, row, offsetType);
                case OffsetTypes.EvenColumnsDown:
                    col = X;
                    row = Z + (Z + Z % 2) / 2;
                    return new Coordinate2D(col, row, offsetType);
                default:
                    throw new ArgumentOutOfRangeException(nameof(offsetType), offsetType, null);
            }
        }

        public static Coordinate3D RotateRight(Coordinate3D center, Coordinate3D position)
        {
            var vector = position - center;
            var rotation = new Coordinate3D(-vector.Z, -vector.X, -vector.Y);
            var result = rotation + center;
            return result;
        }

        public static List<Coordinate2D> To2D(List<Coordinate3D> coordinate3Ds, OffsetTypes offsetType,
            List<Coordinate2D> listToWriteTo = null)
        {
            if (listToWriteTo == null) listToWriteTo = new List<Coordinate2D>();

            foreach (var coordinate3D in coordinate3Ds) listToWriteTo.Add(coordinate3D.To2D(offsetType));

            return listToWriteTo;
        }

        public static Coordinate3D operator +(Coordinate3D a, Coordinate3D b)
        {
            return new Coordinate3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Coordinate3D operator -(Coordinate3D a, Coordinate3D b)
        {
            return new Coordinate3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Coordinate3D operator *(Coordinate3D a, int scalar)
        {
            return new Coordinate3D(a.X * scalar, a.Y * scalar, a.Z * scalar);
        }

        public override string ToString()
        {
            return $"({X}, {Y}, {Z})";
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Coordinate3D))
                return false;

            var other = (Coordinate3D) obj;
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        public override int GetHashCode()
        {
            return X * 10000 + Y * 1000 + Z;
        }
    }
}