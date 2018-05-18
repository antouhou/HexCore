using System.Collections.Generic;
using HexCore.DataStructures;
using HexCore.Helpers;
using HexCore.HexGraph;
using NUnit.Framework;
using Tests.Fixtures;

namespace Tests.Helpers
{
    [TestFixture]
    public class CoordinateConverterTest
    {
        [Test]
        public void ConvertsOffsetCoordinatesToCubeCoordinatesCorrectly()
        {
            var graph = new Graph(3, 3, OffsetTypes.OddRowsRight, MovementTypes.TypesList);
            var cubeCoordinates =
                CoordinateConverter.ConvertManyOffsetToCube(OffsetTypes.OddRowsRight,
                    graph.GetAllCellsOffsetPosition());
            //For odd rows right:
            //Down and right: Y - 1, Z + 1
            //Down and left:  X - 1, Z + 1
            //Right: X + 1, Y - 1;
            var expectedCubeCoordiates = new List<Coordinate3D>
            {
                new Coordinate3D(0, 0, 0),
                // Down right:
                new Coordinate3D(0, -1, 1),
                // Down left:
                new Coordinate3D(-1, -1, 2),
                // Back to top, right from start position:
                new Coordinate3D(1, -1, 0),
                // Down right:
                new Coordinate3D(1, -2, 1),
                // Down left:
                new Coordinate3D(0, -2, 2),
                // Back to top, right from previous top point:
                new Coordinate3D(2, -2, 0),
                // Down right:
                new Coordinate3D(2, -3, 1),
                // Down and left:
                new Coordinate3D(1, -3, 2),
            };

            Assert.That(cubeCoordinates.Count, Is.EqualTo(expectedCubeCoordiates.Count));
            for (var index = 0; index < expectedCubeCoordiates.Count; index++)
            {
                Assert.That(cubeCoordinates[index], Is.EqualTo(expectedCubeCoordiates[index]));
            }
        }

        [Test]
        public void ShouldBeAbleToRestoreOffsetCoordinate()
        {
            var offsetCoord = new Coordinate2D(2, 3);
            var cubeCoord = CoordinateConverter.ConvertOneOffsetToCube(OffsetTypes.OddRowsRight, offsetCoord);
            var restoredOffset = CoordinateConverter.ConvertOneCubeToOffset(OffsetTypes.OddRowsRight, cubeCoord);
            Assert.That(offsetCoord, Is.EqualTo(restoredOffset));
        }
    }
}