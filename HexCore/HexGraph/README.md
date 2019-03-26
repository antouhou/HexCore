## HexCore.HexGraph

This directory contains a set of tools to interact with a hexagonal-based grid.

The main class that you'll be interested in is `Graph`.

### Basic usage

`Graph` is used for most of the operations with a hexagonal grid, such as:
* Finding neighboring cells;
* Finding ranges;
* Pathfinding

There is a factory class, `GraphFactory`, that will produce graphs for you. You need to specify graph size:

```c#
    var simpleGraph = GraphFactory.createSquareGraph(height: 3, width: 4);
```

Doing so will create a graph that looks like this:
```
   ⬡⬡⬡⬡
  ⬡⬡⬡⬡
   ⬡⬡⬡⬡
```
This is a square graph with odd rows placed right. `GraphFactory` can do more, check [its documentation](./GRAPH_FACTORY_README.md).

### Pathfinding and ranges

`Graph` class hash everything you can need for finding paths and ranges.

To get the shortest path from A to B, call `graph.getShortestPath(coordinateA, coordinateB, movementType);`. If you
don't have any movement types, you can use default ones. A movement type is needed for applying movement penalties.
Imagine a situation, when your unit has far lower movement range in the water than on the ground. In this case, the shortest path would include as few water cells as possible.

To get range, call `graph.getRange(center, radius)`

To get movement range for a unit, call `graph.GetMovementRange(center, radius, movementType)`. This method will apply
movement penalties for different movement types.

### Advanced usage

If `GraphFactory` isn't enough for your needs, you can create your graphs without using it.
All you need to create a graph is a list of 
`CellState` instances. `CellState` instances keep track of internal cell state and consist of
three fields and one method. These are: 
- Cell coordinate in the grid;
- Is cell passable or not;
- Movement type. Pathfinding and movement range algorithms use movement type.

To create a hex map, you need to define:
1. Movement types. There is a class for this, `MovementType`.
2. Create a list of cell states. 