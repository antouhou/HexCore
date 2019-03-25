using System.Collections.Generic;
using System.Linq;
using HexCore.DataStructures;

namespace HexCore.HexGraph
{
    public static class GraphUtils
    {
        public static void ResizeSquareGraph(Graph graph, OffsetTypes offsetType, int newWidth, int newHeight,
            MovementType defaultMovementType)
        {
            var offsetCoordinates =
                Coordinate3D.To2D(graph.GetAllCellsCoordinates(), offsetType);

            var coordinatesInitialized = offsetCoordinates.Count > 0;

            var width = coordinatesInitialized ? offsetCoordinates.Select(item => item.X).Max() + 1 : 0;
            var height = coordinatesInitialized ? offsetCoordinates.Select(item => item.Y).Max() + 1 : 0;

            if (width > newWidth)
            {
                var coordinatesToRemove =
                    Coordinate2D.To3D(offsetCoordinates.Where(coordinate => coordinate.X >= newWidth).ToList());
                graph.RemoveCells(coordinatesToRemove);
            }

            if (height > newHeight)
            {
                var coordinatesToRemove =
                    Coordinate2D.To3D(offsetCoordinates.Where(coordinate => coordinate.Y >= newHeight).ToList());
                graph.RemoveCells(coordinatesToRemove);
            }

            if (newWidth > width)
            {
                var cellsToAdd = new List<CellState>();
                // We'll add new columns width already updated height
                for (var x = width; x < newWidth; x++)
                    cellsToAdd.AddRange(CreateNewCellsForColumn(x, 0, newHeight, defaultMovementType,
                        offsetType));

                graph.AddCells(cellsToAdd);
            }

            if (newHeight > height)
            {
                var cellsToAdd = new List<CellState>();
                // New columns already will have correct height; So only old needs to be resized.
                for (var x = 0; x < width; x++)
                    cellsToAdd.AddRange(CreateNewCellsForColumn(x, height, newHeight, defaultMovementType,
                        offsetType));

                graph.AddCells(cellsToAdd);
            }
        }

        private static List<CellState> CreateNewCellsForColumn(int x, int oldY, int newY,
            MovementType defaultMovementType, OffsetTypes offsetType)
        {
            var newCells = new List<CellState>();
            for (var y = oldY; y < newY; y++)
            {
                var position = new Coordinate2D(x, y, OffsetTypes.OddRowsRight);
                var cubeCoordinate = position.To3D();
                newCells.Add(new CellState(false, cubeCoordinate, defaultMovementType));
            }

            return newCells;
        }
    }
}