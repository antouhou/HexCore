using System;
using System.Collections.Generic;
using System.Linq;

namespace HexCore
{
    [Serializable]
    public class Graph : IWeightedGraph
    {
        // Possible directions to detect neighbors        
        public static readonly Coordinate3D[] Directions =
        {
            new Coordinate3D(0, +1, -1),
            new Coordinate3D(+1, 0, -1),
            new Coordinate3D(+1, -1, 0),
            new Coordinate3D(0, -1, +1),
            new Coordinate3D(-1, 0, +1),
            new Coordinate3D(-1, +1, 0)
        };

        internal static readonly ObjectPoolProvider PoolProvider = new ObjectPoolProvider();

        private Dictionary<Coordinate3D, CellState> _cells;

        // Although only dictionary is usually used, this array is needed for unity serialization
        public List<CellState> CellsList = new List<CellState>();
        public MovementTypes MovementTypes;

        public Graph(List<CellState> cellStates, MovementTypes movementAndTerrainTypes)
        {
            MovementTypes = movementAndTerrainTypes;

            AddCells(cellStates);
        }
        
        private Dictionary<Coordinate3D, CellState> Cells
        {
            get
            {
                // This code is needed to repopulate the dict for unity serialization,
                // as it can't serialize dictionaries
                if (_cells != null) return _cells;
                _cells = new Dictionary<Coordinate3D, CellState>();
                foreach (var cellState in CellsList) Cells.Add(cellState.Coordinate3, cellState);

                return _cells;
            }
        }

        /// <summary>
        ///     Returns passable neighbors of the cell
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public List<Coordinate3D> GetPassableNeighbors(Coordinate3D position)
        {
            return GetNeighbors(position, true);
        }

        /// <summary>
        ///     This methods gets movement costs to the coordinate for the movement type in range = 1.
        ///     Used internally by path finding and range finding.
        /// </summary>
        /// <param name="coordinate"></param>
        /// <param name="unitMovementType"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public int GetMovementCostForTheType(Coordinate3D coordinate, MovementType unitMovementType)
        {
            var cellState = GetCellState(coordinate);
            return MovementTypes.GetMovementCost(unitMovementType, cellState.TerrainType);
        }

        public void ReturnListToPool(List<Coordinate3D> list)
        {
            PoolProvider.CoordinateListPool.Return(list);
        }

        public List<Coordinate3D> GetListFromPool()
        {
            return PoolProvider.CoordinateListPool.Get();
        }

        public List<Coordinate2D> GetPassableNeighbors(Coordinate2D position)
        {
            var list = GetNeighbors(position.To3D(), true);
            var neighbors = Coordinate3D.To2D(
                list,
                position.OffsetType
            );

            ReturnListToPool(list);
            return neighbors;
        }

        /* Private methods */
        private void SetCellBlockStatus(List<Coordinate3D> coordinates, bool isBlocked)
        {
            foreach (var coordinate in coordinates) SetCellBlockStatus(coordinate, isBlocked);
        }

        private void SetCellBlockStatus(Coordinate3D coordinate, bool isBlocked)
        {
            var cellState = GetCellState(coordinate);
            cellState.IsBlocked = isBlocked;
        }

        /* Public methods */

        /// <summary>
        ///     Returns neighbors for the cell
        /// </summary>
        /// <param name="position">Coordinate to get neighbors to</param>
        /// <param name="onlyPassable">Return only passable neighbors</param>
        /// <returns>The list of this cell's neighbors</returns>
        public List<Coordinate3D> GetNeighbors(Coordinate3D position, bool onlyPassable)
        {
            var list = PoolProvider.CoordinateListPool.Get();
            for (var i = 0; i < Directions.Length; i++)
            {
                var neighbor = position + Directions[i];
                var returnNeighbour = onlyPassable ? IsPassable(neighbor) : Contains(neighbor);
                if (returnNeighbour) list.Add(neighbor);
            }

            return list;
        }

        private bool IsPassable(Coordinate3D coordinate3D)
        {
            return Contains(coordinate3D) && !IsCellBlocked(coordinate3D);
        }

        public List<Coordinate2D> GetNeighbors(Coordinate2D position, bool onlyPassable)
        {
            var list = GetNeighbors(position.To3D(), onlyPassable);
            var neighbors = Coordinate3D.To2D(
                list,
                position.OffsetType
            );

            ReturnListToPool(list);
            return neighbors;
        }

        public void AddCells(List<CellState> cellStates)
        {
            foreach (var cell in cellStates.Where(cell => !MovementTypes.ContainsTerrainType(cell.TerrainType)))
                throw new ArgumentException(
                    $"One of the cells in graph has an unknown terrain type: '{cell.TerrainType.GetName()}'");

            foreach (var cellState in cellStates) Cells.Add(cellState.Coordinate3, cellState);
            CellsList.AddRange(cellStates);
        }

