## HexCore.HexGraph

This is a set of tools for interacting with hexagonal based grid.

The main class that you'll be interested in is `Graph`.

### Basic usage

`Graph` is used for most of operations with hexagonal grid, such as:
* Finding neighboring cells;
* Finding ranges;
* Pathfinding

There is a factory class, `GraphFactory`, that will produce graphs for you. You just need to specify graph size:

```c#
    var simpleGraph = GraphFactory.createSquareGraph(height: 3, width: 4);
```

This will create graph that looks like this:
```
   ⬡⬡⬡⬡
  ⬡⬡⬡⬡
   ⬡⬡⬡⬡
```
This is a square graph with odd rows placed right. `GraphFactory` can do more, check [its documentation](./GRAPH_FACTORY_README.md).

### Pathfinding and ranges

`Graph` class hash everything you can need for finding paths and ranges.

To get shortest path from A to B, simply call `graph.getShortestPath(coordinateA, coordinateB, movementType);`. If you
don't have any movement types, you can use default ones. A movement type is needed for applying movement penalties.
Imagine a situation, when your unit has far lower movement range in the water than on the ground. In this case, the shortest 
path would include as few water cells as possible.

To get range, call `graph.getRange(center, radius)`

To get movement range for a unit, call `graph.GetMovementRange(center, radius, movementType)`. This metthod will apply
movement penalties for different movement types.

### Advanced usage

If `GraphFactory` isn't enough for your needs, you can create your own graphs without using it.
All you need to create a graph is a list of 
`CellState` instances. `CellState` instances keep track of internal cell state, and consist of
three fields and one method. These are: 
- Cell coordinate in the grid;
- Is cell passable or not;
- Movement type. Movement type is used by pathfinding and movement range algorithms.

So, in order to create hex map you need to define:
1. Movement types. There is a class for this, `MovementType`.
2. Create a list of cell states. 

So, minimal viable graph creation would look like this:
