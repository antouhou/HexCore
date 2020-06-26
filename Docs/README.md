# HexCore Documentation

Welcome to the HexCore documentation! HexCore is a library to perform various operations with a hexagonal grid, such as finding shortest paths from one cell to another, managing terrains and movement types, maintaining the grid state, finding various ranges, neighbors, coordinate systems and converters, and some more stuff you may want to do with a hex grid. This is an in-depth doc, if you're looking for a quick start guide, please take a look here: [Quickstart](../README.md#quickstart)

- [Introduction](#core-principles)
- [Terrain and movement types](#terrain-and-movement-types)
- [Cell states](#cell-state)
- [Coordinate systems](#coordinates)
- Find the shortest path
- Get the pawn's movement range
- Changing state of the cells

## Core principles

The base class is `HexCore.Graph`. On order to work, graph needs some data: Cell states and movement types. Cell state keeps all the info about a particular cell: its coordinate, whether its occupied or not, and its terrain type. Terrain types are needed to calculate movement range for a pawn on a particular terrain (for example, pawn with the movement type "walking" should receive a penalty when swimming through "water").

### Terrain and movement types

As explained above, movement types are essential for the graph. There's a helper class to help you maintain your terrain and movement types. It's called `MovementTypes`. It maintains a table of your movement and terrain types. It's mandatory to specify movement types for a graph. Movement types creation looks like this:

*Note: If you want to have just the same constant movement cost across the whole map, create only one terrain and movement type with a movement cost of 1. One terrain type with a movement cost of 1 is not a default option to ensure that this is a deliberate choice and not a mistake.*

```c#
var ground = new TerrainType(1, "Ground");
var water = new TerrainType(2, "Water");

var walkingType = new MovementType(1, "Walking");
var swimmingType = new MovementType(2, "Swimming");

var movementTypes = new MovementTypes(
    new ITerrainType[] { ground, water }, 
    new Dictionary<IMovementType, Dictionary<ITerrainType, int>>
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
    }
);
```

In this example, walking movement types spends two movement points to swim and one to walk, so his movement range in water is limited. In the  example above the vice versa also would be true for a swimming type.

### Cell State

Cells have a state. The state contains info about the cell coordinate, whether it's occupied or not, and its terrain type. In order to create a graph, you need to have a list of cell states. You can create it like this:

_Note: This example uses `Coordinate2D` to make it easier to understand what's going on, but it's recommended to use `Coordinate3D` istead. You can read more about coordinate systems in the [coordinate systems section](#coordinates)._
```c#
var graph = new Graph(new CellState[] { 
    new CellState(false, new Coordinate2D(0,0, OffsetTypes.OddRowsRight), ground),
    new CellState(false, new Coordinate2D(0,1, OffsetTypes.OddRowsRight), ground),
    new CellState(true, new Coordinate2D(1,0, OffsetTypes.OddRowsRight), water),
    new CellState(false, new Coordinate2D(1,1, OffsetTypes.OddRowsRight), water),
    new CellState(false, new Coordinate2D(1,2, OffsetTypes.OddRowsRight), ground)
}, movementTypes);
```
The resulting graph would look like this:
```
 ⬡⬡
⬡⬡⬡
```
The two cells on top would have terrain type "ground", the two cells in the second row would represent our first lake, and the first water cell would be not passable - we can imagine that there's a sharp rock in the lake at that spot.

### Coordinates

There are currently two coordinate systems implemented: offset coordinate type and 3D coordinate type. Offset coordinate type is easy to read for humans, as it's simple `x: rowNumber, y: columnNumber`. The Coordinate2D, however, requires some additional information to work properly - the offset type. It's also not easy to write algorithms for the `Coordinate2D`. The `Coordinate3D`, on the other hand, is not very human-readable, but very easy to operate with code. The library uses `Coordinate3D` internally everywhere, but it's easily possible to convert them into each other: 
```c#
new Coordinate2D(0,0, OffsetTypes.OddRowsRight).To3D();
new Coordinate3D(0,0,0).To2D(OffsetTypes.OddRowsRight);
```

Both structures also have static method to convert an enumerable:
```c#
Coordinate2D.To3D(new [] { 
    Coordinate2D(0,0, OffsetTypes.OddRowsRight),
    Coordinate2D(0,1, OffsetTypes.OddRowsRight)
});
Coordinate3D.To2D(new [] { 
    Coordinate3D(0,0,0),
    Coordinate3D(0,1,-1)
}, OffsetTypes.OddRowsRight)
```

For the `Coordinate3D`, it's also possible to perform addition and substraction of two coordinates and multiplication by a scalar:
```c#
var a = new Coordinate3D(-1,1,0);
var b = new Coordinate3D(0,1,-1);

var c = a + b; // c == -1, 2, -1
var d = c * 2; // d == -2, 4, -2
```

## Find the shortest path

After the graph was initialized, simply call `graph.GetShortestPath()` method on it. It accepts three parameters: coordinate from, coordinate to and your pawn's movement type. If there's a path from a to b, 

## Get movement range

- Block and unblock cells for movement
- Get block status
- Simple ranges withou terrain types
- IsInBounds
- Changing the cell's terrain type
- Resizing the graph and changing cell states
- GetAllCellsCoordinates() and GetAllCells()