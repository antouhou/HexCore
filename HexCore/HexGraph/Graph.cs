using System;
using System.Collections.Generic;
using System.Linq;
using HexCore.AStar;
using HexCore.DataStructures;
using HexCore.Helpers;

namespace HexCore.HexGraph
{
    [Serializable]
    public class Graph : IWeightedGraph
    {
        private static readonly Random Random = new Random();

        // Possible directions to detect neighbors        
        private static readonly List<Coordinate3D> Directions = new List<Coordinate3D>
        {
            new Coordinate3D(+1, -1, 0),
            new Coordinate3D(+1, 0, -1),
            new Coordinate3D(0, +1, -1),
            new Coordinate3D(-1, +1, 0),
            new Coordinate3D(-1, 0, +1),
            new Coordinate3D(0, -1, +1)
        };

        private readonly List<MovementType> _movementTypes;
        private readonly OffsetTypes _offsetType;

        private List<Coordinate3D> _blocked;

        private List<Coordinate3D> _allCoordinates;
        private List<Coordinate3D> _emptyCells;
        private int _height;
        private int _width;

        public List<List<CellState>> Columns { get; } = new List<List<CellState>>();

        public Graph(int width, int height, OffsetTypes offsetType, List<MovementType> movementTypes)
        {
            _offsetType = offsetType;
            _movementTypes = movementTypes;
            Resize(width, height);
        }

        public IEnumerable<Coordinate3D> GetPassableNeighbors(Coordinate3D position)
        {
            return GetNeighbors(position, true);
        }

        public int GetMovementCost(Coordinate3D coordinate, MovementType unitMovementType)
        {
            var cellState = GetCellState(coordinate);
            return unitMovementType.GetCostTo(cellState.MovementType.Name);
        }

        // TODO: Check if it possible to do something with generics
        public List<Coordinate3D> GetAllCellsCoordinates()
        {
            return _allCoordinates;
        }

        public List<Coordinate3D> GetAllEmptyCellsCoordinates()
        {
            return _emptyCells;
        }

        private bool IsThereEmptyCell()
        {
            return _emptyCells.Count > 0;
        }

        public Coordinate3D? GetCoordinateOfRandomEmptyCell()
        {
            /*
             * After having some thoughts on what is better - to return null or to throw an error,
             * I decided to go with returning null - since to me, it looks like it's an ordinary situation,
             * rather than an exception.
             * For example, if you can't get an empty cell it's may be a good idea to empty some cells and
             * call that method again.
             * (Thanks to this thread on StackOverflow: https://softwareengineering.stackexchange.com/questions/159096/return-magic-value-throw-exception-or-return-false-on-failure)
             */
            if (IsThereEmptyCell()) return _emptyCells[Random.Next(_emptyCells.Count)];
            return null;
        }

        private void ResizeColumn(int columnNumber, int newSize)
        {
            var column = Columns[columnNumber];
            if (column.Count > newSize)
            {
                column.RemoveRange(newSize, column.Count - newSize);
                return;
            }

            if (column.Count >= newSize) return;
            for (var rowNumber = column.Count; rowNumber < newSize; rowNumber++)
            {
                var position = new Coordinate2D(columnNumber, rowNumber);
                var cubeCoordinate = CoordinateConverter.ConvertOneOffsetToCube(_offsetType, position);
                column.Add(new CellState(position, false, cubeCoordinate, _movementTypes[0]));
            }
        }

        private void AddNewColumn(int size)
        {
            var column = new List<CellState>();
            Columns.Add(column);
            ResizeColumn(Columns.Count - 1, size);
        }

        /**
         * Resize graph. Mostly used by the editor
         */
        public void Resize(int width, int height)
        {
            if (width > _width)
                for (var columnNumber = _width; columnNumber < width; columnNumber++)
                    AddNewColumn(height);

            if (_width > width) Columns.RemoveRange(width, _width - width);

            if (height != _height)
                for (var columnNumber = 0; columnNumber < Columns.Count; columnNumber++)
                    ResizeColumn(columnNumber, height);

            _width = width;
            _height = height;
            UpdateCoordinatesList();
        }

        public void SetManyCellsBlocked(IEnumerable<Coordinate3D> coordinates, bool isBlocked)
        {
            foreach (var coordinate in coordinates)
            {
                var cellState = GetCellState(coordinate);
                cellState.IsBlocked = isBlocked;
            }

            UpdateCoordinatesList();
        }

        public void SetOneCellBlocked(Coordinate3D coordinate, bool isBlocked)
        {
            SetManyCellsBlocked(new List<Coordinate3D> {coordinate}, isBlocked);
        }

        public void SetManyCellsMovementType(IEnumerable<Coordinate2D> coordinates, MovementType movementType)
        {
            foreach (var coordinate in coordinates)
            {
                var cellState = GetCellState(coordinate);
                cellState.MovementType = movementType;
            }
        }

        public void SetOneCellMovementType(Coordinate2D coordinate, MovementType movementType)
        {
            SetManyCellsMovementType(new List<Coordinate2D> {coordinate}, movementType);
        }

        private void UpdateCoordinatesList()
        {
            _allCoordinates = Columns.SelectMany(col => col).Select(cell => cell.Coordinate3).ToList();
            _blocked = Columns.SelectMany(col => col).Where(cell => cell.IsBlocked).Select(cell => cell.Coordinate3).ToList();
            _emptyCells = Columns.SelectMany(col => col).Where(cell => !cell.IsBlocked).Select(cell => cell.Coordinate3).ToList();
        }

        public IEnumerable<Coordinate2D> GetAllCellsOffsetPositions()
        {
            return Columns.SelectMany(col => col).Select(cell => cell.Coordinate2);
        }

        private bool IsInBounds(Coordinate3D coordinate)
        {
            return _allCoordinates.Contains(coordinate);
        }

        public bool IsCellBlocked(Coordinate3D coordinate)
        {
            return GetCellState(coordinate).IsBlocked;
        }

        public IEnumerable<Coordinate3D> GetNeighbors(Coordinate3D position, bool onlyPassable)
        {
            foreach (var direction in Directions)
            {
                var next = position + direction;
                if (IsInBounds(next) && !(onlyPassable && IsCellBlocked(next))) yield return next;
            }
        }

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

        public List<Coordinate3D> GetMovementRange(Coordinate3D startPosition, int movementPoints,
            MovementType movementType)
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
                    var movementCostToNeighbor = GetMovementCost(neighbor, movementType);
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

        public CellState GetCellState(Coordinate3D coordinate)
        {
            var coordinate2D = CoordinateConverter.ConvertOneCubeToOffset(_offsetType, coordinate);
            return GetCellState(coordinate2D);
        }

        private CellState GetCellState(Coordinate2D coordinate2D)
        {
            return Columns[coordinate2D.X][coordinate2D.Y];
        }

        private struct Fringe
        {
            public Coordinate3D Coordinate;
            public int CostSoFar;
        }
    }
}