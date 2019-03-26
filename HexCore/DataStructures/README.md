## Data structures

This folder contains various data structures used by the grid toolkit.

### Coordinate2D

A data structure representing coordinate on a grid with offset (columns/rows) system. As columns and rows have offset, 
offset needs to be specified when creating a coordinate. Can be converted to `Coordinate3D` using `.To3D()` instance 
method. A list of `Coordinate2D `s can be converted to a list of `Coordinate3D`s using static `.To3D(Coordinate3D[])` 
method.

### Coordinate3D

A data structure representing coordinate on a grid using a cubic coordinate system. All algorithms in this lib internally use 
`Coordinate3D`. It's harder to understand, but easier to write algorithms with. Can be converted to 2D using instance 
`.To2D(OffsetType)` and static `.To2D(Coordinate3D[], OffsetType)` methods.

`PriorityQueue` - Like an ordinary queue, but with item weights