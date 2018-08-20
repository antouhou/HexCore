using HexCore.DataStructures;

namespace HexCore.BattleCore
{
    public class Pawn
    {
        public string Id { get; }
        public Coordinate3D Position { get; set; }

        public Pawn(string id, Coordinate3D position)
        {
            Id = id;
            Position = position;
        }
    }
}