using System.Collections.Generic;
using HexCore;
using HexCoreTests.Fixtures;
using NUnit.Framework;

namespace HexCoreTests
{
    [TestFixture]
    public class MovementTypesTest
    {
        [Test]
        public void Constructor_ShouldAddNewType()
        {
            var ground = new TerrainType(1, "Ground");
            var walking = new MovementType(1, "Walking");
            var movementTypes = new MovementTypes(new[] {ground},
                new Dictionary<MovementType, Dictionary<TerrainType, int>>
                {
                    [walking] = new Dictionary<TerrainType, int>
                    {
                        [ground] = 1
                    }
                });

            Assert.That(movementTypes.ContainsTerrainType(ground), Is.True);
            Assert.That(movementTypes.ContainsMovementType(walking), Is.True);
        }

        [Test]
        public void Constructor_ShouldThrow_WhenAddingATypeWithCostsOutsideOfKnownTypes()
        {
            var ground = new TerrainType(1, "Ground");
            var water = new TerrainType(2, "Water");
            var air = new TerrainType(3, "Air");

            var walkingType = new MovementType(1, "Walking");
            var swimmingType = new MovementType(2, "Swimming");

            Assert.That(() =>
                {
                    var movementTypes = new MovementTypes(new[] {ground, water},
                        new Dictionary<MovementType, Dictionary<TerrainType, int>>
                        {
                            [walkingType] = new Dictionary<TerrainType, int>
                            {
                                [ground] = 1,
                                [water] = 2,
                                [air] = 1
                            },
                            [swimmingType] = new Dictionary<TerrainType, int>
                            {
                                [ground] = 2,
                                [water] = 1,
                                [air] = 1
                            }
                        });
                },
                Throws.ArgumentException.With.Message.EqualTo(
                    "Error when adding movement type 'Walking': movement costs contain unknown type: 'Air'"));
        }

        [Test]
        public void Constructor_ShouldThrow_WhenAddingATypeWithIncompleteCosts()
        {
            var ground = new TerrainType(1, "Ground");
            var water = new TerrainType(2, "Water");
            var air = new TerrainType(3, "Air");

            var walkingType = new MovementType(1, "Walking");
            var swimmingType = new MovementType(2, "Swimming");
            var flyingType = new MovementType(3, "Flying");

            Assert.That(() =>
                {
                    var movementTypes = new MovementTypes(new[] {ground, water, air},
                        new Dictionary<MovementType, Dictionary<TerrainType, int>>
                        {
                            [walkingType] = new Dictionary<TerrainType, int>
                            {
                                [ground] = 1
                            },
                            [swimmingType] = new Dictionary<TerrainType, int>
                            {
                                [ground] = 2,
                                [water] = 1
                            },
                            [flyingType] = new Dictionary<TerrainType, int>
                            {
                                [ground] = 2,
                                [water] = 1
                            }
                        });
                },
                Throws.ArgumentException.With.Message.EqualTo(
                    "Error when adding movement type 'Walking': missing movement costs to types: 'Water', 'Air'"));
        }

        [Test]
        public void Constructor_ShouldThrow_WhenAddingTwoTypesWithTheSameId()
        {
            var ground = new TerrainType(1, "Ground");
            var water = new TerrainType(2, "Water");

            var walkingType = new MovementType(1, "Walking");
            var swimmingType = new MovementType(1, "Swimming");
            Assert.That(() =>
                {
                    var movementTypes = new MovementTypes(new[] {ground, water},
                        new Dictionary<MovementType, Dictionary<TerrainType, int>>
                        {
                            [walkingType] = new Dictionary<TerrainType, int>
                            {
                                [ground] = 1,
                                [water] = 2
                            },
                            [swimmingType] = new Dictionary<TerrainType, int>
                            {
                                [ground] = 2,
                                [water] = 1
                            }
                        });
                },
                Throws.ArgumentException.With.Message.EqualTo(
                    "Some of the movement types have same Ids. Ids: 1"));
        }

        [Test]
        public void Constructor_ShouldThrowIfAnEmptyDictionaryIsPassed()
        {
            var ground = new TerrainType(1, "Ground");
            var walking = new MovementType(1, "Walking");
            Assert.That(() =>
                {
                    var movementTypes =
                        new MovementTypes(new[] {ground},
                            new Dictionary<MovementType, Dictionary<TerrainType, int>>());
                },
                Throws.ArgumentException.With.Message.EqualTo(
                    "Movement types should always have at least one explicitly defined type. For the reasoning, please visit the movement types section in the library's docs"));

            Assert.That(() =>
                {
                    var movementTypes =
                        new MovementTypes(new TerrainType[] { },
                            new Dictionary<MovementType, Dictionary<TerrainType, int>>
                            {
                                [walking] = new Dictionary<TerrainType, int>
                                {
                                    [ground] = 1
                                }
                            });
                },
                Throws.ArgumentException.With.Message.EqualTo(
                    "Error when adding movement type 'Walking': movement costs contain unknown type: 'Ground'"));
        }

