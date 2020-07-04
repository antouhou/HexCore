using System.Collections.Generic;
using HexCore;
using HexCoreTests.Fixtures;
using NUnit.Framework;

namespace HexCoreTests.DataStructures
{
    [TestFixture]
    public class Coordinate2DTest
    {
        [Test]
        public void ConvertsOffsetCoordinatesToCubeCoordinatesCorrectly()
        {
            var graph = GraphFactory.CreateRectangularGraph(3, 3, MovementTypesFixture.GetMovementTypes(),
                MovementTypesFixture.Ground);
            var cubeCoordinates = graph.GetAllCellsCoordinates();
            //For odd rows right:
            //Down and right: Y - 1, Z + 1
            //Down and left:  X - 1, Z + 1
            //Right: X + 1, Y - 1;
            var expectedCubeCoordinates = new []
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
                new Coordinate3D(1, -3, 2)
            };
            
            Assert.That(cubeCoordinates, Is.EqualTo(expectedCubeCoordinates));
        }

        [Test]
        public void To3D_ShouldConvertEvenColumnsDown()
        {
            var coordinate2D = new Coordinate2D(1, 2, OffsetTypes.EvenColumnsDown);
            var expectedCoordinate3D = new Coordinate3D(1, -2, 1);
            var actualCoordinate3D = coordinate2D.To3D();
            Assert.That(actualCoordinate3D, Is.EqualTo(expectedCoordinate3D));
        }

        [Test]
        public void To3D_ShouldConvertEvenRowsRight()
        {
            var coordinate2D = new Coordinate2D(2, 1, OffsetTypes.EvenRowsRight);
            var expectedCoordinate3D = new Coordinate3D(1, -2, 1);
            var actualCoordinate3D = coordinate2D.To3D();
            Assert.That(actualCoordinate3D, Is.EqualTo(expectedCoordinate3D));
        }

        [Test]
        public void To3D_ShouldConvertOddOddColumnsDown()
        {
            var coordinate2D = new Coordinate2D(1, 1, OffsetTypes.OddColumnsDown);
            var expectedCoordinate3D = new Coordinate3D(1, -2, 1);
            var actualCoordinate3D = coordinate2D.To3D();
            Assert.That(actualCoordinate3D, Is.EqualTo(expectedCoordinate3D));
        }

        [Test]
        public void To3D_ShouldConvertOddRowsRight()
        {
            var coordinate2D = new Coordinate2D(1, 1, OffsetTypes.OddRowsRight);
            var expectedCoordinate3D = new Coordinate3D(1, -2, 1);
            var actualCoordinate3D = coordinate2D.To3D();
            Assert.That(actualCoordinate3D, Is.EqualTo(expectedCoordinate3D));
        }
    }
}