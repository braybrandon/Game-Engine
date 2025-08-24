using GameEngine.Common.Interfaces;
using GameEngine.Gameplay.Scene;
using System.Runtime.CompilerServices;

namespace GameEngine.Core.Entities
{


    /// <summary>
    /// Represents a unique identifier for an object in the game world.
    /// It's a lightweight struct that acts as a handle to components stored in the World.
    /// </summary>
    public readonly struct Entity: IEntity
    {
        /// <summary>
        /// The unique integer ID of this entity.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// A reference to the World instance this entity belongs to.
        /// This allows convenience methods on Entity to delegate to the World's component management.
        /// </summary>
        public IWorld _world { get; }

        /// <summary>
        /// Initializes a new instance of the Entity struct.
        /// </summary>
        /// <param name="id">The unique ID for the entity.</param>
        /// <param name="world">The World instance this entity belongs to.</param>
        public Entity(int id, World world)
        {
            Id = id;
            _world = world;
        }

        /// <summary>
        /// Adds a new component to this entity in its associated World.
        /// If a component of the same type already exists, it will be replaced.
        /// </summary>
        /// <typeparam name="T">The type of the component (must be a struct implementing IComponent).</typeparam>
        /// <param name="component">The component instance to add.</param>
        /// <returns>A reference to the added component in the World's storage.</returns>
        public ref T AddComponent<T>(T component = default) where T : struct, IComponent
        {
            return ref _world.AddComponent<T>(Id, component);
        }

        /// <summary>
        /// Retrieves a component of a specific type from this entity in its associated World.
        /// </summary>
        /// <typeparam name="T">The type of the component to retrieve (must be a struct implementing IComponent).</typeparam>
        /// <returns>A reference to the component in the World's storage.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the component is not found for this entity.</exception>
        public ref T GetComponent<T>() where T : struct, IComponent
        {
            return ref _world.GetComponent<T>(Id);
        }

        /// <summary>
        /// Checks if this entity has a component of a specific type in its associated World.
        /// </summary>
        /// <typeparam name="T">The type of the component to check for (must be a struct implementing IComponent).</typeparam>
        /// <returns>True if the entity has the component, false otherwise.</returns>
        public bool HasComponent<T>() where T : struct, IComponent
        {
            return _world.HasComponent<T>(Id);
        }

        /// <summary>
        /// Removes a component of a specific type from this entity in its associated World.
        /// </summary>
        /// <typeparam name="T">The type of the component to remove (must be a struct implementing IComponent).</typeparam>
        public void RemoveComponent<T>() where T : struct, IComponent
        {
            _world.RemoveComponent<T>(Id);
        }

        /// <summary>
        /// Provides a string representation of the Entity.
        /// </summary>
        public override string ToString() => $"Entity({Id})";

        /// <summary>
        /// Determines whether this Entity is equal to another object.
        /// Entities are considered equal if they have the same ID and belong to the same World instance.
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj is Entity other)
            {
                // Crucially, check if they belong to the same World instance
                // ReferenceEquals is used for the World object itself
                return Id == other.Id && ReferenceEquals(_world, other._world);
            }
            return false;
        }

        /// <summary>
        /// Returns the hash code for this Entity.
        /// Consistent with Equals, it combines the hash codes of the ID and the World instance.
        /// </summary>
        public override int GetHashCode()
        {
            // RuntimeHelpers.GetHashCode(_world) provides a stable hash code for the object reference.
            return HashCode.Combine(Id, RuntimeHelpers.GetHashCode(_world));
        }

        /// <summary>
        /// Overloads the equality operator (==) for Entity structs.
        /// </summary>
        public static bool operator ==(Entity left, Entity right) => left.Equals(right);

        /// <summary>
        /// Overloads the inequality operator (!=) for Entity structs.
        /// </summary>
        public static bool operator !=(Entity left, Entity right) => !(left == right);
    }
}
