using HexCore.HexGraph;

namespace HexCoreTests.Fixtures
{
    public static class MovementTypesFixture
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
    }
}