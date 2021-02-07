using System;
using System.Collections.Generic;

namespace HexCore
{
    public class MovementTypeComparer : IEqualityComparer<MovementType>
    {
        public bool Equals(MovementType x, MovementType y)
        {
            return x.GetName() == y.GetName() && x.GetId() == y.GetId();
        }

        public int GetHashCode(MovementType obj)
        {
            return obj.GetId();
        }
    }

    [Serializable]
    public struct MovementType : IEquatable<MovementType>
    {
        public MovementType(int id, string name)
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

        public bool Equals(MovementType other)
        {
            return GetName() == other.GetName() && GetId() == other.GetId();
        }
    }
}