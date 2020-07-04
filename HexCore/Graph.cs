using System;
using System.Collections.Generic;
using System.Linq;

namespace HexCore
{
    [Serializable]
    public class Graph : IWeightedGraph
    {
        // Possible directions to detect neighbors        
        public static readonly Coordinate3D[] Directions = {
            new Coordinate3D(+1, -1, 0),
            new Coordinate3D(+1, 0, -1),
            new Coordinate3D(0, +1, -1),
            new Coordinate3D(-1, +1, 0),
            new Coordinate3D(-1, 0, +1),
            new Coordinate3D(0, -1, +1)
        };

        private readonly List<CellState> _cellStatesList = new List<CellState>();

        private List<Coordinate3D> _allCoordinates;

        private Dictionary<Coordinate3D, CellState> _cellStatesDictionary;

        private MovementTypes _movementTypes;

        public Graph(IEnumerable<CellState> cellStatesList, MovementTypes movementTypes)
        {
            _movementTypes = movementTypes;

            AddCells(cellStatesList);
            UpdateCoordinatesList();
        }

        /// <summary>
        ///     Returns passable neighbors of the cell
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public IEnumerable<Coordinate3D> GetPassableNeighbors(Coordinate3D position)
        {
            return GetNeighbors(position, true);
        }
        
        public IEnumerable<Coordinate3D> GetPassableNeighbors(Coordinate2D position)
        {
            return GetPassableNeighbors(position.To3D());
        }

        /// <summary>
        ///     This methods gets movement costs to the coordinate for the movement type in range = 1.
        ///     Used internally by path finding and range finding.
        /// </summary>
        /// <param name="coordinate"></param>
        /// <param name="unitMovementType"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public int GetMovementCostForTheType(Coordinate3D coordinate, IMovementType unitMovementType)
        {
            if (!_movementTypes.ContainsMovementType(unitMovementType))
                throw new InvalidOperationException(
                    $"Unknown movement type: {unitMovementType.Name}");
            var cellState = GetCellState(coordinate);
            return _movementTypes.GetMovementCost(unitMovementType, cellState.TerrainType);
        }

        /* Private methods */

        private void UpdateCellStateDictionary()
        {
            _cellStatesDictionary = new Dictionary<Coordinate3D, CellState>();
            foreach (var cellState in _cellStatesList) _cellStatesDictionary.Add(cellState.Coordinate3, cellState);
        }

        private void UpdateCoordinatesList()
        {
            _allCoordinates = _cellStatesList.Select(cell => cell.Coordinate3).ToList();
        }

        private void SetCellBlockStatus(IEnumerable<Coordinate3D> coordinates, bool isBlocked)
        {
            foreach (var coordinate in coordinates)
            {
                var cellState = GetCellState(coordinate);
                cellState.IsBlocked = isBlocked;
            }
        }

        private void SetCellBlockStatus(Coordinate3D coordinate, bool isBlocked)
        {
            SetCellBlockStatus(new []{coordinate}, isBlocked);
        }

        /* Public methods */

        /// <summary>
        ///     Returns neighbors for the cell
        /// </summary>
        /// <param name="position">Coordinate to get neighbors to</param>
        /// <param name="onlyPassable">Return only passable neighbors</param>
        /// <returns>The list of this cell's neighbors</returns>
        public IEnumerable<Coordinate3D> GetNeighbors(Coordinate3D position, bool onlyPassable)
        {
            return Directions
                .Select(direction => position + direction)
                .Where(next => Contains(next) && !(onlyPassable && IsCellBlocked(next)));
        }
        
        public IEnumerable<Coordinate2D> GetNeighbors(Coordinate2D position, bool onlyPassable)
        {
            return Coordinate3D.To2D(
                GetNeighbors(position.To3D(), onlyPassable),
                position.OffsetType
            );
        }

        public void AddCells(IEnumerable<CellState> newCellStatesList)
        {
            var cellStates = newCellStatesList as CellState[] ?? newCellStatesList.ToArray();
            foreach (var cell in cellStates.Where(cell => !_movementTypes.ContainsTerrainType(cell.TerrainType)))
                throw new InvalidOperationException(
                    $"One of the cells in graph has an unknown type: {cell.TerrainType.Name}");
            _cellStatesList.AddRange(cellStates);
            UpdateCellStateDictionary();
            UpdateCoordinatesList();
        }

