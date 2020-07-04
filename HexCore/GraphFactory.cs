namespace HexCore
{
    public static class GraphFactory
    {
        public static Graph CreateRectangularGraph(int width, int height,
            MovementTypes movementTypes,
            ITerrainType defaultTerrainType,
            OffsetTypes offsetType = OffsetTypes.OddRowsRight)
        {
            var graph = new Graph(new CellState[] { }, movementTypes);

            GraphUtils.ResizeSquareGraph(graph, offsetType, width, height, defaultTerrainType);

            return graph;
        }
    }
}