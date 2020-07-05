using System.Collections.Generic;
using HexCore;

namespace HexCore.Website.Data
{
    public class GraphData
    {
        public Graph Graph { get; set; }
        public MovementTypes MovementTypes { get; set; }
        
        public List<SimplePawn> Pawns { get; set; }
    }
}