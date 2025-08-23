
using Common.Interfaces;
using GameEngine.Core.Entities;

namespace GameEngine.Core.Components
{

    /// <summary>
    /// A generic pool for storing components of a specific type (T).
    /// It uses an array for efficient storage and provides direct 'ref' access.
    /// Implements a swap-remove strategy for efficient deletion.
    /// </summary>
    /// <typeparam name="T">The type of component this pool manages (must be a struct implementing IComponent).</typeparam>
    internal class ComponentPool<T> : IComponentPool where T : struct, IComponent
    {
        private T[] _componentsArray; // The actual array where component structs are stored
        private Dictionary<int, int> _entityIdToIndex; // Maps Entity ID to its index in _componentsArray
        private Dictionary<int, int> _indexToEntityId; // Maps Array Index back to Entity ID (for swap-remove)
        private int _count; // Current number of active components in the pool

        private const int DefaultCapacity = 256; // Initial capacity for the component array

        /// <summary>
        /// Initializes a new instance of the ComponentPool for type T.
        /// </summary>
        public ComponentPool()
        {
            _componentsArray = new T[DefaultCapacity];
            _entityIdToIndex = new Dictionary<int, int>();
            _indexToEntityId = new Dictionary<int, int>();
            _count = 0;
        }

        /// <summary>
        /// Gets a reference to the component for the specified entity ID.
        /// </summary>
        /// <param name="entityId">The ID of the entity.</param>
        /// <returns>A reference to the component.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the component is not found for the entity.</exception>
        public ref T Get(int entityId)
        {
            if (_entityIdToIndex.TryGetValue(entityId, out int index))
            {
                return ref _componentsArray[index]; // Direct reference to the struct in the array
            }
            throw new KeyNotFoundException($"Component of type {typeof(T).Name} not found for entity {entityId}");
        }

        /// <summary>
        /// Adds a new component for the specified entity ID, or replaces an existing one.
        /// </summary>
        /// <param name="entityId">The ID of the entity.</param>
        /// <param name="component">The component instance to add/set.</param>
        /// <returns>A reference to the component in the pool.</returns>
        public ref T Add(int entityId, T component)
        {
            if (_entityIdToIndex.TryGetValue(entityId, out int index))
            {
                // Component already exists, just update it
                _componentsArray[index] = component;
                return ref _componentsArray[index];
            }
            else
            {
                // Component does not exist, add new
                if (_count == _componentsArray.Length)
                {
                    // Resize array if capacity is reached
                    ResizeArray(_componentsArray.Length * 2);
                }

                int newIndex = _count;
                _componentsArray[newIndex] = component;

                _entityIdToIndex[entityId] = newIndex;
                _indexToEntityId[newIndex] = entityId;

                _count++;
                return ref _componentsArray[newIndex];
            }
        }

        /// <summary>
        /// Removes the component associated with the given entity ID from this pool.
        /// Uses a swap-remove technique to maintain dense array packing.
        /// </summary>
        /// <param name="entityId">The ID of the entity whose component to remove.</param>
        public void Remove(int entityId)
        {
            if (_entityIdToIndex.TryGetValue(entityId, out int indexToRemove))
            {
                _entityIdToIndex.Remove(entityId);
                _indexToEntityId.Remove(indexToRemove);

                _count--; // Decrement the count of active components

                // If the removed component was not the last one, move the last component
                // into the now-empty slot to keep the array packed (dense).
                if (indexToRemove < _count)
                {
                    int lastComponentEntityId = _indexToEntityId[_count]; // Get entity ID of the last component
                    _componentsArray[indexToRemove] = _componentsArray[_count]; // Move last component to removed slot

                    _entityIdToIndex[lastComponentEntityId] = indexToRemove; // Update index mapping for moved component
                    _indexToEntityId[indexToRemove] = lastComponentEntityId; // Update index mapping for moved component
                }
                // Clear the last element (optional for structs, more important for reference types)
                _componentsArray[_count] = default(T);
            }
        }

