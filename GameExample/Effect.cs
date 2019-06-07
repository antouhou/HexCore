using System;

namespace GameExample
{
    public class Effect
    {
        private Func<Attributes, Attributes, Attributes> calculateCasterBonus;
        private Func<Attributes, Attributes, Attributes> calculateTargetBonus;

        public Effect(Attributes effectEffectAttributes, Func<Attributes, Attributes, Attributes> casterBonus,
            Func<Attributes, Attributes, Attributes> targetBonus)
        {
            EffectAttributes = effectEffectAttributes;
            calculateCasterBonus = casterBonus;
            calculateTargetBonus = targetBonus;
        }

        public Attributes EffectAttributes { get; }

        public Attributes GetCasterBonus(Attributes casterAttributes)
        {
            return calculateCasterBonus(EffectAttributes, casterAttributes);
        }

        public Attributes GetTargetBonus(Attributes targetAttributes)
        {
            return calculateTargetBonus(EffectAttributes, targetAttributes);
        }
    }
}