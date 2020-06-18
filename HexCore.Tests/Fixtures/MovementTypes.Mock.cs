using System.Collections.Generic;
using HexCore.HexGraph;

namespace HexCoreTests.Fixtures
{
    public static class MovementTypesFixture
    {
        public static readonly MovementType Ground = new MovementType(1, "Ground");
        public static readonly MovementType Water = new MovementType(2, "Water");
        public static readonly MovementType Air = new MovementType(3, "Air");

        public static MovementTypes GetMovementTypes()
        {
            var movementTypes = new MovementTypes(
                new Dictionary<IMovementType, Dictionary<IMovementType, int>>
                {
                    [Ground] = new Dictionary<IMovementType, int>
                    {
                        [Ground] = 1,
                        [Water] = 2,
                        [Air] = 999
                    },
                    [Water] = new Dictionary<IMovementType, int>
                    {
                        [Ground] = 2,
                        [Water] = 1,
                        [Air] = 999
                    },
                    [Air] = new Dictionary<IMovementType, int>
                    {
                        [Ground] = 1,
                        [Water] = 1,
                        [Air] = 1
                    }
                }
            );
            return movementTypes;
        }
    }
}