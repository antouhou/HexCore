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
        // Possible directions to detect neighbors        
        private readonly List<Coordinate3D> _directions = new List<Coordinate3D>
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
        public readonly List<List<CellState>> Columns = new List<List<CellState>>();

        private List<Coordinate3D> _blocked;

        private List<Coordinate3D> _cubeCoordinates;
        private int _height;
        private int _width;

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
            var cellState = GetCellStateByCoordinate3D(coordinate);
            return unitMovementType.GetCostTo(cellState.MovementType.Name);
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

        public void SetManyCellsBlocked(IEnumerable<Coordinate2D> coordinates, bool isBlocked)
        {
            foreach (var coordinate in coordinates)
            {
                var cellState = GetCellStateByCoordinate2D(coordinate);
                cellState.IsBlocked = isBlocked;
            }

            UpdateCoordinatesList();
        }

        public void SetOneCellBlocked(Coordinate2D coordinate, bool isBlocked)
        {
            SetManyCellsBlocked(new List<Coordinate2D> {coordinate}, isBlocked);
        }

        public void SetManyCellsMovementType(IEnumerable<Coordinate2D> coordinates, MovementType movementType)
        {
            foreach (var coordinate in coordinates)
            {
                var cellState = GetCellStateByCoordinate2D(coordinate);
                cellState.MovementType = movementType;
            }
        }

        public void SetOneCellMovementType(Coordinate2D coordinate, MovementType movementType)
        {
            SetManyCellsMovementType(new List<Coordinate2D> {coordinate}, movementType);
        }

        private void UpdateCoordinatesList()
        {
            _cubeCoordinates = GetAllCellsCubeCoordinates().ToList();
            _blocked = GetBlockedCells().ToList();
        }

        public IEnumerable<Coordinate2D> GetAllCellsOffsetPosition()
        {
            return Columns.SelectMany(col => col).Select(cell => cell.Coordinate2);
        }

        private IEnumerable<Coordinate3D> GetAllCellsCubeCoordinates()
        {
            return Columns.SelectMany(col => col).Select(cell => cell.Coordinate3);
        }

        private IEnumerable<Coordinate3D> GetBlockedCells()
        {
            return Columns.SelectMany(col => col).Where(cell => cell.IsBlocked).Select(cell => cell.Coordinate3);
        }

        private bool IsInBounds(Coordinate3D coordinate)
        {
            return _cubeCoordinates.Contains(coordinate);
        }

        private bool IsBlocked(Coordinate3D coordinate)
        {
            return _blocked.Contains(coordinate);
        }

        public IEnumerable<Coordinate3D> GetNeighbors(Coordinate3D position, bool onlyPassable)
        {
            foreach (var direction in _directions)
            {
                var next = position + direction;
                if (IsInBounds(next) && !(onlyPassable && IsBlocked(next))) yield return next;
            }
        }

        public List<Coordinate3D> GetMovableArea(Coordinate3D startPosition, int distance, MovementType movementType)
        {
            var visited = new List<Coordinate3D> {startPosition};
            var fringes = new List<List<Fringe>>
            {
                new List<Fringe> {new Fringe {Coordinate = startPosition, CostSoFar = 0}}
            };

            for (var k = 1; k < distance; k++)
            {
                var fringe = new List<Fringe>();
                foreach (var position in fringes[k - 1])
                foreach (var neighbor in GetPassableNeighbors(position.Coordinate))
                {
                    if (visited.Contains(neighbor)) continue;
                    var movementCostToNeighbor = GetMovementCost(neighbor, movementType);
                    var totalCost = position.CostSoFar + movementCostToNeighbor;
                    if (totalCost > distance) continue;
                    visited.Add(neighbor);
                    fringe.Add(new Fringe {Coordinate = neighbor, CostSoFar = totalCost});
                }

                fringes.Add(fringe);
            }

            // So start position won't be included in the area
            visited.Remove(startPosition);
            return visited;
        }

        private CellState GetCellStateByCoordinate3D(Coordinate3D coordinate3D)
        {
            var coordinate2D = CoordinateConverter.ConvertOneCubeToOffset(_offsetType, coordinate3D);
            return GetCellStateByCoordinate2D(coordinate2D);
        }

        private CellState GetCellStateByCoordinate2D(Coordinate2D coordinate2D)
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