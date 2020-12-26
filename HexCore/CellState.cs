using System;

namespace HexCore
{
    [Serializable]
    public class CellState
    {
        public Coordinate3D Coordinate3;

        // Never set this field directly, only through graph.BlockCells, since changing cell state requires
        // rebuild of some graph coordinates.
        public bool IsBlocked;
        public TerrainType TerrainType;

        public CellState(bool isBlocked, Coordinate3D coordinate3, TerrainType terrainType)
        {
            IsBlocked = isBlocked;
            Coordinate3 = coordinate3;
            TerrainType = terrainType;
        }

        public CellState(bool isBlocked, Coordinate2D coordinate2, TerrainType terrainType)
        {
            IsBlocked = isBlocked;
            Coordinate3 = coordinate2.To3D();
            TerrainType = terrainType;
        }
    }
}
