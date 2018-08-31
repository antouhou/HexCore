using System.Collections.Generic;
using HexCore.DataStructures;
using HexCore.HexGraph;

namespace HexCore.BattleCore
{
    public class Pawn
    {
        public string Id { get; }
        public string TeamId { get; }
        public Coordinate3D Position { get; set; }
        public int MovementPoints { get; }
        public MovementType MovementType { get; }
        public int PhysicalAttackRange { get; }
        public List<Ability> Abilities { get; } = new List<Ability>();
        public int HealthPoints = 2;

        public Pawn(string id, Coordinate3D position, MovementType movementType, int movementPoints, int physicalAttackRange, string teamId)
        {
            Id = id;
            Position = position;
            MovementType = movementType;
            MovementPoints = movementPoints;
            PhysicalAttackRange = physicalAttackRange;
            TeamId = teamId;
        }
    }
}