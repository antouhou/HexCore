using HexCore.DataStructures;
using NUnit.Framework;

namespace Tests.DataStructures
{
    [TestFixture]
    public class Coordinate3D_spec
    {
        [Test]
        public void ShouldAddOneCoordinateToAnother()
        {
            var a = new Coordinate3D(1, 2, 3);
            var b = new Coordinate3D(1, 1, 1);
            var c = a + b;
            Assert.That(c, Is.EqualTo(new Coordinate3D(2, 3, 4)));
        }

        [Test]
        public void ShouldConvertFrom3Dto2DAndBack()
        {
            var offsetCoord = new Coordinate2D(2, 2, OffsetTypes.OddRowsRight);
            var cubeCoord = offsetCoord.To3D();
            var restoredOffset = cubeCoord.To2D(OffsetTypes.OddRowsRight);
            Assert.That(cubeCoord, Is.EqualTo(new Coordinate3D(1, -3, 2)));
            Assert.That(offsetCoord, Is.EqualTo(restoredOffset));
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
    }
}