        public void RemoveCells(List<Coordinate3D> coordinatesToRemove)
        {
            foreach (var coordinate in coordinatesToRemove)
            {
                RemoveCell(coordinate);
            }
        }

        public void RemoveCells(List<Coordinate2D> coordinatesToRemove2d)
        {
            RemoveCells(Coordinate2D.To3D(coordinatesToRemove2d));
        }

        public void RemoveCell(Coordinate3D coordinate3D)
        {
            CellsList.Remove(Cells[coordinate3D]);
            Cells.Remove(coordinate3D);
        }

        public List<Coordinate3D> GetAllCellsCoordinates()
        {
            var list = GetListFromPool();
            list.AddRange(Cells.Keys);
            return list;
        }

        public List<Coordinate2D> GetAllCellsCoordinates(OffsetTypes offsetType)
        {
            var result = PoolProvider.OffsetCoordinateListPool.Get();
            var cells = GetAllCellsCoordinates();
            Coordinate3D.To2D(cells, offsetType, result);

            ReturnListToPool(cells);
            return result;
        }

        public List<CellState> GetAllCells()
        {
            var list = PoolProvider.CellStatesListPool.Get();
            list.AddRange(Cells.Values);
            return list;
        }

        public void BlockCells(List<Coordinate3D> coordinates)
        {
            SetCellBlockStatus(coordinates, true);
        }

        public void BlockCells(Coordinate3D coordinate)
        {
            SetCellBlockStatus(coordinate, true);
        }

        public void BlockCells(List<Coordinate2D> coordinates)
        {
            BlockCells(Coordinate2D.To3D(coordinates));
        }

        public void BlockCells(Coordinate2D coordinate)
        {
            BlockCells(coordinate.To3D());
        }

        public void UnblockCells(List<Coordinate3D> coordinates)
        {
            SetCellBlockStatus(coordinates, false);
        }

        public void UnblockCells(Coordinate3D coordinate)
        {
            SetCellBlockStatus(coordinate, false);
        }

        public void UnblockCells(List<Coordinate2D> coordinates)
        {
            UnblockCells(Coordinate2D.To3D(coordinates));
        }

        public void UnblockCells(Coordinate2D coordinate)
        {
            UnblockCells(coordinate.To3D());
        }

        public void SetCellsTerrainType(List<Coordinate3D> coordinates, TerrainType terrainType)
        {
            foreach (var coordinate in coordinates) SetCellsTerrainType(coordinate, terrainType);
        }

        public void SetCellsTerrainType(Coordinate3D coordinate, TerrainType terrainType)
        {
            var cellState = GetCellState(coordinate);
            cellState.TerrainType = terrainType;
        }

        public void SetCellsTerrainType(List<Coordinate2D> coordinates, TerrainType terrainType)
        {
            SetCellsTerrainType(Coordinate2D.To3D(coordinates), terrainType);
        }

        public void SetCellsTerrainType(Coordinate2D coordinate, TerrainType terrainType)
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
            return Cells.ContainsKey(coordinate);
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
            return Cells[coordinate].IsBlocked;
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
            var currentFringes = PoolProvider.FringeListPool.Get();
            var nextFringes = PoolProvider.FringeListPool.Get();
            var range = PoolProvider.CoordinateHashSetPool.Get();
            var result = GetListFromPool();

            range.Add(startPosition);
            nextFringes.Add(new Fringe {Coordinate = startPosition, CostSoFar = 0});

            for (var currentRangePoints = 0; currentRangePoints < radius; currentRangePoints++)
            {
                currentFringes.Clear();
                currentFringes.AddRange(nextFringes);
                nextFringes.Clear();

                foreach (var currentFringe in currentFringes)
                {
                    var neighbors = GetNeighbors(currentFringe.Coordinate, false);
                    foreach (var neighbor in neighbors)
                    {
                        if (range.Contains(neighbor)) continue;
                        range.Add(neighbor);
                        result.Add(neighbor);
                        nextFringes.Add(new Fringe {Coordinate = neighbor, CostSoFar = 0});
                    }

                    ReturnListToPool(neighbors);
                }
            }

            PoolProvider.FringeListPool.Return(currentFringes);
            PoolProvider.FringeListPool.Return(nextFringes);
            PoolProvider.CoordinateHashSetPool.Return(range);

            return result;
        }

        public List<Coordinate2D> GetRange(Coordinate2D startPosition, int radius)
        {
            var list = GetRange(startPosition.To3D(), radius);
            var result = PoolProvider.OffsetCoordinateListPool.Get();
            Coordinate3D.To2D(
                list,
                startPosition.OffsetType,
                result
            );

            ReturnListToPool(list);
            return result;
        }