        public void RemoveCells(IEnumerable<Coordinate3D> coordinatesToRemove)
        {
            _cellStatesList.RemoveAll(cellState => coordinatesToRemove.Contains(cellState.Coordinate3));
            UpdateCellStateDictionary();
            UpdateCoordinatesList();
        }
        
        public void RemoveCells(IEnumerable<Coordinate2D> coordinatesToRemove2d)
        {
            RemoveCells(Coordinate2D.To3D(coordinatesToRemove2d));
        }

        public List<Coordinate3D> GetAllCellsCoordinates()
        {
            return _allCoordinates;
        }
        
        public List<Coordinate2D> GetAllCellsCoordinates(OffsetTypes offsetType)
        {
            return Coordinate3D.To2D(_allCoordinates, offsetType);
        }

        public List<CellState> GetAllCells()
        {
            return _cellStatesList;
        }

        public void BlockCells(IEnumerable<Coordinate3D> coordinates)
        {
            SetCellBlockStatus(coordinates, true);
        }

        public void BlockCells(Coordinate3D coordinate)
        {
            SetCellBlockStatus(coordinate, true);
        }
        
        public void BlockCells(IEnumerable<Coordinate2D> coordinates)
        {
            BlockCells(Coordinate2D.To3D(coordinates));
        }

        public void BlockCells(Coordinate2D coordinate)
        {
            BlockCells(coordinate.To3D());
        }

        public void UnblockCells(IEnumerable<Coordinate3D> coordinates)
        {
            SetCellBlockStatus(coordinates, false);
        }

        public void UnblockCells(Coordinate3D coordinate)
        {
            SetCellBlockStatus(coordinate, false);
        }
        
        public void UnblockCells(IEnumerable<Coordinate2D> coordinates)
        {
            UnblockCells(Coordinate2D.To3D(coordinates));
        }

        public void UnblockCells(Coordinate2D coordinate)
        {
            UnblockCells(coordinate.To3D());
        }

        public void SetCellsTerrainType(IEnumerable<Coordinate3D> coordinates, ITerrainType terrainType)
        {
            foreach (var coordinate in coordinates)
            {
                var cellState = GetCellState(coordinate);
                cellState.TerrainType = terrainType;
            }
        }

        public void SetCellsTerrainType(Coordinate3D coordinate, ITerrainType terrainType)
        {
            SetCellsTerrainType(new []{coordinate}, terrainType);
        }
        
        public void SetCellsTerrainType(IEnumerable<Coordinate2D> coordinates, ITerrainType terrainType)
        {
            SetCellsTerrainType(Coordinate2D.To3D(coordinates), terrainType);
        }

        public void SetCellsTerrainType(Coordinate2D coordinate, ITerrainType terrainType)
        {
            SetCellsTerrainType(coordinate.To3D(), terrainType);
        }

        /// <summary>
        ///     Returns true if graph contains the coordinate, false otherwise
        /// </summary>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        public bool Contains(Coordinate3D coordinate)
        {
            return _allCoordinates.Contains(coordinate);
        }
        
        public bool Contains(Coordinate2D coordinate)
        {
            return Contains(coordinate.To3D());
        }

        /// <summary>
        ///     Returns true if the cell is marked as not passable, false otherwise
        /// </summary>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        public bool IsCellBlocked(Coordinate3D coordinate)
        {
            return GetCellState(coordinate).IsBlocked;
        }
        
        public bool IsCellBlocked(Coordinate2D coordinate)
        {
            return IsCellBlocked(coordinate.To3D());
        }

        /// <summary>
        ///     Returns circular range of the cell
        /// </summary>
        /// <param name="startPosition">The center of the range</param>
        /// <param name="radius">Radius of the range</param>
        /// <returns></returns>
        public List<Coordinate3D> GetRange(Coordinate3D startPosition, int radius)
        {
            var visited = new List<Coordinate3D> {startPosition};
            var fringes = new List<List<Fringe>>
            {
                new List<Fringe> {new Fringe {Coordinate = startPosition, CostSoFar = 0}}
            };

            for (var currentRange = 0; currentRange < radius; currentRange++)
            {
                var newFringes = new List<Fringe>();
                foreach (var currentFringe in fringes[currentRange])
                foreach (var neighbor in GetNeighbors(currentFringe.Coordinate, false))
                {
                    if (visited.Contains(neighbor)) continue;
                    visited.Add(neighbor);
                    newFringes.Add(new Fringe {Coordinate = neighbor, CostSoFar = 0});
                }

                fringes.Add(newFringes);
            }

            // So start position won't be included in the range
            visited.Remove(startPosition);
            return visited;
        }
        
