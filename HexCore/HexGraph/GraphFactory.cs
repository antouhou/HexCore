using System.Collections.Generic;
using HexCore.DataStructures;

namespace HexCore.HexGraph
{
    public static class GraphFactory
    {
        public static Graph CreateSquareGraph(int width, int height, OffsetTypes offsetType = OffsetTypes.OddRowsRight,
            MovementType defaultMovementType = null
        )
        {
            var cells = new List<CellState>();

            var graph = new Graph(cells);

            if (defaultMovementType == null) defaultMovementType = BasicMovementTypes.Ground;

            GraphUtils.ResizeSquareGraph(graph, offsetType, width, height, defaultMovementType);

            return graph;
        }
    }
}