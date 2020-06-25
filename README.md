[logo]: ./icon48.png "HexCore logo"

# HexCore ![alt text][logo] 
[![NuGet Version and Downloads count](https://buildstats.info/nuget/HexCore)](https://www.nuget.org/packages/HexCore)

HexCore is a library to perform various operations with a hexagonal grid, such as finding shortest paths from one cell to another, managing terrains and movement types, maintaining the grid state, finding various ranges, neighbors, variuos coordinate systems and converters between them, and some more stuff you may want to do with a hex grid.

## Installation

The library can be installed from [NuGet](https://www.nuget.org/packages/HexCore). Run from the command line `dotnet add package HexCore` in your project or use your IDE of choice.

## Usage

For the main principles and detailed explanations please see [the docs](./Docs).

### Qucickstart

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

## Contributing

Everyone is welcome to contribute in any way of form! For the further details, please read [CONTRIBUTING.md](./CONTRIBUTING.md)

## Authors
 - [Anton Suprunchuk](https://github.com/antouhou) - [Website](https://antouhou.com)

See also the list of contributors who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE.md](./LICENSE.md) file for details

