using System;

namespace HexCore
{
    [Serializable]
    public struct TerrainType : IEquatable<TerrainType>
    {
        public TerrainType(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id;
        public string Name;
        
        public int GetId()
        {
            return Id;
        }

        public string GetName()
        {
            return Name;
        }

        public bool Equals(TerrainType other)
        {
            return GetName() == other.GetName() && GetId() == other.GetId();
        }
    }
}
