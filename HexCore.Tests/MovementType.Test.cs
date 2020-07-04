using HexCore;
using NUnit.Framework;

namespace HexCoreTests
{
    [TestFixture]
    public class MovementTypeTest
    {
        [Test]
        public void Equals_ShouldReturnFalse_WhenMovementTypesAreNotEqual()
        {
            var type1 = new MovementType(1, "Type");
            var type2 = new MovementType(2, "Type");
            var type3 = new MovementType(1, "Some other type");

            Assert.That(type1.Equals(type2), Is.False);
            Assert.That(type1.Equals(type3), Is.False);
            Assert.That(type1.Equals(null), Is.False);
        }

        [Test]
        public void Equals_ShouldReturnTrue_WhenMovementTypesAreEqual()
        {
            var type1 = new MovementType(1, "Type");
            var type2 = new MovementType(1, "Type");

            Assert.That(type1.Equals(type2), Is.True);
        }
    }
}