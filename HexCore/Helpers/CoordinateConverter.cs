using System;
using System.Collections.Generic;
using System.Linq;
using HexCore.DataStructures;
using HexCore.HexGraph;

namespace HexCore.Helpers
{
    public static class CoordinateConverter
    {
        public static Coordinate3D ConvertOneOffsetToCube(OffsetTypes offsetType, Coordinate2D offsetCoordinate)
        {
            var cubeCoordinates = ConvertManyOffsetToCube(offsetType, new List<Coordinate2D> {offsetCoordinate});
            return cubeCoordinates[0];
        }

        public static Coordinate2D ConvertOneCubeToOffset(OffsetTypes offsetType, Coordinate3D coordinate3D)
        {
            var offsetCoordinates = ConvertManyCubeToOffset(offsetType, new List<Coordinate3D> {coordinate3D});
            return offsetCoordinates[0];
        }

        /**
         * Converts offset coordinates to three axis cube coordinates.
         * In poinst X stands for column, Y - for row. When you think about it, Y axis is going up, representing rows,
         * and X axis going right, representing columns.
         */
        public static List<Coordinate3D> ConvertManyOffsetToCube(OffsetTypes offsetType,
            IEnumerable<Coordinate2D> offsetCoords)
        {
            switch (offsetType)
            {
                case OffsetTypes.OddRowsRight:
                    return offsetCoords.Select(point =>
                    {
                        var x = point.X - (point.Y - (point.Y % 2)) / 2;
                        var z = point.Y;
                        var y = -x - z;
                        return new Coordinate3D(x, y, z);
                    }).ToList();
                case OffsetTypes.EvenRowsRight:
                    return offsetCoords.Select(point =>
                    {
                        var x = point.X - (point.Y + (point.Y % 2)) / 2;
                        var z = point.Y;
                        var y = -x - z;
                        return new Coordinate3D(x, y, z);
                    }).ToList();
                case OffsetTypes.OddColumnsDown:
                    return offsetCoords.Select(point =>
                    {
                        var x = point.X;
                        var z = point.Y - (point.X - (point.X % 2)) / 2;
                        var y = -x - z;
                        return new Coordinate3D(x, y, z);
                    }).ToList();
                case OffsetTypes.EvenColumnsDown:
                    return offsetCoords.Select(point =>
                    {
                        var x = point.X;
                        var z = point.Y - (point.X + (point.X % 2)) / 2;
                        var y = -x - z;
                        return new Coordinate3D(x, y, z);
                    }).ToList();
                default:
                    throw new ArgumentOutOfRangeException(nameof(offsetType), offsetType, null);
            }
        }

        public static List<Coordinate2D> ConvertManyCubeToOffset(OffsetTypes offsetType,
            List<Coordinate3D> cubeCoordinates)
        {
            switch (offsetType)
            {
                case OffsetTypes.OddRowsRight:
                    return cubeCoordinates.Select(cube =>
                    {
                        var col = cube.X + (cube.Z - (cube.Z % 2)) / 2;
                        var row = cube.Z;
                        return new Coordinate2D(col, row);
                    }).ToList();
                case OffsetTypes.EvenRowsRight:
                    return cubeCoordinates.Select(cube =>
                    {
                        var col = cube.X + (cube.Z + (cube.Z % 2)) / 2;
                        var row = cube.Z;
                        return new Coordinate2D(col, row);
                    }).ToList();
                case OffsetTypes.OddColumnsDown:
                    return cubeCoordinates.Select(cube =>
                    {
                        var col = cube.X;
                        var row = cube.Z + (cube.Z - (cube.Z % 2)) / 2;
                        return new Coordinate2D(col, row);
                    }).ToList();
                case OffsetTypes.EvenColumnsDown:
                    return cubeCoordinates.Select(cube =>
                    {
                        var col = cube.X;
                        var row = cube.Z + (cube.Z + (cube.Z % 2)) / 2;
                        return new Coordinate2D(col, row);
                    }).ToList();
                default:
                    throw new ArgumentOutOfRangeException(nameof(offsetType), offsetType, null);
            }
        }
    }
}