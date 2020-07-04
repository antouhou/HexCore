using System.Collections.Generic;
using HexCore;

namespace HexCoreTests
{
    public class QuickStart
    {
        public static void Demo()
        {
            var ground = new TerrainType(1, "Ground");
            var water = new TerrainType(2, "Water");

            var walkingType = new MovementType(1, "Walking");
            var swimmingType = new MovementType(2, "Swimming");

            var movementTypes = new MovementTypes(
                new ITerrainType[] {ground, water},
                new Dictionary<IMovementType, Dictionary<ITerrainType, int>>
                {
                    [walkingType] = new Dictionary<ITerrainType, int>
                    {
                        [ground] = 1,
                        [water] = 2
                    },
                    [swimmingType] = new Dictionary<ITerrainType, int>
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
        }
    }
}