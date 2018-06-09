using System;
using HexCore.HexGraph;
using NUnit.Framework;
using Tests.Fixtures;
using HexCore.DataStructures;
using HexCore.Helpers;

namespace Tests.HexGraph
{
    [TestFixture]
    public class GraphFactoryTest
    {
        private readonly CoordinateConverter
            _coordinateConverterOrr = new CoordinateConverter(OffsetTypes.OddRowsRight);
        
        [Test]
        public void ShouldCreateSquareGraph()
        {
            var width = 4;
            var height = 3;
            var graph = GraphFactory.CreateSquareGraph(width, height, OffsetTypes.OddRowsRight, MovementTypes.Ground);
            Assert.That(graph.GetAllCellsCoordinates().Count, Is.EqualTo(width * height));
            
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                Assert.That(
                    graph.GetAllCellsCoordinates()
                        .Contains(_coordinateConverterOrr.ConvertOneOffsetToCube(new Coordinate2D(x, y))), Is.True);
        }
    }
}