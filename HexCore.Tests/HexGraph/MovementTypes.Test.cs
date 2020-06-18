using System.Collections.Generic;
using HexCore.HexGraph;
using NUnit.Framework;

namespace HexCoreTests.HexGraph
{
    [TestFixture]
    public class MovementTypesTest
    {
        [Test]
        public void Constructor_ShouldAddNewType()
        {
            var ground = new MovementType(1, "Ground");
            var movementTypes = new MovementTypes(
                new Dictionary<IMovementType, Dictionary<IMovementType, int>>
                {
                    [ground] = new Dictionary<IMovementType, int>
                    {
                        [ground] = 1
                    }
                }
            );

            Assert.That(movementTypes.Contains(ground), Is.True);
        }

        [Test]
        public void Constructor_ShouldThrow_WhenAddingATypeWithCostsOutsideOfKnownTypes()
        {
            var ground = new MovementType(1, "Ground");
            var water = new MovementType(2, "Water");
            var heavy = new MovementType(3, "Heavy");
            Assert.That(() =>
                {
                    var movementTypes = new MovementTypes(
                        new Dictionary<IMovementType, Dictionary<IMovementType, int>>
                        {
                            [ground] = new Dictionary<IMovementType, int>
                            {
                                [ground] = 1,
                                [water] = 2,
                                [heavy] = 1
                            },
                            [water] = new Dictionary<IMovementType, int>
                            {
                                [ground] = 2,
                                [water] = 1,
                                [heavy] = 1
                            }
                        }
                    );
                },
                Throws.ArgumentException.With.Message.EqualTo(
                    "Error when adding movement type 'Ground': movement costs contain unknown type: 'Heavy'"));
        }

        [Test]
        public void Constructor_ShouldThrow_WhenAddingATypeWithIncompleteCosts()
        {
            var ground = new MovementType(1, "Ground");
            var water = new MovementType(2, "Water");
            var heavy = new MovementType(3, "Heavy");
            Assert.That(() =>
                {
                    var movementTypes = new MovementTypes(
                        new Dictionary<IMovementType, Dictionary<IMovementType, int>>
                        {
                            [ground] = new Dictionary<IMovementType, int>
                            {
                                [ground] = 1
                            },
                            [water] = new Dictionary<IMovementType, int>
                            {
                                [ground] = 2,
                                [water] = 1
                            },
                            [heavy] = new Dictionary<IMovementType, int>
                            {
                                [ground] = 2,
                                [water] = 1
                            }
                        }
                    );
                },
                Throws.ArgumentException.With.Message.EqualTo(
                    "Error when adding movement type 'Ground': missing movement costs to types: 'Water', 'Heavy'"));
        }

        [Test]
        public void Constructor_ShouldThrow_WhenAddingTwoTypesWithTheSameId()
        {
            var ground = new MovementType(1, "Ground");
            var water = new MovementType(1, "Water");
            Assert.That(() =>
                {
                    var movementTypes = new MovementTypes(
                        new Dictionary<IMovementType, Dictionary<IMovementType, int>>
                        {
                            [ground] = new Dictionary<IMovementType, int>
                            {
                                [ground] = 1,
                                [water] = 2
                            },
                            [water] = new Dictionary<IMovementType, int>
                            {
                                [ground] = 2,
                                [water] = 1
                            }
                        }
                    );
                },
                Throws.ArgumentException.With.Message.EqualTo(
                    "An item with the same key has already been added. Key: 1"));
        }

        [Test]
        public void Constructor_ShouldThrowIfAnEmptyDictionaryIsPassed()
        {
            var ground = new MovementType(1, "Ground");
            Assert.That(() =>
                {
                    var movementTypes =
                        new MovementTypes(new Dictionary<IMovementType, Dictionary<IMovementType, int>>());
                },
                Throws.ArgumentException.With.Message.EqualTo(
                    "Movement types should always have at least one explicitly defined type. For the reasoning, please visit the movement types section in the library's docs"));
        }

        [Test]
        public void GetAllTypes_ShouldReturnAllAddedTypes()
        {
            var ground = new MovementType(1, "Ground");
            var water = new MovementType(123, "Water");
            var movementTypes = new MovementTypes(
                new Dictionary<IMovementType, Dictionary<IMovementType, int>>
                {
                    [ground] = new Dictionary<IMovementType, int>
                    {
                        [ground] = 1,
                        [water] = 2
                    },
                    [water] = new Dictionary<IMovementType, int>
                    {
                        [ground] = 2,
                        [water] = 1
                    }
                }
            );

            Assert.That(movementTypes.GetAllTypes(), Is.EquivalentTo(new[] {ground, water}));
        }

        [Test]
        public void GetId_ShouldReturnIdOfTheType()
        {
            var ground = new MovementType(1, "Ground");
            var water = new MovementType(123, "Water");
            var movementTypes = new MovementTypes(
                new Dictionary<IMovementType, Dictionary<IMovementType, int>>
                {
                    [ground] = new Dictionary<IMovementType, int>
                    {
                        [ground] = 1,
                        [water] = 2
                    },
                    [water] = new Dictionary<IMovementType, int>
                    {
                        [ground] = 2,
                        [water] = 1
                    }
                }
            );

            Assert.That(movementTypes.GetId(ground), Is.EqualTo(1));
            Assert.That(movementTypes.GetId(water), Is.EqualTo(123));
        }

        [Test]
        public void GetMovementCost_ShouldReturnCostFromAToB()
        {
            var ground = new MovementType(1, "Ground");
            var water = new MovementType(2, "Water");
            var air = new MovementType(3, "Air");
            var movementTypes = new MovementTypes(
                new Dictionary<IMovementType, Dictionary<IMovementType, int>>
                {
                    [ground] = new Dictionary<IMovementType, int>
                    {
                        [ground] = 1,
                        [water] = 2,
                        [air] = 999
                    },
                    [water] = new Dictionary<IMovementType, int>
                    {
                        [ground] = 2,
                        [water] = 1,
                        [air] = 999
                    },
                    [air] = new Dictionary<IMovementType, int>
                    {
                        [ground] = 1,
                        [water] = 1,
                        [air] = 1
                    }
                }
            );

            Assert.That(movementTypes.GetMovementCost(ground, ground), Is.EqualTo(1));
            Assert.That(movementTypes.GetMovementCost(water, water), Is.EqualTo(1));
            Assert.That(movementTypes.GetMovementCost(ground, water), Is.EqualTo(2));
            Assert.That(movementTypes.GetMovementCost(water, ground), Is.EqualTo(2));
            Assert.That(movementTypes.GetMovementCost(ground, air), Is.EqualTo(999));
            Assert.That(movementTypes.GetMovementCost(air, ground), Is.EqualTo(1));
        }

        [Test]
        public void GetType_ShouldReturnTypeByTheId()
        {
            var ground = new MovementType(1, "Ground");
            var water = new MovementType(123, "Water");
            var movementTypes = new MovementTypes(
                new Dictionary<IMovementType, Dictionary<IMovementType, int>>
                {
                    [ground] = new Dictionary<IMovementType, int>
                    {
                        [ground] = 1,
                        [water] = 2
                    },
                    [water] = new Dictionary<IMovementType, int>
                    {
                        [ground] = 2,
                        [water] = 1
                    }
                }
            );

            Assert.That(movementTypes.GetType(1), Is.EqualTo(ground));
            Assert.That(movementTypes.GetType(123), Is.EqualTo(water));
        }
    }
}