using System.Collections.Generic;

namespace HexCore.HexGraph
{
    public static class BasicMovementTypes
    {
        public static readonly MovementType Ground = new MovementType
        {
            Name = "ground",
            MovementCostTo =
            {
                {"ground", 1},
                {"water", 2}
            }
        };

        public static readonly MovementType Water = new MovementType
        {
            Name = "water",
            MovementCostTo =
            {
                {"ground", 2},
                {"water", 1}
            }
        };

        public static readonly List<MovementType> TypesList = new List<MovementType>
        {
            Ground,
            Water
        };
    }
}