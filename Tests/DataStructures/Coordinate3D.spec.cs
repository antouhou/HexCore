using HexCore.DataStructures;
using NUnit.Framework;

namespace Tests.DataStructures
{
    [TestFixture]
    public class Coordinate3D_spec
    {
        [Test]
        public void ShouldBeAbleToRestoreOffsetCoordinate()
        {
            var offsetCoord = new Coordinate2D(2, 3, OffsetTypes.OddRowsRight);
            var cubeCoord = offsetCoord.To3D();
            var restoredOffset = cubeCoord.To2D(OffsetTypes.OddRowsRight);
            Assert.That(offsetCoord, Is.EqualTo(restoredOffset));
        }
    }
}