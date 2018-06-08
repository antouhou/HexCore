# HexCore

HexCore is a library for building hexagonal-grid-based games. It consists of two parts:

The first part is HexCore library that does the hexagonal-grid math:
- A* pathfinding;
- Finding neighbors;
- Finding ranges;
- Finding ranges with penalties, that can be used to simulate different terrain types;
- Coordinate systems converter

And so on.

The second part is BattleCore library that implements a battlefield:
- Units: 
  - Movement;
  - Attack