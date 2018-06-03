﻿using System;
using HexCore.DataStructures;
using HexCore.HexGraph;

namespace HexCore.BattleCore.Unit
{
    [Serializable]
    public class UnitState
    {
        private Attack _attack = new Attack {Range = 1};
        private int _movementPoints;
        private MovementType _movementType;
        private Coordinate3D _position;

        public UnitState(MovementType movementType, int movementPoints)
        {
            _movementType = movementType;
            _movementPoints = movementPoints;
        }

        public int MovementPoints
        {
            get { return _movementPoints; }
            set { _movementPoints = value; }
        }

        public MovementType MovementType
        {
            get { return _movementType; }
            set { _movementType = value; }
        }

        public Coordinate3D Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Attack Attack
        {
            get { return _attack; }
            set { _attack = value; }
        }
    }
}