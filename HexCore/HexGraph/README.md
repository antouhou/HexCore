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
    var simpleGraph = GraphFactory.createSquareGraph(height: 2, width: 3);
```

This will create graph that looks like this:
```
   ⬡⬡⬡
  ⬡⬡⬡
```
This is square graph with odd rows placed right. `GraphFactory` can do more, check [its documentation](./GRAPH_FACTORY_README.md).


### Advanced usage

If `GraphFactory` isn't enough for you, you can create your own graphs without using it.
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
