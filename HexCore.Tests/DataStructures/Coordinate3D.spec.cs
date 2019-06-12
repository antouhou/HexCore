using HexCore.DataStructures;
using NUnit.Framework;

namespace HexCoreTests.DataStructures
{
    [TestFixture]
    public class Coordinate3D_spec
    {
        private static Coordinate3D[][] RotateRightCases =
        {
            new[]
            {
                new Coordinate3D(0, 0, 0),
                new Coordinate3D(0, 2, -2),
                new Coordinate3D(2, 0, -2)
            },
            new[]
            {
                new Coordinate3D(0, 0, 0),
                new Coordinate3D(2, 0, -2),
                new Coordinate3D(2, -2, -0)
            },
            new[]
            {
                new Coordinate3D(0, 0, 0),
                new Coordinate3D(1, 1, -2),
                new Coordinate3D(2, -1, -1)
            },
            new[]
            {
                new Coordinate3D(1, 1, -2),
                new Coordinate3D(1, 3, -4),
                new Coordinate3D(3, 1, -4)
            },
            new[]
            {
                new Coordinate3D(0, 2, -2),
                new Coordinate3D(0, 4, -4),
                new Coordinate3D(2, 2, -4)
            }
        };

        [Test]
        [TestCaseSource(nameof(RotateRightCases))]
        public void RotateRight_ShouldRotateCoordinateToTheRight(Coordinate3D center, Coordinate3D position,
            Coordinate3D expectedRotation)
        {
            var rotation = Coordinate3D.RotateRight(center, position);
            Assert.That(rotation, Is.EqualTo(expectedRotation));
        }

        [Test]
        public void ShouldAddOneCoordinateToAnother()
        {
            var a = new Coordinate3D(1, 2, 3);
            var b = new Coordinate3D(1, 1, 1);
            var c = a + b;
            Assert.That(c, Is.EqualTo(new Coordinate3D(2, 3, 4)));
        }

        [Test]
        public void ShouldMultiplyCoordinateByScalar()
        {
            var a = new Coordinate3D(2, 3, 4);
            var b = a * 2;
            Assert.That(b, Is.EqualTo(new Coordinate3D(4, 6, 8)));
        }

        [Test]
        public void ShouldMultiplyTwoCoordinates()
        {
            var a = new Coordinate3D(1, 1, 1);
            var b = new Coordinate3D(1, 1, 1);
            var c = new Coordinate3D(2, 3, 4);
            var d = a * b;
            var e = c * c;
            Assert.That(d, Is.EqualTo(new Coordinate3D(1, 1, 1)));
            Assert.That(e, Is.EqualTo(new Coordinate3D(4, 9, 16)));
        }

        [Test]
        public void ShouldSubtractOneCoordinateFromAnother()
        {
            var a = new Coordinate3D(1, 2, 3);
            var b = new Coordinate3D(1, 1, 1);
            var c = a - b;
            Assert.That(c, Is.EqualTo(new Coordinate3D(0, 1, 2)));
        }

        [Test]
        public void To3D_ShouldConvertFrom3Dto2DAndBack()
        {
            var offsetCoord = new Coordinate2D(2, 2, OffsetTypes.OddRowsRight);
            var cubeCoord = offsetCoord.To3D();
            var restoredOffset = cubeCoord.To2D(OffsetTypes.OddRowsRight);
            Assert.That(cubeCoord, Is.EqualTo(new Coordinate3D(1, -3, 2)));
            Assert.That(offsetCoord, Is.EqualTo(restoredOffset));
        }
    }
}