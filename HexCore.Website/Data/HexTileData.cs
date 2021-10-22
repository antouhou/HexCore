namespace HexCore.Website.Data
{
    public class HexTileData
    {
        public bool IsEmptySpace { get; set; }
        public PawnData Pawn;
        public bool ShowMoveButton { get; set; }
        public bool IsSelected { get; set; }
        public CellState CellState { get; set; }
        public Coordinate2D Coordinate2D { get; set; }
    }
}