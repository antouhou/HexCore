﻿using System.Collections.Generic;

namespace HexCore.HexGraph
{
    [System.Serializable]
    public class MovementType
    {
        public string Name = "";
        public Dictionary<string, int> MovementCostTo = new Dictionary<string, int>();

        public int GetCostTo(string typeName)
        {
            return MovementCostTo[typeName];
        }
    }
}