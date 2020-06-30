using System.Collections.Generic;
using System.Linq;

namespace HexCore
{
    public static class GraphUtils
    {
        public static void ResizeSquareGraph(Graph graph, OffsetTypes offsetType, int newWidth, int newHeight,
            ITerrainType defaultTerrainType)
        {
            var offsetCoordinates =
                Coordinate3D.To2D(graph.GetAllCellsCoordinates(), offsetType);

            var areCoordinatesInitialized = offsetCoordinates.Count > 0;

            var width = areCoordinatesInitialized ? offsetCoordinates.Select(item => item.X).Max() + 1 : 0;
            var height = areCoordinatesInitialized ? offsetCoordinates.Select(item => item.Y).Max() + 1 : 0;

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
                    cellsToAdd.AddRange(CreateNewCellsForColumn(x, 0, newHeight, defaultTerrainType,
                        offsetType));

                graph.AddCells(cellsToAdd);
            }

            if (newHeight <= height) return;
            {
                var cellsToAdd = new List<CellState>();
                // New columns already will have correct height; So only old needs to be resized.
                for (var x = 0; x < width; x++)
                    cellsToAdd.AddRange(CreateNewCellsForColumn(x, height, newHeight, defaultTerrainType,
                        offsetType));

                graph.AddCells(cellsToAdd);
            }
        }

        private static IEnumerable<CellState> CreateNewCellsForColumn(int x, int oldY, int newY,
            ITerrainType defaultTerrainType, OffsetTypes offsetType)
        {
            var newCells = new List<CellState>();
            for (var y = oldY; y < newY; y++)
            {
                var position = new Coordinate2D(x, y, offsetType);
                var cubeCoordinate = position.To3D();
                newCells.Add(new CellState(false, cubeCoordinate, defaultTerrainType));
            }

            return newCells;
        }
    }
}