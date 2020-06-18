using System;

namespace HexCore.HexGraph
{
    public interface IMovementType : IEquatable<IMovementType>
    {
        string Name { get; }
        int Id { get; }
    }
}