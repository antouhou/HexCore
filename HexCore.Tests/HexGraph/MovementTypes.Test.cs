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
            var ground = new TerrainType(1, "Ground");
            var walking = new MovementType(1, "Walking");
            var movementTypes = new MovementTypes(new ITerrainType[] { ground }, new Dictionary<IMovementType, Dictionary<ITerrainType, int>>
            {
                [walking] = new Dictionary<ITerrainType, int>
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
                    var movementTypes = new MovementTypes(new ITerrainType[] { ground, water }, new Dictionary<IMovementType, Dictionary<ITerrainType, int>>
                    {
                        [walkingType] = new Dictionary<ITerrainType, int>
                        {
                            [ground] = 1,
                            [water] = 2,
                            [air] = 1
                        },
                        [swimmingType] = new Dictionary<ITerrainType, int>
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
                    var movementTypes = new MovementTypes(new ITerrainType[] { ground, water, air }, new Dictionary<IMovementType, Dictionary<ITerrainType, int>>
                    {
                        [walkingType] = new Dictionary<ITerrainType, int>
                        {
                            [ground] = 1
                        },
                        [swimmingType] = new Dictionary<ITerrainType, int>
                        {
                            [ground] = 2,
                            [water] = 1
                        },
                        [flyingType] = new Dictionary<ITerrainType, int>
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
                    var movementTypes = new MovementTypes(new ITerrainType[] { ground, water }, new Dictionary<IMovementType, Dictionary<ITerrainType, int>>
                    {
                        [walkingType] = new Dictionary<ITerrainType, int>
                        {
                            [ground] = 1,
                            [water] = 2
                        },
                        [swimmingType] = new Dictionary<ITerrainType, int>
                        {
                            [ground] = 2,
                            [water] = 1
                        }
                    });
                },
                Throws.ArgumentException.With.Message.EqualTo(
                    "An item with the same key has already been added. Key: 1"));
        }

        [Test]
        public void Constructor_ShouldThrowIfAnEmptyDictionaryIsPassed()
        {
            var ground = new TerrainType(1, "Ground");
            var walking = new MovementType(1, "Walking");
            Assert.That(() =>
                {
                    var movementTypes =
                        new MovementTypes(new ITerrainType[] { ground }, new Dictionary<IMovementType, Dictionary<ITerrainType, int>>());
                },
                Throws.ArgumentException.With.Message.EqualTo(
                    "Movement types should always have at least one explicitly defined type. For the reasoning, please visit the movement types section in the library's docs"));
            
            Assert.That(() =>
                {
                    var movementTypes =
                        new MovementTypes(new ITerrainType[] { }, new Dictionary<IMovementType, Dictionary<ITerrainType, int>>()
                        {
                            [walking] = new Dictionary<ITerrainType, int>()
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

            var movementTypes = new MovementTypes(new ITerrainType[] { ground, water }, new Dictionary<IMovementType, Dictionary<ITerrainType, int>>
            {
                [walkingType] = new Dictionary<ITerrainType, int>
                {
                    [ground] = 1,
                    [water] = 2
                },
                [swimmingType] = new Dictionary<ITerrainType, int>
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

            var movementTypes = new MovementTypes(new ITerrainType[] { ground, water }, new Dictionary<IMovementType, Dictionary<ITerrainType, int>>
            {
                [walkingType] = new Dictionary<ITerrainType, int>
                {
                    [ground] = 1,
                    [water] = 2
                },
                [swimmingType] = new Dictionary<ITerrainType, int>
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
            
            var movementTypes = new MovementTypes(new ITerrainType[]
            {
                ground, water, air
            }, new Dictionary<IMovementType, Dictionary<ITerrainType, int>>
            {
                [walkingType] = new Dictionary<ITerrainType, int>
                {
                    [ground] = 1,
                    [water] = 2,
                    [air] = 999
                },
                [swimmingType] = new Dictionary<ITerrainType, int>
                {
                    [ground] = 2,
                    [water] = 1,
                    [air] = 999
                },
                [flyingType] = new Dictionary<ITerrainType, int>
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
        public void GetType_ShouldReturnTypeByTheId()
        {
            var ground = new TerrainType(1, "Ground");
            var water = new TerrainType(2, "Water");

            var walkingType = new MovementType(1, "Walking");
            var swimmingType = new MovementType(123, "Swimming");

            var movementTypes = new MovementTypes(new ITerrainType[] { ground, water }, new Dictionary<IMovementType, Dictionary<ITerrainType, int>>
            {
                [walkingType] = new Dictionary<ITerrainType, int>
                {
                    [ground] = 1,
                    [water] = 2
                },
                [swimmingType] = new Dictionary<ITerrainType, int>
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