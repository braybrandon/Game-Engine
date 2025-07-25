# Game Engine Architecture Overview

This document outlines the architecture for your ECS-based MonoGame engine, covering the core game loop, service management, entity-component-system (ECS) implementation, and how to approach "no-code" (data-driven) game creation via visual editors.

---

## 1. Core Engine & Game Loop Hierarchy

This diagram illustrates the flow of control and updates during your game's runtime.

[1. MonoGame Main Game Loop] - MyGame.cs (Your class inheriting from Microsoft.Xna.Framework.Game)
├── MyGame.Run() (Called by MonoGame Framework, runs continuously)
│   ├──> MyGame.Update(gameTime)  <-- Your game's per-frame logic update
│   │      │
│   │      ├── 1. Global Service Updates (Order is CRITICAL here for fresh data)
│   │      │   ├──> ServiceLocator.Get

│          │                        └── (Internal: Renderer.EndSpriteBatch() or similar global render cleanup)
│          │
│          └── MyGame.base.Draw(gameTime) (MonoGame's internal drawing)
│
└── (Continuous loop until game exits)

[2. Static Global Access] - ServiceLocator.cs
├── Register

[3. ECS Data Storage] - ComponentManager.cs (Likely a Service itself, accessed via ServiceLocator)
├── CreateEntity()
├── AddComponent(entityId, component)
├── GetComponent

---

## 2. Engine Features and Potential Expansions

This section details the core engine features you have in place and outlines common expansions, particularly focusing on the "no-code" data-driven approach.

### 2.1 Core Engine Foundation (Currently Covered)

* **Game Loop Management:** The `MyGame` class orchestrates the main update/draw cycle, delegating to core services and the `GameStateManager`. Your custom `GameLoop` class within each scene effectively manages the execution of ECS systems, including a fixed-time step.
* **Time Management:** An `ITimeManager` service provides consistent time information (`deltaTime`, `fixedDeltaTime`, `totalTime`) to all systems and services.
* **Input Handling:** An `IInputService` polls raw input states once per frame, providing a clean API for systems to query input without direct MonoGame dependencies.
* **ECS Core:**
    * **Components:** Pure data structures defining entity attributes (e.g., `TransformComponent`, `VelocityComponent`, `SpriteComponent`).
    * **Entities:** Simple IDs that group components.
    * **Systems (`EngineSystem`):** Logic units that iterate over entities with specific component combinations to perform game mechanics (e.g., `PlayerInputSystem`, `MovementSystem`, `SpriteRenderingSystem`).
    * **`ComponentManager`:** The central registry for entities and their components, providing efficient retrieval by entity ID or component type.
* **Rendering (Basic):** An `IRenderer` service abstracts graphics device operations, and rendering systems (`SpriteRenderingSystem`) draw components to the screen.
* **Resource Loading:** An `IResourceManager` service handles loading assets (textures, sounds, data files) and provides them to other parts of the engine.
* **Game State Management:** An `IGameStateManager` service transitions between `IScene` implementations, each representing a distinct game state (e.g., Main Menu, Level 1).
* **Dependency Management:** A `ServiceLocator` (static class) provides a simple way for systems and other services to access shared, globally unique service instances.

### 2.2 "No Code" Game Creation (Data-Driven Editor Focus)

This approach leverages your ECS to allow content creators to build games visually, primarily by configuring data rather than writing new code for every new element. This requires building **separate editor applications** that output structured data files read by your runtime engine.

1.  **Core Principle: Everything is Data:**
    * Game elements like entities, levels, UI, animations, and game settings are defined in structured data formats (e.g., **JSON** is highly recommended for its human-readability and widespread library support).
    * Your runtime engine's `IResourceManager` and specific systems/factories are responsible for *loading and interpreting* this data.

2.  **Visual Data Editors:** These would be separate applications (perhaps built with MonoGame.Forms, Avalonia, or ImGui.NET for their UI capabilities) that allow users to visually define game content.

    * **Tilemap Editor:**
        * **Purpose:** Design game levels using tiles.
        * **Functionality:**
            * Import sprite sheets for tile sets.
            * Visually divide sprite sheets into individual tile definitions (`SourceRectangle`).
            * Define tile properties: `IsPassable`, `IsDangerous`, `HasLoot`, `CollisionLayer`.
            * Brush, fill, and erase tools to place tiles on layers.
            * Save/Load `TileMap` data (e.g., `level1.json`) for runtime consumption.
        * **Runtime Integration:**
            * `TileMapComponent`: Holds loaded `TileMap` data.
            * `TileMapRenderingSystem`: Draws tile layers.
            * `TileCollisionSystem`: Handles entity-tile interactions based on tile properties.

    * **Entity Visual Toolset / Prefab Editor:**
        * **Purpose:** Define reusable entity blueprints (prefabs) and their visual appearance/initial components.
        * **Functionality:**
            * Visually select a sprite sheet for an entity's base sprite.
            * Define sprite frames and animation sequences (`SourceRectangle`, `Duration`).
            * Visually attach and configure **components** to an entity blueprint (e.g., add `TransformComponent` and set initial position, add `HealthComponent` and set `MaxHealth`). This is key for "no code" configuration.
            * Optionally, assign tags/types (`IsEnemyTagComponent`, `IsNPCTagComponent`) that systems can filter by.
            * Save/Load `EntityBlueprint` data (e.g., `goblin_prefab.json`).
        * **Runtime Integration:**
            * `IEntityFactory` (Service): Gains a method like `CreateEntityFromBlueprint(string blueprintPath, Vector2 initialPosition)`. This factory reads the blueprint data and uses the `ComponentManager` to create the entity with all its defined components.

    * **Scene Editor:**
        * **Purpose:** Visually compose a game scene by placing instances of tilemaps and entity prefabs.
        * **Functionality:**
            * Load existing tilemaps.
            * Drag-and-drop entity prefabs into the scene.
            * Modify instance-specific properties (e.g., position, unique ID).
            * Save/Load `SceneData` (e.g., `level_instance_1.json`) which lists loaded tilemaps and placed entity prefab instances.

### 2.3 Other Common Engine Features (Build on Your Foundation)

These systems and services build *on top* of the core ECS and service architecture you've established.

1.  **Physics Engine/System:**
    * **Capabilities:** Full collision detection (broad-phase, narrow-phase), collision resolution (response, impulses), and simulating forces (gravity, friction, drag).
    * **Components:** `ColliderComponent` (defines collision shape), `RigidBodyComponent` (mass, inertia, type: dynamic/static/kinematic), `PhysicsMaterialComponent` (friction, restitution).
    * **Integration:** Often a dedicated `PhysicsSystem` that processes entities with `RigidBodyComponent` and `ColliderComponent`, modifying `TransformComponent` and `VelocityComponent`.

2.  **Audio System:**
    * **`IAudioService`:** Manages playback of sound effects and music, volume control, spatial audio, and sound instance pooling.
    * **Components:** `AudioSourceComponent` (to attach sounds to entities).
    * **Systems:** `AudioPlaybackSystem` (plays sounds based on `AudioSourceComponent` state).

3.  **User Interface (UI) System:**
    * **Capabilities:** Rendering UI elements (buttons, text boxes, sliders, panels), handling UI-specific input, layout management, and eventing.
    * **Components:** `UIElementComponent`, `ButtonComponent`, `TextComponent`.
    * **Systems:** `UISystem` (processes UI input, updates UI state, renders UI components). Often complex enough to be a separate framework or sub-engine.

4.  **Animation System:**
    * **Capabilities:** Manages playback of sprite sheet animations, potentially skeletal animations for 3D. Includes animation state machines for smooth transitions.
    * **Components:** `AnimationComponent` (current animation, frame, speed, state), `SpriteSheetComponent`.
    * **Systems:** `AnimationSystem` (updates animation frames based on time, sets `SourceRectangle` for `SpriteComponent`).

5.  **Particle System:**
    * **Capabilities:** Generating and simulating various particle effects (fire, smoke, explosions, rain).
    * **Components:** `ParticleEmitterComponent` (defines particle properties, spawn rate), `ParticleComponent` (individual particle state).
    * **Systems:** `ParticleEmissionSystem` (spawns particles), `ParticleUpdateSystem` (moves/updates particles), `ParticleRenderSystem` (draws particles).

6.  **Event/Messaging System:**
    * **`IEventBus` / `IMessageBus` (Service):** Provides a publish-subscribe mechanism for loose coupling. Systems and services can communicate without direct references, reacting to game events (e.g., "PlayerDiedEvent", "ItemCollectedEvent", "CollisionEvent").

7.  **Serialization/Saving & Loading Game State:**
    * **`ISaveLoadService`:** Manages persisting and restoring the game's state (current scene, entity components, player progress) to/from disk. Relies heavily on robust data serialization (e.g., using JSON.NET or `System.Text.Json`).

---

This comprehensive setup provides you with a powerful ECS-driven engine, where the "no-code" aspect is achieved by making your game highly data-driven and building intuitive visual editors to manipulate that data.