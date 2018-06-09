## BattleCore.Unit

### Sections:

- Working with units
- Basic unit tutorial

Units are the essential elements of any battle. They performing such actions as move, attack, cast a spell. Also, they keep track of its internal state - characteristics, HP, MP, and so on.

### Working with units

A unit consists of two parts: data and behavior.
`UnitState` class handles the data storage, `UnitBehavior` class describes unit's behavior.

There are some base requirements for units:

1. Unit needs access to an instance of the `Graph` class. Without it, the unit won't be able to calculate ranges and perform movements.

2. Unit behavior should be a subclass of the `AbstractUnitBehaviour` class. That way all unit behaviors will have a consistent set of methods.

3. Unit behavior requires a place to store results of its calculations. So each behavior instance needs its own `UnitState` instance.

4. Unit needs to know its movement type. `MovementType` class describes movement types. It's located in the `HexCore.Graph` namespace.

> There is a class that implements `AbstractUnitBehaviour`, `UnitBehavior`. This class covers most of the use-cases required by a basic game. But for more advanced games you'll want to create your own unit behaviors. "Advanced Usage" section covers this topic.

### Basic unit tutorial

To create a simple unit you need to import `Graph` class from the `HexCore` namespace.

```c#
using HexCore.HexGraph;
```

Then we need to create a basic game class. Remember that we need `Graph` instance to work with units, so let's create it too. Also, we need to create at least one basic movement type for the unit. Let's call it `GroundMovementType`

```c#
public class Game {
    private readonly Graph _graph = new Graph(4, 4);
}
```