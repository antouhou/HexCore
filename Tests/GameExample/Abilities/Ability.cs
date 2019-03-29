using HexCore.DataStructures;
using Tests.GameExample.Effects;

namespace Tests.GameExample.Abilities
{
    public class Ability
    {
        public Ability(string name, IEffect effect, uint castRange, uint effectDuration, Coordinate3D[] coveringArea)
        {
        }

        public Coordinate3D[] GetApplicableArea()
        {
            return new Coordinate3D[] { };
        }
    }
}