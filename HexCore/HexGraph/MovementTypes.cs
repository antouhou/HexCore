using System;
using System.Collections.Generic;
using System.Linq;

namespace HexCore.HexGraph
{
    public class MovementTypes
    {
        private readonly Dictionary<int, IMovementType> _ids = new Dictionary<int, IMovementType>();

        private readonly Dictionary<IMovementType, int> _inverseIds = new Dictionary<IMovementType, int>();

        // <unit_type<position_type, cost>>
        private readonly Dictionary<IMovementType, Dictionary<IMovementType, int>> _movementCosts =
            new Dictionary<IMovementType, Dictionary<IMovementType, int>>();

        private readonly HashSet<IMovementType> _movementTypes;

        // Public constructor
        public MovementTypes(Dictionary<IMovementType, Dictionary<IMovementType, int>> movementTypesWithCosts)
        {
            if (!movementTypesWithCosts.Any())
                throw new ArgumentException(
                    "Movement types should always have at least one explicitly defined type. For the reasoning, please visit the movement types section in the library's docs");
            _movementTypes = new HashSet<IMovementType>(movementTypesWithCosts.Keys);
            foreach (var movementType in movementTypesWithCosts)
            {
                // For fast lookups
                _ids.Add(movementType.Key.Id, movementType.Key);
                _inverseIds.Add(movementType.Key, movementType.Key.Id);
                AddCostsForType(movementType.Key, movementType.Value);
            }
        }

        // Private methods
        private void AddCostsForType(
            IMovementType type,
            Dictionary<IMovementType, int> movementCostsUnitToPosition
        )
        {
            var missingTypes = GetAllTypes().Except(movementCostsUnitToPosition.Keys).ToArray();
            if (missingTypes.Any())
            {
                var missingTypesEnumerationString = CreateTypesEnumerationString(missingTypes);
                throw new ArgumentException(
                    $"Error when adding movement type '{type.Name}': missing movement costs to {missingTypesEnumerationString}");
            }

            var excessTypes = movementCostsUnitToPosition.Keys.Except(GetAllTypes()).ToArray();
            if (excessTypes.Any())
            {
                var excessTypesEnumerationString = CreateTypesEnumerationString(excessTypes);
                throw new ArgumentException(
                    $"Error when adding movement type '{type.Name}': movement costs contain unknown {excessTypesEnumerationString}");
            }

            _movementCosts.Add(type, movementCostsUnitToPosition);
        }

        private static string CreateTypesEnumerationString(IMovementType[] movementTypes)
        {
            var movementTypeNames = movementTypes
                .Select(movementType => movementType.Name).ToArray();
            var movementTypesNamesConcatenated = string.Join("', '", movementTypeNames);
            var pluralEnding = movementTypeNames.Length > 1 ? "s" : "";
            return $"type{pluralEnding}: '{movementTypesNamesConcatenated}'";
        }

        // Public methods
        public int GetId(IMovementType movementType)
        {
            return _inverseIds[movementType];
        }

        public IMovementType GetType(int typeId)
        {
            return _ids[typeId];
        }

        public IEnumerable<IMovementType> GetAllTypes()
        {
            return _movementTypes;
        }

        public int GetMovementCost(IMovementType from, IMovementType to)
        {
            return _movementCosts[from][to];
        }

        public bool Contains(IMovementType movementType)
        {
            return GetAllTypes().Contains(movementType);
        }
    }
}