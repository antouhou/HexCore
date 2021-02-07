using System.Linq;

namespace HexCore
{
    public static class GraphFactory
    {
        public static Graph CreateRectangularGraph(int width, int height,
            MovementTypes movementTypes,
            TerrainType defaultTerrainType,
            OffsetTypes offsetType = OffsetTypes.OddRowsRight)
        {
            var graph = new Graph(new CellState[] { }.ToList(), movementTypes);

            GraphUtils.ResizeSquareGraph(graph, offsetType, width, height, defaultTerrainType);

            return graph;
        }
    }
}