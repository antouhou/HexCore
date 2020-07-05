using System.Collections.Generic;
using System.Linq;
using HexCore;

namespace HexCore.Website.Data
{
    public class GraphService
    {
        public GraphData[] GetGraphs()
        {
            var ground = new TerrainType(1, "Ground");
            var water = new TerrainType(2, "Water");

            var walkingType = new MovementType(1, "Walking");
            var swimmingType = new MovementType(2, "Swimming");

            var movementTypes = new MovementTypes(
                new ITerrainType[] { ground, water }, 
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
            
            return Enumerable.Range(1, 1).Select(index =>
            {
                var pawn1 = new SimplePawn
                {
                    Name = "Knight",
                    CurrentPosition = new Coordinate2D(1, 0, OffsetTypes.OddRowsRight),
                    MovementType = walkingType,
                    MovementRange = 2
                };
                var pawn2 = new SimplePawn{
                    Name = "Orc",
                    CurrentPosition = new Coordinate2D(3, 4, OffsetTypes.OddRowsRight),
                    MovementType = walkingType,
                    MovementRange = 2
                };
                var pawn3 = new SimplePawn{
                    Name = "Hydra",
                    CurrentPosition = new Coordinate2D(0, 3, OffsetTypes.OddRowsRight),
                    MovementType = swimmingType,
                    MovementRange = 2
                };

                var pawns = new List<SimplePawn> {pawn1, pawn2, pawn3};
                
                // Nice round shape
                var graph = new Graph(new []
                {
                    new CellState(false, new Coordinate2D(1, 0, OffsetTypes.OddRowsRight), ground), 
                    new CellState(false, new Coordinate2D(2, 0, OffsetTypes.OddRowsRight), ground),
                    new CellState(false, new Coordinate2D(3, 0, OffsetTypes.OddRowsRight), ground),
                    new CellState(false, new Coordinate2D(0, 1, OffsetTypes.OddRowsRight), water), 
                    new CellState(false, new Coordinate2D(1, 1, OffsetTypes.OddRowsRight), ground), 
                    new CellState(false, new Coordinate2D(2, 1, OffsetTypes.OddRowsRight), ground),
                    new CellState(false, new Coordinate2D(3, 1, OffsetTypes.OddRowsRight), ground),
                    new CellState(false, new Coordinate2D(0, 2, OffsetTypes.OddRowsRight), water),
                    new CellState(false, new Coordinate2D(1, 2, OffsetTypes.OddRowsRight), water), 
                    new CellState(false, new Coordinate2D(2, 2, OffsetTypes.OddRowsRight), water), 
                    new CellState(false, new Coordinate2D(3, 2, OffsetTypes.OddRowsRight), ground),
                    new CellState(false, new Coordinate2D(4, 2, OffsetTypes.OddRowsRight), water),
                    new CellState(false, new Coordinate2D(0, 3, OffsetTypes.OddRowsRight), water), 
                    new CellState(false, new Coordinate2D(1, 3, OffsetTypes.OddRowsRight), water), 
                    new CellState(false, new Coordinate2D(2, 3, OffsetTypes.OddRowsRight), ground),
                    new CellState(false, new Coordinate2D(3, 3, OffsetTypes.OddRowsRight), ground),
                    new CellState(false, new Coordinate2D(1, 4, OffsetTypes.OddRowsRight), water), 
                    new CellState(false, new Coordinate2D(2, 4, OffsetTypes.OddRowsRight), ground),
                    new CellState(false, new Coordinate2D(3, 4, OffsetTypes.OddRowsRight), ground),
                }, movementTypes);
                
                // Blocking our pawns positions
                graph.BlockCells(pawns.Select(pawn => pawn.CurrentPosition));

                var graphData = new GraphData
                {
                    Graph = graph, 
                    MovementTypes = movementTypes, 
                    Pawns = pawns
                };


                return graphData;
            }).ToArray();
        }
    }
}