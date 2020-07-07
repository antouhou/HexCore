using HexCore;

namespace HexCore.Website.Data
{
    public class PawnData
    {
        public Coordinate2D CurrentPosition { get; set; }
        public string Name { get; set; }
        
        public MovementType MovementType { get; set; }
        
        public int MovementRange { get; set; }
        
        public string Color { get; set; }
    }
}