# HexCore Documentation

Welcome to the HexCore documentation! HexCore is a relatively simple library
to perform various operation with a hexagonal grids.

- [Introduction](#core-principles)
- Create a graph
- Get the pawn's movement range
- Find the shortest path
- Other types of ranges
- Coordinate systems

## Core principles

The base class is `HexCore.Graph`. On order to work, graph needs some 
data: Cell states and movement types. Cell state keeps all the info 
about a particular cell: its coordinate, whether its occupied or not, 
and it's movement type (think of it as a terrain type for the cell).
Movement types are needed to calculate movement range of a pawn on
a particular terrain (for example, pawn with the movement type "ground"
should receive a penalty when swimming through "water").

Graph operates on 3D coordinates because they are easier to create
algorithms for, but if you prefer to use coordinates in the form of
(x: row, y: column), you can check [coordinates section](#coordinates) below.

### Understanding movement types

As explained above, movement types are essential for the graph.
There's a helper class to help you maintain your terrain and movement
types. It's called `MovementTypes`. It maintains a table of your
movement and terrain types. It's mandatory to specify movement types
for a graph. Movement types creation looks like this:

*If you want to have just the same constant movement cost across the whole map,
 create only one terrain and movement type with a movement cost of 1. 
 One terrain type with a movement cost of 1 is not a default option to ensure that 
 this is a deliberate choice and not a mistake.*

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

In this example, ground walking movement types spends two movement points
to swim and one to walk, so his movement range in water is limited. In the 
example above the same also would be true for a swimming type.

### CellState (block and unblock cells)

Cells have a state. The state contains info about the cell coordinate, 
whether it's occupied or not, and its terrain type.

### Coordinates

## Examples

For more usage examples, you can refer to the tests:

- 