        public List<Coordinate3D> GetLine(Coordinate3D start, Coordinate3D direction, int length)
        {
            if (!Directions.Contains(direction)) throw new InvalidOperationException("Invalid direction");
            var result = GetListFromPool();

            for (var currentLength = 1; currentLength < length + 1; currentLength++)
            {
                var next = start + direction * currentLength;
                if (Contains(next)) result.Add(next);
            }

            return result;
        }

        /// <summary>
        ///     Similar to get range, but also takes two additional params:
        /// </summary>
        /// <param name="startPosition">Center of the range</param>
        /// <param name="movementPoints">Amount of points allowed to spend on the movement</param>
        /// <param name="movementType">Movement type of the pawn to calculate movement range based on the movement points</param>
        /// <returns></returns>
        public List<Coordinate3D> GetMovementRange(Coordinate3D startPosition, int movementPoints,
            MovementType movementType)
        {
            var currentFringes = PoolProvider.FringeListPool.Get();
            var nextFringes = PoolProvider.FringeListPool.Get();
            var range = PoolProvider.CoordinateHashSetPool.Get();
            var result = PoolProvider.CoordinateListPool.Get();

            range.Add(startPosition);
            nextFringes.Add(new Fringe {Coordinate = startPosition, CostSoFar = 0});

            for (var currentRangePoints = 0; currentRangePoints < movementPoints; currentRangePoints++)
            {
                currentFringes.Clear();
                currentFringes.AddRange(nextFringes);
                nextFringes.Clear();

                foreach (var currentFringe in currentFringes)
                {
                    var neighbors = GetPassableNeighbors(currentFringe.Coordinate);

                    foreach (var neighbor in neighbors)
                    {
                        if (range.Contains(neighbor)) continue;
                        var movementCostToNeighbor = GetMovementCostForTheType(neighbor, movementType);
                        var newCost = currentFringe.CostSoFar + movementCostToNeighbor;
                        if (newCost > movementPoints) continue;
                        range.Add(neighbor);
                        result.Add(neighbor);
                        nextFringes.Add(new Fringe {Coordinate = neighbor, CostSoFar = newCost});
                    }

                    ReturnListToPool(neighbors);
                }
            }

            PoolProvider.FringeListPool.Return(currentFringes);
            PoolProvider.FringeListPool.Return(nextFringes);
            PoolProvider.CoordinateHashSetPool.Return(range);

            return result;
        }

        public List<Coordinate2D> GetMovementRange(Coordinate2D startPosition, int movementPoints,
            MovementType movementType)
        {
            var list = GetMovementRange(startPosition.To3D(), movementPoints, movementType);
            var result = PoolProvider.OffsetCoordinateListPool.Get();
            Coordinate3D.To2D(
                list,
                startPosition.OffsetType,
                result
            );

            ReturnListToPool(list);
            return result;
        }

        public bool IsWithinMovementRange(Coordinate2D startPosition, Coordinate2D target, int movementPoints,
            MovementType movementType)
        {
            var target3D = target.To3D();
            var result = false;
            var list = GetMovementRange(startPosition.To3D(), movementPoints, movementType);

            foreach (var coordinate in list)
                if (coordinate.Equals(target3D))
                    result = true;

            ReturnListToPool(list);
            return result;
        }

        /// <summary>
        ///     Returns the state of the cell for a given coordinate
        /// </summary>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        public CellState GetCellState(Coordinate3D coordinate)
        {
            return Cells[coordinate];
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
        public List<Coordinate3D> GetShortestPath(Coordinate3D start, Coordinate3D goal, MovementType unitMovementType)
        {
            return AStarSearch.FindShortestPath(this, start, goal, unitMovementType);
        }

        public List<Coordinate2D> GetShortestPath(Coordinate2D start, Coordinate2D goal, MovementType unitMovementType)
        {
            var path = GetShortestPath(start.To3D(), goal.To3D(), unitMovementType);
            var result = PoolProvider.OffsetCoordinateListPool.Get();

            Coordinate3D.To2D(
                path,
                start.OffsetType,
                result
            );

            ReturnListToPool(path);
            return result;
        }

        public void ReturnListToPool(List<Coordinate2D> list)
        {
            PoolProvider.OffsetCoordinateListPool.Return(list);
        }

        public void ReturnListToPool(List<CellState> list)
        {
            PoolProvider.CellStatesListPool.Return(list);
        }

        [Serializable]
        public struct Fringe
        {
            public Coordinate3D Coordinate;
            public int CostSoFar;
        }
    }
}