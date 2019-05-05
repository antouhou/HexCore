using System;
using HexCore.HexGraph;
using NUnit.Framework.Constraints;

namespace Tests.GameExample
{
    [Serializable]
    public class Attributes
    {
        public double Attack;
        public int AttackRange;
        public bool CanAttack;
        public bool CanCast;
        public bool CanMove;
        public double Defense;
        public double HP;
        public double MagicPower;
        public int MovementRange;
        public MovementType MovementType;
        public double MP;

        public Attributes(MovementType movementType = null, double attack = 0, double defense = 0, double hp = 0,
            double mp = 0, double magicPower = 0, int movementRange = 0, int attackRange = 0)
        {
            Attack = attack;
            Defense = defense;
            HP = hp;
            MP = mp;
            MagicPower = magicPower;
            MovementRange = movementRange;
            MovementType = movementType;
            AttackRange = attackRange;
        }
    }
}