        /// <summary>
        /// Checks if this pool contains a component for the given entity ID.
        /// </summary>
        /// <param name="entityId">The ID of the entity to check.</param>
        /// <returns>True if a component exists for the entity, false otherwise.</returns>
        public bool Has(int entityId)
        {
            return _entityIdToIndex.ContainsKey(entityId);
        }

        /// <summary>
        /// Gets an enumerable collection of entity IDs that have a component in this pool.
        /// </summary>
        /// <returns>An enumerable of entity IDs.</returns>
        public IEnumerable<int> GetEntityIds()
        {
            // Only iterate over the active components
            for (int i = 0; i < _count; i++)
            {
                yield return _indexToEntityId[i];
            }
        }

        /// <summary>
        /// Resizes the internal component array to a new capacity.
        /// </summary>
        /// <param name="newCapacity">The new capacity for the array.</param>
        private void ResizeArray(int newCapacity)
        {
            T[] newArray = new T[newCapacity];
            Array.Copy(_componentsArray, newArray, _count); // Copy only active components
            _componentsArray = newArray;
        }
    }

    /// <summary>
    /// The central data store for the Entity-Component-System.
    /// It manages the creation and destruction of entities, and the storage and retrieval of their components
    /// using specialized component pools for performance.
    /// </summary>
    public class World : IWorld
    {
        // --- Entity Management Fields ---
        private int _nextEntityId = 0; // Counter for generating unique entity IDs
        private readonly HashSet<int> _activeEntityIds = new HashSet<int>(); // Stores IDs of currently active entities
        private readonly HashSet<int> _entitiesToDestroy = new HashSet<int>();

        // --- Component Storage Field (Now using Component Pools) ---
        // Outer Dictionary maps Component Type -> IComponentPool instance for that type
        private readonly Dictionary<Type, IComponentPool> _componentPools = new Dictionary<Type, IComponentPool>();

        /// <summary>
        /// Initializes a new instance of the World class.
        /// </summary>
        public World()
        {
            // Any initial setup for the world can go here.
        }

        // --- Entity Management Methods ---

        /// <summary>
        /// Creates a new unique entity in this World.
        /// </summary>
        /// <returns>The newly created Entity struct, which acts as a handle.</returns>
        public IEntity CreateEntity()
        {
            int entityId = _nextEntityId++;
            _activeEntityIds.Add(entityId); // Add the new entity's ID to the active set
            return new Entity(entityId, this); // Create and return the Entity struct, linking it to this World instance
        }

        public void DestroyEntity(IEntity entity)
        {
            if (!_activeEntityIds.Contains(entity.Id))
            {
                return;
            }

            _activeEntityIds.Remove(entity.Id);
            _entitiesToDestroy.Add(entity.Id);
        }

        public void RemoveInactiveEntities()
        {
            foreach (var entityId in _entitiesToDestroy)
            {
                foreach (var componentType in _componentPools.Keys.ToList())
                {
                    if (_componentPools.TryGetValue(componentType, out var pool))
                    {
                        if (pool.Has(entityId))
                        {
                            pool.Remove(entityId);
                        }
                    }
                }
            }

            _entitiesToDestroy.Clear(); // Clear the list for the next frame
        }

        /// <summary>
        /// Checks if a given entity ID currently represents an active entity in this World.
        /// </summary>
        /// <param name="entityId">The ID of the entity to check.</param>
        /// <returns>True if the entity is active, false otherwise.</returns>
        public bool IsEntityActive(int entityId)
        {
            return _activeEntityIds.Contains(entityId);
        }

        // --- Component Management Methods ---

