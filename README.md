<h1 align="center">
  <a href="https://github.com/antouhou/HexCore">
    <img src="./HexCore.Website/wwwroot/img/logo.svg" alt="alt text" title="HexCore logo" style="max-width:400px;">
  </a>
  <br>HexCore <br>
</h1>

<p align="center">
  <a href='https://www.nuget.org/packages/HexCore' target='_blank'><img src='https://buildstats.info/nuget/HexCore' alt='Coverage Status' /></a>
  <a href='https://coveralls.io/github/antouhou/HexCore?branch=master' target='_blank'><img src='https://coveralls.io/repos/github/antouhou/HexCore/badge.svg?branch=master' alt='Coverage Status' /></a>
  <img src='https://github.com/antouhou/HexCore/workflows/Test%20and%20build/badge.svg' alt="Build status" />
  <a href="https://lgtm.com/projects/g/antouhou/HexCore/alerts/"><img alt="Total alerts" src="https://img.shields.io/lgtm/alerts/g/antouhou/HexCore.svg?logo=lgtm&logoWidth=18"/></a>
</p>

HexCore is a library to perform various operations with a hexagonal grid, such as finding shortest paths from one cell to another, managing terrains and movement types, maintaining the grid state, finding various ranges, neighbors, various coordinate systems and converters between them, and some more stuff you may want to do with a hex grid.
## Installation

The library can be installed from [NuGet](https://www.nuget.org/packages/HexCore). Run from the command line `dotnet add package HexCore` in your project or use your IDE of choice.

## Usage

For the detailed explanations please see [the docs](./Docs).

## Demo

For the interactive demo, please visit the [website](https://hexcore.antouhou.com)!

### Quickstart

```c#
using System.Collections.Generic;
using HexCore;

namespace HexCoreTests
{
    public class QuickStart
    {
        public static void Demo()
        {
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

            var pawnPosition = new Coordinate2D(0,0, OffsetTypes.OddRowsRight).To3D();
            // Mark pawn's position as occupied
            graph.BlockCells(pawnPosition);

            const int pawnMovementPoints = 3;

            var pawnMovementRange = graph.GetMovementRange(
                pawnPosition, pawnMovementPoints, walkingType
            );

            var pawnGoal = new Coordinate2D(1, 2, OffsetTypes.OddRowsRight).To3D();
            
            var theShortestPath = graph.GetShortestPath(
                pawnPosition, 
                pawnGoal,
                walkingType
            );
            // When moving pawn, unblock old position and block the new one.
            graph.UnblockCells(pawnPosition);
            pawnPosition = pawnGoal;
            graph.BlockCells(pawnGoal);
        }
    }
}
```
The resulting graph would look like this:
```
   ⬡⬡
  ⬡⬡⬡
```
The two cells on top would have terrain type "ground", the two cells in the second row would represent our first lake, and the first water cell would be not passable - we can imagine that there's a sharp rock in the lake at that spot.

For the detailed explanations please see [the docs](./Docs).

## Contributing

Everyone is welcome to contribute in any way of form! For the further details, please read [CONTRIBUTING.md](./CONTRIBUTING.md)

## Authors
 - [Anton Suprunchuk](https://github.com/antouhou) - [Website](https://antouhou.com)

See also the list of contributors who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE.md](./LICENSE.md) file for details

