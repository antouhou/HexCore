using System;
using System.Collections.Generic;
using System.Linq;
using HexCore;
using HexCore.Website.Pages;

namespace HexCore.Website.Data
{
    public static class Movements
    {
        public static TerrainType Ground = new TerrainType(1, "Ground");
        public static TerrainType Water = new TerrainType(2, "Water");
        public static MovementType WalkingType = new MovementType(1, "Walking");
        public static MovementType SwimmingType = new MovementType(2, "Swimming");
        public static MovementType HeavyType = new MovementType(3, "Heavy Type");
    }
    public class GraphService
    {
        private GraphData _graphData;
        
        public event Action GraphDataUpdated;
        
        private static Dictionary<int, Dictionary<int, CellState>> CreateTwoDimensionalMap(IEnumerable<CellState> cells)
        {
            var sortedCoordinates = new Dictionary<int, Dictionary<int, CellState>>();
            foreach (var cell in cells)
            {
                var coordinate2d = cell.Coordinate3.To2D(OffsetTypes.OddRowsRight);
                var row = coordinate2d.Y;
                var col = coordinate2d.X;
                if (!sortedCoordinates.ContainsKey(row))
                {
                    sortedCoordinates.Add(row, new Dictionary<int, CellState>());
                }
                sortedCoordinates[row].Add(col, cell);
            }

            return sortedCoordinates;
        }

        private static List<PawnData> CreatePawns()
        {
            var knightPawn = new PawnData
            {
                Name = "Knight",
                CurrentPosition = new Coordinate2D(1, 0, OffsetTypes.OddRowsRight),
                MovementType = Movements.HeavyType,
                MovementRange = 2,
                Color = "red"
            };
            var orcPawn = new PawnData{
                Name = "Orc",
                CurrentPosition = new Coordinate2D(2, 3, OffsetTypes.OddRowsRight),
                MovementType = Movements.WalkingType,
                MovementRange = 2,
                Color = "green"
            };
            var hydraPawn = new PawnData{
                Name = "Hydra",
                CurrentPosition = new Coordinate2D(2, 2, OffsetTypes.OddRowsRight),
                MovementType = Movements.SwimmingType,
                MovementRange = 2,
                Color = "blue"
            };

            return new List<PawnData> {knightPawn, orcPawn, hydraPawn};
        }

        private static MovementTypes CreateMovementTypes()
        {
            return new MovementTypes(
                new ITerrainType[] { Movements.Ground, Movements.Water }, 
                new Dictionary<IMovementType, Dictionary<ITerrainType, int>>
                {
                    [Movements.WalkingType] = new Dictionary<ITerrainType, int>
                    {
                        [Movements.Ground] = 1,
                        [Movements.Water] = 2
                    },
                    [Movements.SwimmingType] = new Dictionary<ITerrainType, int>
                    {
                        [Movements.Ground] = 2,
                        [Movements.Water] = 1
                    },
                    [Movements.HeavyType] = new Dictionary<ITerrainType, int>
                    {
                        [Movements.Ground] = 1,
                        [Movements.Water] = 99
                    }
                }
            );
        }
        
        private GraphData CreateGraph()
        {
            var movementTypes = CreateMovementTypes();
        
            // Nice round shape
            var graph = new Graph(new []
            {
                new CellState(false, new Coordinate2D(1, 0, OffsetTypes.OddRowsRight), Movements.Ground), 
                new CellState(false, new Coordinate2D(2, 0, OffsetTypes.OddRowsRight), Movements.Ground),
                new CellState(false, new Coordinate2D(3, 0, OffsetTypes.OddRowsRight), Movements.Ground),
                new CellState(false, new Coordinate2D(0, 1, OffsetTypes.OddRowsRight), Movements.Water), 
                new CellState(false, new Coordinate2D(1, 1, OffsetTypes.OddRowsRight), Movements.Ground), 
                new CellState(false, new Coordinate2D(2, 1, OffsetTypes.OddRowsRight), Movements.Ground),
                new CellState(false, new Coordinate2D(3, 1, OffsetTypes.OddRowsRight), Movements.Ground),
                new CellState(false, new Coordinate2D(0, 2, OffsetTypes.OddRowsRight), Movements.Water),
                new CellState(false, new Coordinate2D(1, 2, OffsetTypes.OddRowsRight), Movements.Water), 
                new CellState(false, new Coordinate2D(2, 2, OffsetTypes.OddRowsRight), Movements.Water), 
                new CellState(false, new Coordinate2D(3, 2, OffsetTypes.OddRowsRight), Movements.Ground),
                new CellState(false, new Coordinate2D(4, 2, OffsetTypes.OddRowsRight), Movements.Water),
                new CellState(false, new Coordinate2D(0, 3, OffsetTypes.OddRowsRight), Movements.Water), 
                new CellState(false, new Coordinate2D(1, 3, OffsetTypes.OddRowsRight), Movements.Water), 
                new CellState(false, new Coordinate2D(2, 3, OffsetTypes.OddRowsRight), Movements.Ground),
                new CellState(false, new Coordinate2D(3, 3, OffsetTypes.OddRowsRight), Movements.Ground),
                new CellState(false, new Coordinate2D(1, 4, OffsetTypes.OddRowsRight), Movements.Water), 
                new CellState(false, new Coordinate2D(2, 4, OffsetTypes.OddRowsRight), Movements.Ground),
                new CellState(false, new Coordinate2D(3, 4, OffsetTypes.OddRowsRight), Movements.Ground),
            }, movementTypes);
            
            var pawns = CreatePawns();
            // Blocking our pawns positions, so no pawns can occupy the same position at the same time
            // or use it for movement
            graph.BlockCells(pawns.Select(pawn => pawn.CurrentPosition));

            var graphData = new GraphData
            {
                Graph = graph, 
                MovementTypes = movementTypes, 
                Pawns = pawns,
                SortedCells = CreateTwoDimensionalMap(graph.GetAllCells()),
                TileData = new List<List<HexTileData>>()
            };

            _graphData = graphData;
            foreach (var rowIndex in Enumerable.Range(0, _graphData.MaxRowNumber))
            {
                _graphData.TileData.Add(new List<HexTileData>());
                foreach (var columnIndex in Enumerable.Range(0, _graphData.MaxColumnNumber))
                {
                    _graphData.TileData[rowIndex].Add(new HexTileData
                    {
                        IsEmptySpace = IsEmptySpace(rowIndex, columnIndex),
                        IsSelected = IsCurrentlySelected(rowIndex, columnIndex),
                        ShowMoveButton = NeedsToShowMoveButton(rowIndex, columnIndex),
                        Pawn = GetPawn(rowIndex, columnIndex),
                        CellState = GetCellState(rowIndex, columnIndex),
                        Coordinate2D = RowAndColumnToOffsetOrr(rowIndex, columnIndex)
                    });
                }
            }

            SelectCell(graph.GetAllCells()[0]);
            return graphData;
        }

