using System;

namespace HexCore.HexGraph
{
    [Serializable]
    public struct TerrainType : ITerrainType
    {
        public TerrainType(int id, string name)
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