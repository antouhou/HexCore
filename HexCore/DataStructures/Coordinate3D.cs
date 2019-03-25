﻿using System;
using System.Collections.Generic;
using System.Linq;
using HexCore.HexGraph;

namespace HexCore.DataStructures
{
    [Serializable]
    public struct Coordinate3D
    {
        public readonly int X, Y, Z;

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

        public static List<Coordinate2D> To2D(IEnumerable<Coordinate3D> coordinate3Ds, OffsetTypes offsetType)
        {
            return coordinate3Ds.Select(coordinate3D => coordinate3D.To2D(offsetType)).ToList();
        }

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