        public GraphData GetGraph()
        {
            return _graphData ??= CreateGraph();
        }

        private void HideAllMoveButtons()
        {
            foreach (var tile in _graphData.TileData.SelectMany(tileRow => tileRow))
            {
                tile.ShowMoveButton = false;
            }
        }

        private PawnData GetPawn(CellState cell)
        {
            var coordinate = cell.Coordinate3.To2D(OffsetTypes.OddRowsRight);
            return _graphData.Pawns.Find(pawn => pawn.CurrentPosition.Equals(coordinate));
        }

        private PawnData GetPawn(int row, int column)
        {
            if (IsEmptySpace(row, column)) return null;
            var coordinate = RowAndColumnToOffsetOrr(row, column);
            return _graphData.Pawns.Find(pawn => pawn.CurrentPosition.Equals(coordinate));

        }

        private void UnselectCell(CellState cellState)
        {
            if (cellState == null) return;
            var coordinate2D = cellState.Coordinate3.To2D(OffsetTypes.OddRowsRight);
            _graphData.TileData[coordinate2D.Y][coordinate2D.X].IsSelected = false;
        }

        private void SelectPawn(PawnData pawn)
        {
            _graphData.CurrentlySelectedPawnData = pawn;

            if (pawn == null) return;
            var movementRange = _graphData.Graph.GetMovementRange(
                pawn.CurrentPosition, pawn.MovementRange, pawn.MovementType
            );
                
            foreach (var coordinate2 in movementRange)
            {
                _graphData.TileData[coordinate2.Y][coordinate2.X].ShowMoveButton = true;
            }
        }

        private static Coordinate2D RowAndColumnToOffsetOrr(int row, int column)
        {
            return new Coordinate2D(column, row, OffsetTypes.OddRowsRight);
        }

        private bool IsEmptySpace(int row, int column)
        {
            return !_graphData.Graph.Contains(RowAndColumnToOffsetOrr(row, column));
        }

        private bool IsCurrentlySelected(int row, int column)
        {
            return _graphData.CurrentlySelectedCell == GetCellState(row, column);
        }

        private CellState GetCellState(int row, int column)
        {
            return IsEmptySpace(row, column) ? null : _graphData.SortedCells[row][column];
        }
        
        private bool NeedsToShowMoveButton(int row, int column)
        {
            if (!_graphData.CellExists(row, column)) return false;
            var coordinate = _graphData.SortedCells[row][column].Coordinate3.To2D(OffsetTypes.OddRowsRight);
            return _graphData.SelectedPawnMovementRange.Contains(coordinate);
        }

        private HexTileData GetTileData(Coordinate3D coordinate3D)
        {
            var coordinate2D = coordinate3D.To2D(OffsetTypes.OddRowsRight);
            return _graphData.TileData[coordinate2D.Y][coordinate2D.X];
        } 
        
        public void SelectCell(CellState cell)
        {
            HideAllMoveButtons();
            UnselectCell(_graphData.CurrentlySelectedCell);

            // Apply current selection
            var tileData = GetTileData(cell.Coordinate3);
            _graphData.CurrentlySelectedCell = cell;
            tileData.IsSelected = true;
            
            // Add pawn to selection if there's a pawn on a cell
            var pawnOnCell = GetPawn(cell);
            SelectPawn(pawnOnCell);

            GraphDataUpdated?.Invoke();
        }

        public void MoveCurrentlySelectedPawn(Coordinate2D moveTo)
        {
            var pawn = _graphData.CurrentlySelectedPawnData;
            
            // Remove pawn from old position
            _graphData.TileData[pawn.CurrentPosition.Y][pawn.CurrentPosition.X].Pawn = null;
            _graphData.Graph.UnblockCells(pawn.CurrentPosition);
            
            // Place pawn into new position
            pawn.CurrentPosition = moveTo;
            _graphData.Graph.BlockCells(pawn.CurrentPosition);
            _graphData.TileData[pawn.CurrentPosition.Y][pawn.CurrentPosition.X].Pawn = pawn;
            
            GraphDataUpdated?.Invoke();
        }
    }
}