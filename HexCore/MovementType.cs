using System;

namespace HexCore
{
    [Serializable]
    public struct MovementType : IMovementType
    {
        public MovementType(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }
        public string Name { get; }

        public bool Equals(IMovementType other)
        {
            if (other is null)
                return false;

            return Name == other.Name && Id == other.Id;
        }
    }
}