        /// <summary>
        /// Adds a new component to the specified entity in this World, or replaces an existing one.
        /// </summary>
        /// <typeparam name="T">The type of the component (must be a struct implementing IComponent).</typeparam>
        /// <param name="entityId">The ID of the entity to add the component to.</param>
        /// <param name="component">The component instance to add.</param>
        /// <returns>A reference to the added/updated component in the World's storage.</returns>
        /// <exception cref="InvalidOperationException">Thrown if attempting to add a component to an inactive entity.</exception>
        public ref T AddComponent<T>(int entityId, T component) where T : struct, IComponent
        {
            if (!_activeEntityIds.Contains(entityId))
            {
                throw new InvalidOperationException($"Cannot add component to inactive entity {entityId}.");
            }

            // Get or create the ComponentPool for this type
            if (!_componentPools.TryGetValue(typeof(T), out var pool))
            {
                pool = new ComponentPool<T>();
                _componentPools[typeof(T)] = pool;
            }

            // Add/Update the component in its specific pool and return a direct reference
            return ref ((ComponentPool<T>)pool).Add(entityId, component);
        }

        /// <summary>
        /// Retrieves a component of a specific type from the specified entity in this World.
        /// </summary>
        /// <typeparam name="T">The type of the component to retrieve (must be a struct implementing IComponent).</typeparam>
        /// <param name="entityId">The ID of the entity to get the component from.</param>
        /// <returns>A reference to the component in the World's storage.</returns>
        /// <exception cref="InvalidOperationException">Thrown if attempting to get a component from an inactive entity.</exception>
        /// <exception cref="KeyNotFoundException">Thrown if the component is not found for the entity.</exception>
        public ref T GetComponent<T>(int entityId) where T : struct, IComponent
        {
            if (!_activeEntityIds.Contains(entityId))
            {
                throw new InvalidOperationException($"Cannot get component from inactive entity {entityId}.");
            }

            // Try to get the ComponentPool for this type
            if (_componentPools.TryGetValue(typeof(T), out var pool))
            {
                // Get a direct reference from the pool
                return ref ((ComponentPool<T>)pool).Get(entityId);
            }
            throw new KeyNotFoundException($"Component of type {typeof(T).Name} not found for entity {entityId}");
        }

        /// <summary>
        /// Checks if the specified entity has a component of a specific type in this World.
        /// </summary>
        /// <typeparam name="T">The type of the component to check for (must be a struct implementing IComponent).</typeparam>
        /// <param name="entityId">The ID of the entity to check.</param>
        /// <returns>True if the entity has the component, false otherwise.</returns>
        public bool HasComponent<T>(int entityId) where T : struct, IComponent
        {
            // An inactive entity is considered not to have components in a meaningful way
            if (!_activeEntityIds.Contains(entityId)) return false;
            return _componentPools.TryGetValue(typeof(T), out var pool) && pool.Has(entityId);
        }

        /// <summary>
        /// Removes a component of a specific type from the specified entity in this World.
        /// </summary>
        /// <typeparam name="T">The type of the component to remove (must be a struct implementing IComponent).</typeparam>
        /// <param name="entityId">The ID of the entity to remove the component from.</param>
        public void RemoveComponent<T>(int entityId) where T : struct, IComponent
        {
            // No need to remove from an inactive entity
            if (!_activeEntityIds.Contains(entityId)) return;

            if (_componentPools.TryGetValue(typeof(T), out var pool))
            {
                pool.Remove(entityId);
                // Optional: Clean up the pool itself if it becomes empty
                // (This can be more complex if entities are frequently added/removed)
            }
        }


        // --- Entity Querying Methods ---
        // These methods allow systems to efficiently find entities that possess specific component combinations.

        /// <summary>
        /// Gets all active entities that have a component of type T1.
        /// </summary>
        /// <typeparam name="T1">The type of the component to filter by.</typeparam>
        /// <returns>An enumerable collection of matching Entity structs.</returns>
        public IEnumerable<IEntity> GetEntitiesWith<T1>() where T1 : struct, IComponent
        {
            // Try to get the component pool for T1
            if (_componentPools.TryGetValue(typeof(T1), out var pool1))
            {
                // Iterate through the entity IDs that have T1 in this pool
                foreach (var entityId in pool1.GetEntityIds())
                {
                    // Only yield entities that are currently active in the world
                    if (_activeEntityIds.Contains(entityId))
                    {
                        yield return new Entity(entityId, this); // Create and yield a new Entity handle
                    }
                }
            }
        }

