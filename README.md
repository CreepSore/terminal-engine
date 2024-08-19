# Terminal-Engine
**A small not so game engine with a TTY console as the main renderer.**

This projects main goal is to freshen up my C# knowledge.
It's able to reach 1 million ticks/second in idle and around 70k TPS with a miner mining a world with the dimensions 120x30 (find the best target -> pathfinding -> walking -> mining -> repeat).

## Main components

The engine consists of several main interfaces:
  - [ITickable](Sim/Logic/ITickable.cs)
    - Defines a class as a tickable object
  - [IRenderable](Sim/Renderables/IRenderable.cs)
    - Defines a class as a renderable object
  - [IWithPosition](Sim/Logic/IWithPosition.cs)
    - Defines a class with a specified position contained inside a world
  - [IWithCapabilities](Sim/Capabilities/IWithCapabilities.cs)
    - Defines a class that can use capabilities.
    - Capabilities define a certain action that the object is able to do. 
  - [IObject](Sim/Objects/IObject.cs)
    - A non-ticking spawnable object with position
  - [IEntity](Sim/Entities/IEntity.cs)
    - A ticking spawnable object with position
  - [IState](Sim/Logic/State/IState.cs)
    - Defines a behavior for entities, used by the StateMachine.
  - [IPathFinder](Sim/Pathfinding/IPathFinder.cs)
    - Finds the best path from one position to another
    - A* is currently the only one implemented
  - [IRenderer](Sim/Rendering/IRenderer.cs)
    - Renders all IRenderable objects
    - TtyRenderer is currently the only implemented renderer.
      - Built using tasks, STDOUT eats a lot of processing power.
  - [IWorldPopulator](Sim/World/Populator/IWorldPopulator.cs)
    - Generates the terrain using objects.
    - TODO: Perlin noise

Also to name some of the most important classes:
  - [World](Sim/World/World.cs)
  - [BaseEntity](Sim/Entities/BaseEntity.cs)
  - [BaseObject](Sim/Objects/BaseObject.cs)
  - [BaseCapability](Sim/Capabilities/BaseCapability.cs)
  - [CostMatrix](Sim/Pathfinding/CostMatrix.cs) (for pathfinding)
  - [TtyRenderer](Sim/Rendering/TtyRenderer.cs)
  - [StateMachine](Sim/Logic/State/StateMachine.cs)
  - [Timer](Sim/Logic/Timer.cs)
