using System;
using System.Collections.Generic;
using System.Linq;

namespace HexCore
{
    public class MovementTypes
    {
        private readonly Dictionary<IMovementType, int> _inverseMovementTypeIds = new Dictionary<IMovementType, int>();

        private readonly Dictionary<ITerrainType, int> _inverseTerrainTypeIds = new Dictionary<ITerrainType, int>();

        // <movement_type<terrain_type, cost>>
        private readonly Dictionary<IMovementType, Dictionary<ITerrainType, int>> _movementCosts =
            new Dictionary<IMovementType, Dictionary<ITerrainType, int>>();

        private readonly Dictionary<int, IMovementType> _movementTypeIds = new Dictionary<int, IMovementType>();

        private readonly HashSet<IMovementType> _movementTypes;
        private readonly Dictionary<int, ITerrainType> _terrainTypeIds = new Dictionary<int, ITerrainType>();
        private readonly HashSet<ITerrainType> _terrainTypes;

        // Public constructor
        public MovementTypes(ITerrainType[] terrainTypes,
            Dictionary<IMovementType, Dictionary<ITerrainType, int>> movementTypesWithCosts)
        {
            if (!movementTypesWithCosts.Any())
                throw new ArgumentException(
                    "Movement types should always have at least one explicitly defined type. For the reasoning, please visit the movement types section in the library's docs");
            _movementTypes = new HashSet<IMovementType>(movementTypesWithCosts.Keys);
            _terrainTypes = new HashSet<ITerrainType>(terrainTypes);

            foreach (var terrainType in _terrainTypes)
            {
                _terrainTypeIds.Add(terrainType.Id, terrainType);
                _inverseTerrainTypeIds.Add(terrainType, terrainType.Id);
            }

            foreach (var movementType in movementTypesWithCosts)
            {
                // For fast lookups
                _movementTypeIds.Add(movementType.Key.Id, movementType.Key);
                _inverseMovementTypeIds.Add(movementType.Key, movementType.Key.Id);
                AddCostsForType(movementType.Key, movementType.Value);
            }
        }

        // Private methods
        private void AddCostsForType(
            IMovementType type,
            Dictionary<ITerrainType, int> movementCostsToTerrain
        )
        {
            var missingTypes = GetAllTerrainTypes().Except(movementCostsToTerrain.Keys).ToArray();
            if (missingTypes.Any())
            {
                var missingTypesEnumerationString = CreateTypesEnumerationString(missingTypes);
                throw new ArgumentException(
                    $"Error when adding movement type '{type.Name}': missing movement costs to {missingTypesEnumerationString}");
            }

            var excessTypes = movementCostsToTerrain.Keys.Except(GetAllTerrainTypes()).ToArray();
            if (excessTypes.Any())
            {
                var excessTypesEnumerationString = CreateTypesEnumerationString(excessTypes);
                throw new ArgumentException(
                    $"Error when adding movement type '{type.Name}': movement costs contain unknown {excessTypesEnumerationString}");
            }

            _movementCosts.Add(type, movementCostsToTerrain);
        }

        private static string CreateTypesEnumerationString(ITerrainType[] movementTypes)
        {
            var movementTypeNames = movementTypes
                .Select(movementType => movementType.Name).ToArray();
            var movementTypesNamesConcatenated = string.Join("', '", movementTypeNames);
            var pluralEnding = movementTypeNames.Length > 1 ? "s" : "";
            return $"type{pluralEnding}: '{movementTypesNamesConcatenated}'";
        }

        // Public methods
        public int GetMovementTypeId(IMovementType movementType)
        {
            return _inverseMovementTypeIds[movementType];
        }

        public IMovementType GetMovementTypeById(int typeId)
        {
            return _movementTypeIds[typeId];
        }

        public IEnumerable<IMovementType> GetAllMovementTypes()
        {
            return _movementTypes;
        }

        public IEnumerable<ITerrainType> GetAllTerrainTypes()
        {
            return _terrainTypes;
        }

        public int GetMovementCost(IMovementType from, ITerrainType to)
        {
            return _movementCosts[from][to];
        }

        public bool ContainsMovementType(IMovementType movementType)
        {
            return GetAllMovementTypes().Contains(movementType);
        }

        public bool ContainsTerrainType(ITerrainType movementType)
        {
            return GetAllTerrainTypes().Contains(movementType);
        }
    }
}