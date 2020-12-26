using System.Collections.Generic;
using HexCore;
using NUnit.Framework;

namespace HexCoreTests
{
    [TestFixture]
    public class QuickstartTest
    {
        [Test]
        public void QuickstartExample_ShouldWork()
        {
            var ground = new TerrainType(1, "Ground");
            var water = new TerrainType(2, "Water");

            var walkingType = new MovementType(1, "Walking");
            var swimmingType = new MovementType(2, "Swimming");

            var movementTypes = new MovementTypes(
                new TerrainType[] {ground, water},
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

            var graph = new Graph(new[]
            {
                new CellState(false, new Coordinate2D(0, 0, OffsetTypes.OddRowsRight), ground),
                new CellState(false, new Coordinate2D(0, 1, OffsetTypes.OddRowsRight), ground),
                new CellState(true, new Coordinate2D(1, 0, OffsetTypes.OddRowsRight), water),
                new CellState(false, new Coordinate2D(1, 1, OffsetTypes.OddRowsRight), water),
                new CellState(false, new Coordinate2D(1, 2, OffsetTypes.OddRowsRight), ground)
            }, movementTypes);

            var pawnPosition = new Coordinate2D(0, 0, OffsetTypes.OddRowsRight).To3D();
            // Mark pawn's position as occupied
            graph.BlockCells(pawnPosition);

            const int pawnMovementPoints = 3;

            var pawnMovementRange = graph.GetMovementRange(
                pawnPosition, pawnMovementPoints, walkingType
            );

            var pawnGoal = new Coordinate2D(1, 2, OffsetTypes.OddRowsRight).To3D();

            var theShortestPath = graph.GetShortestPath(
                pawnPosition,
                pawnGoal,
                walkingType
            );
            // When moving pawn, unblock old position and block the new one.
            graph.UnblockCells(pawnPosition);
            pawnPosition = pawnGoal;
            graph.BlockCells(pawnGoal);

            Assert.That(graph.IsCellBlocked(pawnPosition), Is.True);
            Assert.That(theShortestPath, Is.EquivalentTo(new[]
            {
                new Coordinate2D(0, 1, OffsetTypes.OddRowsRight).To3D(),
                pawnGoal
            }));

            var expectedMovementRange = new[]
            {
                new Coordinate2D(0, 1, OffsetTypes.OddRowsRight).To3D(),
                new Coordinate2D(1, 1, OffsetTypes.OddRowsRight).To3D(),
                new Coordinate2D(1, 2, OffsetTypes.OddRowsRight).To3D()
            };
            Assert.That(pawnMovementRange, Is.EquivalentTo(expectedMovementRange));
            
            QuickStart.Demo();
        }
    }
}