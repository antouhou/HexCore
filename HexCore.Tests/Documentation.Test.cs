using System.Collections.Generic;
using HexCore;
using NUnit.Framework;

namespace HexCoreTests
{
    [TestFixture]
    public class Documentation
    {
        [Test]
        public void TerrainMovementTypeExample_ShouldWork()
        {
            var ground = new TerrainType(1, "Ground");
            var water = new TerrainType(2, "Water");

            var walkingType = new MovementType(1, "Walking");
            var swimmingType = new MovementType(2, "Swimming");

            var movementTypes = new MovementTypes(
                new[] {ground, water},
                new Dictionary<MovementType, Dictionary<TerrainType, int>>
                {
                    [walkingType] = new Dictionary<TerrainType, int>
                    {
                        [ground] = 1,
                        [water] = 2
                    },
                    [swimmingType] = new Dictionary<TerrainType, int>
                    {
                        [ground] = 2,
                        [water] = 1
                    }
                }
            );
        }

        [Test]
        public void CellStateExample_ShouldWork()
        {
            var ground = new TerrainType(1, "Ground");
            var water = new TerrainType(2, "Water");

            var walkingType = new MovementType(1, "Walking");
            var swimmingType = new MovementType(2, "Swimming");

            var movementTypes = new MovementTypes(
                new[] {ground, water},
                new Dictionary<MovementType, Dictionary<TerrainType, int>>
                {
                    [walkingType] = new Dictionary<TerrainType, int>
                    {
                        [ground] = 1,
                        [water] = 2
                    },
                    [swimmingType] = new Dictionary<TerrainType, int>
                    {
                        [ground] = 2,
                        [water] = 1
                    }
                }
            );

            var graph = new Graph(new List<CellState>
            {
                new CellState(false, new Coordinate2D(0, 0, OffsetTypes.OddRowsRight), ground),
                new CellState(false, new Coordinate2D(0, 1, OffsetTypes.OddRowsRight), ground),
                new CellState(true, new Coordinate2D(1, 0, OffsetTypes.OddRowsRight), water),
                new CellState(false, new Coordinate2D(1, 1, OffsetTypes.OddRowsRight), water),
                new CellState(false, new Coordinate2D(1, 2, OffsetTypes.OddRowsRight), ground)
            }, movementTypes);
        }
    }
}