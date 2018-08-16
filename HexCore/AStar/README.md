## A*

Most of the time you don't need to use this class directly; Instead, you can use `FindShortestPath` 
method of `HexGraph` instance. `AStarSearch` class is used by this method internally.

A* one of the most common pathfing algorythms.
In order to work requires implementation of `IWeightedGraph` interface.

`IWeightedInterface` should be able to provide information about neighbors of a givent point and movement cost to that point.
