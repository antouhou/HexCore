using System.Collections.Generic;
using HexCore.DataStructures;

namespace HexCore.HexGraph
{
    public static class GraphFactory
    {
        public static Graph CreateRectangularGraph(int width, int height,
            MovementType defaultMovementType,
            OffsetTypes offsetType = OffsetTypes.OddRowsRight)
        {
            var cells = new List<CellState>();

            var graph = new Graph(cells);

            GraphUtils.ResizeSquareGraph(graph, offsetType, width, height, defaultMovementType);

            return graph;
        }
    }
}