        /// <summary>
        /// Gets all active entities that have components of both type T1 and T2.
        /// </summary>
        /// <typeparam name="T1">The first component type.</typeparam>
        /// <typeparam name="T2">The second component type.</typeparam>
        /// <returns>An enumerable collection of matching Entity structs.</returns>
        public IEnumerable<IEntity> GetEntitiesWith<T1, T2>() where T1 : struct, IComponent where T2 : struct, IComponent
        {
            // Try to get component pools for both types
            if (_componentPools.TryGetValue(typeof(T1), out var pool1) &&
                _componentPools.TryGetValue(typeof(T2), out var pool2))
            {
                // Find common entity IDs that exist in both component sets
                foreach (var entityId in pool1.GetEntityIds().Intersect(pool2.GetEntityIds()))
                {
                    // Only yield entities that are currently active in the world
                    if (_activeEntityIds.Contains(entityId))
                    {
                        yield return new Entity(entityId, this);
                    }
                }
            }
        }

        /// <summary>
        /// Gets all active entities that have components of type T1, T2, and T3.
        /// </summary>
        /// <typeparam name="T1">The first component type.</typeparam>
        /// <typeparam name="T2">The second component type.</typeparam>
        /// <typeparam name="T3">The third component type.</typeparam>
        /// <returns>An enumerable collection of matching Entity structs.</returns>
        public IEnumerable<IEntity> GetEntitiesWith<T1, T2, T3>() where T1 : struct, IComponent where T2 : struct, IComponent where T3 : struct, IComponent
        {
            // Try to get component pools for all three types
            if (_componentPools.TryGetValue(typeof(T1), out var pool1) &&
                _componentPools.TryGetValue(typeof(T2), out var pool2) &&
                _componentPools.TryGetValue(typeof(T3), out var pool3))
            {
                // Find common entity IDs across all three component sets
                foreach (var entityId in pool1.GetEntityIds().Intersect(pool2.GetEntityIds()).Intersect(pool3.GetEntityIds()))
                {
                    // Only yield entities that are currently active in the world
                    if (_activeEntityIds.Contains(entityId))
                    {
                        yield return new Entity(entityId, this);
                    }
                }
            }
        }

        /// <summary>
        /// Gets all active entities that have components of type T1, T2, and T3, T4.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <returns></returns>
        public IEnumerable<IEntity> GetEntitiesWith<T1, T2, T3, T4>() where T1 : struct, IComponent where T2 : struct, IComponent where T3 : struct, IComponent where T4 : struct, IComponent
        {
            // Try to get component pools for all four types
            if (_componentPools.TryGetValue(typeof(T1), out var pool1) &&
                _componentPools.TryGetValue(typeof(T2), out var pool2) &&
                _componentPools.TryGetValue(typeof(T3), out var pool3) &&
                _componentPools.TryGetValue(typeof(T4), out var pool4))
            {
                // Find common entity IDs across all four component sets
                foreach (var entityId in pool1.GetEntityIds()
                    .Intersect(pool2.GetEntityIds())
                    .Intersect(pool3.GetEntityIds())
                    .Intersect(pool4.GetEntityIds()))
                {
                    // Only yield entities that are currently active in the world
                    if (_activeEntityIds.Contains(entityId))
                    {
                        yield return new Entity(entityId, this);
                    }
                }
            }
        }

        // --- IDisposable Implementation ---

        /// <summary>
        /// Disposes of the World, clearing all entities and components.
        /// This should be called when a World is no longer needed (e.g., when changing scenes).
        /// </summary>
        public void Dispose()
        {
            _componentPools.Clear(); // Clear all component pools
            _activeEntityIds.Clear(); // Clear all active entity IDs
            _nextEntityId = 0; // Reset the ID counter

            // No unmanaged resources typically held directly by World itself,
            // but this ensures all internal collections are reset.
            GC.SuppressFinalize(this); // Prevent finalizer from running twice if Dispose is called manually
        }
    }
}
