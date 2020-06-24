using System.Collections.Generic;
using HexCore.HexGraph;

namespace HexCoreTests.Fixtures
{
    public static class MovementTypesFixture
    {
        public static readonly MovementType Walking = new MovementType(1, " Walking");
        public static readonly MovementType Swimming = new MovementType(2, "Swimming");
        public static readonly MovementType Flying = new MovementType(3, "Flying");
        
        public static readonly TerrainType Ground = new TerrainType(1, "Ground");
        public static readonly TerrainType Water = new TerrainType(2, "Water");
        public static readonly TerrainType Air = new TerrainType(3, "Air");

        public static MovementTypes GetMovementTypes()
        {
            ITerrainType[] terrainTypes = {Ground, Water, Air};
            var movementTypes = new MovementTypes(terrainTypes, new Dictionary<IMovementType, Dictionary<ITerrainType, int>>
            {
                [Walking] = new Dictionary<ITerrainType, int>
                {
                    [Ground] = 1,
                    [Water] = 2,
                    [Air] = 999
                },
                [Swimming] = new Dictionary<ITerrainType, int>
                {
                    [Ground] = 2,
                    [Water] = 1,
                    [Air] = 999
                },
                [Flying] = new Dictionary<ITerrainType, int>
                {
                    [Ground] = 1,
                    [Water] = 1,
                    [Air] = 1
                }
            });
            return movementTypes;
        }
    }
}