using System.Collections.Generic;
using System.Linq;
using HexCore;

namespace HexCore.Website.Data
{
    public class GraphData
    {
        public Graph Graph { get; set; }
        public MovementTypes MovementTypes { get; set; }
        
        public List<PawnData> Pawns { get; set; }
        
        public List<Coordinate2D> SelectedPawnMovementRange { get; set; } = new List<Coordinate2D>();

        public Dictionary<int, Dictionary<int, CellState>> SortedCells { get; set; }
        
        public bool CellExists(int row, int column)
        {
            return SortedCells.ContainsKey(row) && SortedCells[row].ContainsKey(column);
        }

        public List<List<HexTileData>> TileData;

        public CellState CurrentlySelectedCell { get; set; }
        public PawnData CurrentlySelectedPawnData { get; set; }
        
        public int MaxColumnNumber => SortedCells
            .Select(row => row.Value.Keys.Max() + 1)
            .Concat(new[] {0})
            .Max();

        public int MaxRowNumber => SortedCells.Keys.Max() + 1;
    }
}