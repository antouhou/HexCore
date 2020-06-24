using System;
using HexCore.DataStructures;

namespace HexCore.HexGraph
{
    [Serializable]
    public class CellState
    {
        public Coordinate3D Coordinate3;

        // Never set this field directly, only through graph.SetCellBlocked, since changing cell state requires
        // rebuild of some graph coordinates.
        public bool IsBlocked;
        public ITerrainType TerrainType;

        public CellState(bool isBlocked, Coordinate3D coordinate3, ITerrainType terrainType)
        {
            IsBlocked = isBlocked;
            Coordinate3 = coordinate3;
            TerrainType = terrainType;
        }
    }
}