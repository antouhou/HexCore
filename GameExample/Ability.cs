using HexCore.DataStructures;

namespace GameExample
{
    public class Ability
    {
        public Ability(string name, Effect effect, Coordinate3D[] shape, uint castRange, uint effectDuration)
        {
            Effect = effect;
            Shape = shape;
            CastRange = castRange;
            EffectDuration = effectDuration;
        }

        public Effect Effect { get; }
        public uint CastRange { get; }
        public uint EffectDuration { get; }

        public Coordinate3D[] Shape { get; }

        // Caster and target positions are required for directional abilities, as to calculate direction
        // you need to know where the caster is and where he is facing
        public Coordinate3D[] GetApplicableArea(Coordinate3D casterPosition, Coordinate3D targetPosition)
        {
            return Shape;
        }
    }
}