        public List<Coordinate2D> GetRange(Coordinate2D startPosition, int radius)
        {
            return Coordinate3D.To2D(
                GetRange(startPosition.To3D(), radius), 
                startPosition.OffsetType
            );
        }

        public IEnumerable<Coordinate3D> GetLine(Coordinate3D start, Coordinate3D direction, int length)
        {
            if (!Directions.Contains(direction)) throw new InvalidOperationException("Invalid direction");

            for (var currentLength = 1; currentLength < length + 1; currentLength++)
            {
                var next = start + direction * currentLength;
                if (Contains(next)) yield return next;
            }
        }

        /// <summary>
        ///     Similar to get range, but also takes two additional params:
        /// </summary>
        /// <param name="startPosition">Center of the range</param>
        /// <param name="movementPoints">Amount of points allowed to spend on the movement</param>
        /// <param name="movementType">Movement type of the pawn to calculate movement range based on the movement points</param>
        /// <returns></returns>
        public List<Coordinate3D> GetMovementRange(Coordinate3D startPosition, int movementPoints,
            IMovementType movementType)
        {
            var visited = new List<Coordinate3D> {startPosition};
            var fringes = new List<List<Fringe>>
            {
                new List<Fringe> {new Fringe {Coordinate = startPosition, CostSoFar = 0}}
            };

            for (var currentRangeIndex = 0; currentRangeIndex < movementPoints; currentRangeIndex++)
            {
                var newFringes = new List<Fringe>();
                foreach (var currentFringe in fringes[currentRangeIndex])
                foreach (var neighbor in GetPassableNeighbors(currentFringe.Coordinate))
                {
                    if (visited.Contains(neighbor)) continue;
                    var movementCostToNeighbor = GetMovementCostForTheType(neighbor, movementType);
                    var newCost = currentFringe.CostSoFar + movementCostToNeighbor;
                    if (newCost > movementPoints) continue;
                    visited.Add(neighbor);
                    newFringes.Add(new Fringe {Coordinate = neighbor, CostSoFar = newCost});
                }

                fringes.Add(newFringes);
            }

            // So start position won't be included in the range
            visited.Remove(startPosition);
            return visited;
        }
        
        public List<Coordinate2D> GetMovementRange(Coordinate2D startPosition, int movementPoints,
            IMovementType movementType)
        {
            return Coordinate3D.To2D(
                GetMovementRange(startPosition.To3D(), movementPoints, movementType), 
                startPosition.OffsetType
            );
        }

        /// <summary>
        ///     Returns the state of the cell for a given coordinate
        /// </summary>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        public CellState GetCellState(Coordinate3D coordinate)
        {
            return _cellStatesDictionary[coordinate];
        }
        
        public CellState GetCellState(Coordinate2D coordinate)
        {
            return GetCellState(coordinate.To3D());
        }

        /// <summary>
        ///     Finds shortest path from the start to the goal. Requires pawn's movement type to operate
        /// </summary>
        /// <param name="start"></param>
        /// <param name="goal"></param>
        /// <param name="unitMovementType"></param>
        /// <returns></returns>
        public List<Coordinate3D> GetShortestPath(Coordinate3D start, Coordinate3D goal, IMovementType unitMovementType)
        {
            return AStarSearch.FindShortestPath(this, start, goal, unitMovementType);
        }
        
        public List<Coordinate2D> GetShortestPath(Coordinate2D start, Coordinate2D goal, IMovementType unitMovementType)
        {
            return Coordinate3D.To2D(
                GetShortestPath(start.To3D(), goal.To3D(), unitMovementType),
                start.OffsetType
            );
        }

        private struct Fringe
        {
            public Coordinate3D Coordinate;
            public int CostSoFar;
        }
    }
}