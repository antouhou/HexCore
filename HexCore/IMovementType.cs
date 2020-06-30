using System;

namespace HexCore
{
    public interface IMovementType : IEquatable<IMovementType>
    {
        string Name { get; }
        int Id { get; }
    }
}