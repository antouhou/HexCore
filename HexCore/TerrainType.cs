using System;
using System.Collections.Generic;

namespace HexCore
{
    public class TerrainTypeComparer : IEqualityComparer<TerrainType>
    {
        public bool Equals(TerrainType x, TerrainType y)
        {
            return x.GetName() == y.GetName() && x.GetId() == y.GetId();
        }

        public int GetHashCode(TerrainType obj)
        {
            return obj.GetId();
        }
    }

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