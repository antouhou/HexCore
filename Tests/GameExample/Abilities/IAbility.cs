using HexCore.DataStructures;
using Tests.GameExample.Effects;

namespace Tests.GameExample.Abilities
{
    public interface IAbility
    {
        IEffect Effect { get; }
        uint CastRange { get; }

        uint Duration { get; }

        // Caster and target positions are required for directional abilities, as to calculate direction
        // you need to know where the caster is and where he is facing
        Coordinate3D[] GetApplicableArea(Coordinate3D casterPosition, Coordinate3D targetPosition);

        Attributes GetCasterBonus(Attributes casterAttributes);

        Attributes GetTargetBonus(Attributes targetAttributes);
    }
}