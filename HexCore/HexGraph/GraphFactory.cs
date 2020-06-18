using HexCore.DataStructures;

namespace HexCore.HexGraph
{
    public static class GraphFactory
    {
        public static Graph CreateRectangularGraph(int width, int height,
            MovementTypes movementTypes,
            MovementType defaultMovementType,
            OffsetTypes offsetType = OffsetTypes.OddRowsRight)
        {
            var graph = new Graph(new CellState[] { }, movementTypes);

            GraphUtils.ResizeSquareGraph(graph, offsetType, width, height, defaultMovementType);

            return graph;
        }
    }
}