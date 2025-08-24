namespace GameEngine.Common.Interfaces
{
    public interface IEntity
    {
        /// <summary>
        /// The unique integer ID of this entity.
        /// </summary>
        public int Id { get; }

        public IWorld _world { get; }

        /// <summary>
        /// Adds a new component to this entity in its associated World.
        /// If a component of the same type already exists, it will be replaced.
        /// </summary>
        /// <typeparam name="T">The type of the component (must be a struct implementing IComponent).</typeparam>
        /// <param name="component">The component instance to add.</param>
        /// <returns>A reference to the added component in the World's storage.</returns>
        public ref T AddComponent<T>(T component = default) where T : struct, IComponent;

        /// <summary>
        /// Retrieves a component of a specific type from this entity in its associated World.
        /// </summary>
        /// <typeparam name="T">The type of the component to retrieve (must be a struct implementing IComponent).</typeparam>
        /// <returns>A reference to the component in the World's storage.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the component is not found for this entity.</exception>
        public ref T GetComponent<T>() where T : struct, IComponent;

        /// <summary>
        /// Checks if this entity has a component of a specific type in its associated World.
        /// </summary>
        /// <typeparam name="T">The type of the component to check for (must be a struct implementing IComponent).</typeparam>
        /// <returns>True if the entity has the component, false otherwise.</returns>
        public bool HasComponent<T>() where T : struct, IComponent;

        /// <summary>
        /// Removes a component of a specific type from this entity in its associated World.
        /// </summary>
        /// <typeparam name="T">The type of the component to remove (must be a struct implementing IComponent).</typeparam>
        public void RemoveComponent<T>() where T : struct, IComponent;

        /// <summary>
        /// Provides a string representation of the Entity.
        /// </summary>
        public string ToString();

        /// <summary>
        /// Determines whether this Entity is equal to another object.
        /// Entities are considered equal if they have the same ID and belong to the same World instance.
        /// </summary>
        public bool Equals(object? obj);

        /// <summary>
        /// Returns the hash code for this Entity.
        /// Consistent with Equals, it combines the hash codes of the ID and the World instance.
        /// </summary>
         public int GetHashCode();
    }
}
