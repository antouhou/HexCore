using System.Collections.Generic;
using HexCore.HexGraph;

namespace HexCoreTests.Mocks
{
    public static class MovementTypes
    {
        public static readonly MovementType Ground = new MovementType
        {
            Name = "ground",
            MovementCostTo =
            {
                {"ground", 1},
                {"forest", 2},
                {"water", 2}
            }
        };

        public static readonly MovementType Water = new MovementType
        {
            Name = "water",
            MovementCostTo =
            {
                {"ground", 2},
                {"forest", 2},
                {"water", 1}
            }
        };

        public static readonly MovementType Forest = new MovementType
        {
            Name = "forest",
            MovementCostTo =
            {
                {"ground", 1},
                {"forest", 1},
                {"water", 3}
            }
        };

        public static readonly List<MovementType> TypesList = new List<MovementType>
        {
            Ground,
            Forest,
            Water
        };
    }
}