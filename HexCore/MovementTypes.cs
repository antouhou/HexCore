using System;
using System.Collections.Generic;
using System.Linq;

namespace HexCore
{
    [Serializable]
    public struct Cost
    {
        public TerrainType TerrainType;
        public MovementType MovementType;
        public int MovementCost;
    }

    [Serializable]
    public class MovementTypes
    {
        public List<Cost> Costs;
        public MovementType[] MovementTypesArray;
        public HashSet<MovementType> MovementTypesHashSet;
        private MovementType selectedMovementType;

        private TerrainType selectedTerrainType;
        public HashSet<TerrainType> TerrainsHashSet;
        public TerrainType[] TerrainTypes;

        // Public constructor
        public MovementTypes(TerrainType[] terrainTypes,
            Dictionary<MovementType, Dictionary<TerrainType, int>> movementTypesWithCosts)
        {
            if (!movementTypesWithCosts.Any())
                throw new ArgumentException(
                    "Movement types should always have at least one explicitly defined type. For the reasoning, please visit the movement types section in the library's docs");
            MovementTypesHashSet = new HashSet<MovementType>(movementTypesWithCosts.Keys, new MovementTypeComparer());
            MovementTypesArray = MovementTypesHashSet.ToArray();
            TerrainsHashSet = new HashSet<TerrainType>(terrainTypes, new TerrainTypeComparer());
            TerrainTypes = TerrainsHashSet.ToArray();
            Costs = new List<Cost>();
            var duplicatedIds = MovementTypesArray
                .GroupBy(type => type.GetId())
                .Where(group => group.Count() > 1)
                .Select(group => group.Key)
                .ToArray();

            if (duplicatedIds.Any())
                throw new ArgumentException(
                    $"Some of the movement types have same Ids. Ids: {string.Join("', '", duplicatedIds)}");

            foreach (var movementType in movementTypesWithCosts) AddCostsForType(movementType.Key, movementType.Value);
        }

        // Private methods
        private void AddCostsForType(
            MovementType type,
            Dictionary<TerrainType, int> movementCostsToTerrain
        )
        {
            var missingTypes = GetAllTerrainTypes().Except(movementCostsToTerrain.Keys).ToArray();
            if (missingTypes.Any())
            {
                var missingTypesEnumerationString = CreateTypesEnumerationString(missingTypes);
                throw new ArgumentException(
                    $"Error when adding movement type '{type.GetName()}': missing movement costs to {missingTypesEnumerationString}");
            }

            var excessTypes = movementCostsToTerrain.Keys.Except(GetAllTerrainTypes()).ToArray();
            if (excessTypes.Any())
            {
                var excessTypesEnumerationString = CreateTypesEnumerationString(excessTypes);
                throw new ArgumentException(
                    $"Error when adding movement type '{type.GetName()}': movement costs contain unknown {excessTypesEnumerationString}");
            }

            foreach (var terrainAndCost in movementCostsToTerrain)
            {
                var terrain = terrainAndCost.Key;
                var costValue = terrainAndCost.Value;
                Costs.Add(new Cost
                {
                    TerrainType = terrain,
                    MovementType = type,
                    MovementCost = costValue
                });
            }
        }

        private static string CreateTypesEnumerationString(TerrainType[] movementTypes)
        {
            var movementTypeNames = movementTypes
                .Select(movementType => movementType.GetName()).ToArray();
            var movementTypesNamesConcatenated = string.Join("', '", movementTypeNames);
            var pluralEnding = movementTypeNames.Length > 1 ? "s" : "";
            return $"type{pluralEnding}: '{movementTypesNamesConcatenated}'";
        }

        public int GetMovementTypeId(MovementType movementType)
        {
            return movementType.GetId();
        }

        public MovementType GetMovementTypeById(int typeId)
        {
            MovementType movementType = default;
            foreach (var type in MovementTypesArray)
            {
                if (type.GetId() != typeId) continue;
                movementType = type;
                break;
            }

            return movementType;
        }

        public MovementType[] GetAllMovementTypes()
        {
            return MovementTypesArray;
        }

        public TerrainType[] GetAllTerrainTypes()
        {
            return TerrainTypes;
        }

        public int GetMovementCost(MovementType pawnMovementType, TerrainType terrainType)
        {
            if (!ContainsMovementType(pawnMovementType))
                throw new ArgumentException(
                    $"Unknown movement type: '{pawnMovementType.GetName()}'");
            if (!ContainsTerrainType(terrainType))
                throw new ArgumentException(
                    $"Unknown terrain type: '{terrainType.GetName()}'");

            selectedMovementType = pawnMovementType;
            selectedTerrainType = terrainType;

            var costValue = int.MaxValue;
            foreach (var cost in Costs)
            {
                if (!cost.TerrainType.Equals(selectedTerrainType) ||
                    !cost.MovementType.Equals(selectedMovementType)) continue;
                costValue = cost.MovementCost;
                break;
            }

            return costValue;
        }

        public bool ContainsMovementType(MovementType movementType)
        {
            if (MovementTypesHashSet == null)
                MovementTypesHashSet = new HashSet<MovementType>(MovementTypesArray, new MovementTypeComparer());
            return MovementTypesHashSet.Contains(movementType);
        }

        public bool ContainsTerrainType(TerrainType movementType)
        {
            if (TerrainsHashSet == null)
                TerrainsHashSet = new HashSet<TerrainType>(TerrainTypes, new TerrainTypeComparer());
            return TerrainsHashSet.Contains(movementType);
        }
    }
}