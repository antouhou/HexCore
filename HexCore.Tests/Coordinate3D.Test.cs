using System;
using HexCore;
using NUnit.Framework;

namespace HexCoreTests
{
    [TestFixture]
    public class Coordinate3DTest
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
        public void Coordinate3D_ShouldBeNotPossibleToCreateACoordinateWithNotZeroSum()
        {
            var exception = Assert.Throws<InvalidOperationException>(() => { new Coordinate3D(1, 1, 1); });
            Assert.That(exception.Message,
                Is.EqualTo("Sum of all points in 3D coordinate should always be equal to zero"));
        }

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
            var a = new Coordinate3D(1, -2, 1);
            var b = new Coordinate3D(2, 2, -4);
            var c = a + b;
            Assert.That(c, Is.EqualTo(new Coordinate3D(3, 0, -3)));
        }

        [Test]
        public void ShouldMultiplyCoordinateByScalar()
        {
            var a = new Coordinate3D(-2, 3, -1);
            var b = a * 2;
            Assert.That(b, Is.EqualTo(new Coordinate3D(-4, 6, -2)));
        }

        [Test]
        public void ShouldSubtractOneCoordinateFromAnother()
        {
            var a = new Coordinate3D(1, -3, 2);
            var b = new Coordinate3D(-2, 6, -4);
            var c = a - b;
            Assert.That(c, Is.EqualTo(new Coordinate3D(3, -9, 6)));
        }

        [Test]
        public void To2D_ShouldConvertTo2DEvenColumnsDown()
        {
            var coordinate3D = new Coordinate3D(1, -2, 1);
            var actualCoordinate2D = coordinate3D.To2D(OffsetTypes.EvenColumnsDown);
            var expectedCoordinate2D = new Coordinate2D(1, 2, OffsetTypes.EvenColumnsDown);
            Assert.That(actualCoordinate2D, Is.EqualTo(expectedCoordinate2D));
        }

        [Test]
        public void To2D_ShouldConvertTo2DEvenRowsRight()
        {
            var coordinate3D = new Coordinate3D(1, -2, 1);
            var actualCoordinate2D = coordinate3D.To2D(OffsetTypes.EvenRowsRight);
            var expectedCoordinate2D = new Coordinate2D(2, 1, OffsetTypes.EvenRowsRight);
            Assert.That(actualCoordinate2D, Is.EqualTo(expectedCoordinate2D));
        }

        [Test]
        public void To2D_ShouldConvertTo2DOddColumnsDown()
        {
            var coordinate3D = new Coordinate3D(1, -2, 1);
            var actualCoordinate2D = coordinate3D.To2D(OffsetTypes.OddColumnsDown);
            var expectedCoordinate2D = new Coordinate2D(1, 1, OffsetTypes.OddColumnsDown);
            Assert.That(actualCoordinate2D, Is.EqualTo(expectedCoordinate2D));
        }

        [Test]
        public void To2D_ShouldConvertTo2DOddRowsRight()
        {
            var coordinate3D = new Coordinate3D(1, -2, 1);
            var actualCoordinate2D = coordinate3D.To2D(OffsetTypes.OddRowsRight);
            var expectedCoordinate2D = new Coordinate2D(1, 1, OffsetTypes.OddRowsRight);
            Assert.That(actualCoordinate2D, Is.EqualTo(expectedCoordinate2D));
        }

        [Test]
        public void ToString_ShouldSerializeToString()
        {
            var coordinate3D = new Coordinate3D(1, -2, 1);

            Assert.That(coordinate3D.ToString(), Is.EqualTo("(1, -2, 1)"));
        }
    }
}