        [Test]
        public void GetAllTypes_ShouldReturnAllAddedTypes()
        {
            var ground = new TerrainType(1, "Ground");
            var water = new TerrainType(2, "Water");

            var walkingType = new MovementType(1, "Walking");
            var swimmingType = new MovementType(2, "Swimming");

            var movementTypes = new MovementTypes(new[] {ground, water},
                new Dictionary<MovementType, Dictionary<TerrainType, int>>
                {
                    [walkingType] = new Dictionary<TerrainType, int>
                    {
                        [ground] = 1,
                        [water] = 2
                    },
                    [swimmingType] = new Dictionary<TerrainType, int>
                    {
                        [ground] = 2,
                        [water] = 1
                    }
                });

            Assert.That(movementTypes.GetAllMovementTypes(), Is.EquivalentTo(new[] {walkingType, swimmingType}));
            Assert.That(movementTypes.GetAllTerrainTypes(), Is.EquivalentTo(new[] {ground, water}));
        }

        [Test]
        public void GetId_ShouldReturnIdOfTheType()
        {
            var ground = new TerrainType(1, "Ground");
            var water = new TerrainType(2, "Water");

            var walkingType = new MovementType(1, "Walking");
            var swimmingType = new MovementType(123, "Swimming");

            var movementTypes = new MovementTypes(new[] {ground, water},
                new Dictionary<MovementType, Dictionary<TerrainType, int>>
                {
                    [walkingType] = new Dictionary<TerrainType, int>
                    {
                        [ground] = 1,
                        [water] = 2
                    },
                    [swimmingType] = new Dictionary<TerrainType, int>
                    {
                        [ground] = 2,
                        [water] = 1
                    }
                });

            Assert.That(movementTypes.GetMovementTypeId(walkingType), Is.EqualTo(1));
            Assert.That(movementTypes.GetMovementTypeId(swimmingType), Is.EqualTo(123));
        }

        [Test]
        public void GetMovementCost_ShouldReturnCostFromAToB()
        {
            var ground = new TerrainType(1, "Ground");
            var water = new TerrainType(2, "Water");
            var air = new TerrainType(3, "Air");

            var walkingType = new MovementType(1, "Walking");
            var swimmingType = new MovementType(2, "Swimming");
            var flyingType = new MovementType(3, "Flying");

            var movementTypes = new MovementTypes(new[]
            {
                ground, water, air
            }, new Dictionary<MovementType, Dictionary<TerrainType, int>>
            {
                [walkingType] = new Dictionary<TerrainType, int>
                {
                    [ground] = 1,
                    [water] = 2,
                    [air] = 999
                },
                [swimmingType] = new Dictionary<TerrainType, int>
                {
                    [ground] = 2,
                    [water] = 1,
                    [air] = 999
                },
                [flyingType] = new Dictionary<TerrainType, int>
                {
                    [ground] = 1,
                    [water] = 1,
                    [air] = 1
                }
            });

            Assert.That(movementTypes.GetMovementCost(walkingType, ground), Is.EqualTo(1));
            Assert.That(movementTypes.GetMovementCost(swimmingType, water), Is.EqualTo(1));
            Assert.That(movementTypes.GetMovementCost(walkingType, water), Is.EqualTo(2));
            Assert.That(movementTypes.GetMovementCost(swimmingType, ground), Is.EqualTo(2));
            Assert.That(movementTypes.GetMovementCost(walkingType, air), Is.EqualTo(999));
            Assert.That(movementTypes.GetMovementCost(flyingType, ground), Is.EqualTo(1));
        }

        [Test]
        public void GetMovementCost_ShouldThrow_WhenUnknownMovementTypeIsPassed()
        {
            var unknownMovementType = new MovementType(1, "Some movement type");

            var movementTypes = MovementTypesFixture.GetMovementTypes();

            Assert.That(() => { movementTypes.GetMovementCost(unknownMovementType, MovementTypesFixture.Ground); },
                Throws.ArgumentException.With.Message.EqualTo(
                    "Unknown movement type: 'Some movement type'"));
        }

        [Test]
        public void GetMovementCost_ShouldThrow_WhenUnknownTerrainTypeIsPassed()
        {
            var unknownTerrainType = new TerrainType(1, "Some terrain type");

            var movementTypes = MovementTypesFixture.GetMovementTypes();

            Assert.That(() => { movementTypes.GetMovementCost(MovementTypesFixture.Walking, unknownTerrainType); },
                Throws.ArgumentException.With.Message.EqualTo(
                    "Unknown terrain type: 'Some terrain type'"));
        }

        [Test]
        public void GetType_ShouldReturnTypeByTheId()
        {
            var ground = new TerrainType(1, "Ground");
            var water = new TerrainType(2, "Water");

            var walkingType = new MovementType(1, "Walking");
            var swimmingType = new MovementType(123, "Swimming");

            var movementTypes = new MovementTypes(new[] {ground, water},
                new Dictionary<MovementType, Dictionary<TerrainType, int>>
                {
                    [walkingType] = new Dictionary<TerrainType, int>
                    {
                        [ground] = 1,
                        [water] = 2
                    },
                    [swimmingType] = new Dictionary<TerrainType, int>
                    {
                        [ground] = 2,
                        [water] = 1
                    }
                });

            Assert.That(movementTypes.GetMovementTypeById(1), Is.EqualTo(walkingType));
            Assert.That(movementTypes.GetMovementTypeById(123), Is.EqualTo(swimmingType));
        }
    }
}