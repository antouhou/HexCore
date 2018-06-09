using System.Collections.Generic;

namespace HexCore.HexGraph
{
    public static class GraphFactory
    {
        public static Graph CreateSquareGraph(int width, int height, OffsetTypes offsetType,
            MovementType defaultMovementType
        )
        {
            var cells = new List<CellState>();

            var graph = new Graph(cells);

            GraphUtils.ResizeSquareGraph(graph, offsetType, width, height, defaultMovementType);

            return graph;
        }
    }
}