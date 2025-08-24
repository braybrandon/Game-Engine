using GameEngine.Common.Interfaces;
using GameEngine.Core.Entities;

namespace GameEngine.Engine.Scene
{
    /// <summary>
    /// A pool for storing components of a specific type (T), providing efficient storage and direct 'ref' access.
    /// Implements a swap-remove strategy for efficient deletion and dense packing.
    /// </summary>
    /// <typeparam name="T">The type of component this pool manages (must be a struct implementing IComponent).</typeparam>
    internal class ComponentPool<T> : IComponentPool where T : struct, IComponent
    {
        private T[] _componentsArray;
        private Dictionary<int, int> _entityIdToIndex;
        private Dictionary<int, int> _indexToEntityId;
        private int _count;
        private const int DefaultCapacity = 256;

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
                return ref _componentsArray[index];
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
                _componentsArray[index] = component;
                return ref _componentsArray[index];
            }
            else
            {
                if (_count == _componentsArray.Length)
                {
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
        /// Removes the component associated with the given entity ID from this pool using swap-remove for dense packing.
        /// </summary>
        /// <param name="entityId">The ID of the entity whose component to remove.</param>
        public void Remove(int entityId)
        {
            if (_entityIdToIndex.TryGetValue(entityId, out int indexToRemove))
            {
                _entityIdToIndex.Remove(entityId);
                _indexToEntityId.Remove(indexToRemove);
                _count--;
                if (indexToRemove < _count)
                {
                    int lastComponentEntityId = _indexToEntityId[_count];
                    _componentsArray[indexToRemove] = _componentsArray[_count];
                    _entityIdToIndex[lastComponentEntityId] = indexToRemove;
                    _indexToEntityId[indexToRemove] = lastComponentEntityId;
                }
                _componentsArray[_count] = default;
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
            Array.Copy(_componentsArray, newArray, _count);
            _componentsArray = newArray;
        }
    }

    /// <summary>
    /// The central data store for the Entity-Component-System. Manages creation and destruction of entities, and storage and retrieval of their components using specialized component pools.
    /// </summary>
    public class World : IWorld
    {
        private int _nextEntityId = 0;
        private readonly HashSet<int> _activeEntityIds = new HashSet<int>();
        private readonly HashSet<int> _entitiesToDestroy = new HashSet<int>();
        private readonly Dictionary<Type, IComponentPool> _componentPools = new Dictionary<Type, IComponentPool>();

        /// <summary>
        /// Initializes a new instance of the World class.
        /// </summary>
        public World() { }

        /// <summary>
        /// Creates a new unique entity in this World.
        /// </summary>
        /// <returns>The newly created Entity struct, which acts as a handle.</returns>
        public IEntity CreateEntity()
        {
            int entityId = _nextEntityId++;
            _activeEntityIds.Add(entityId);
            return new Entity(entityId, this);
        }

        /// <summary>
        /// Destroys an entity and removes all its associated components from this World.
        /// </summary>
        /// <param name="entity">The entity to destroy.</param>
        public void DestroyEntity(IEntity entity)
        {
            if (!_activeEntityIds.Contains(entity.Id)) return;
            _activeEntityIds.Remove(entity.Id);
            _entitiesToDestroy.Add(entity.Id);
        }

        /// <summary>
        /// Removes all inactive entities and their components from the World.
        /// </summary>
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
            _entitiesToDestroy.Clear();
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
            if (!_componentPools.TryGetValue(typeof(T), out var pool))
            {
                pool = new ComponentPool<T>();
                _componentPools[typeof(T)] = pool;
            }
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
            if (_componentPools.TryGetValue(typeof(T), out var pool))
            {
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
            if (!_activeEntityIds.Contains(entityId)) return;
            if (_componentPools.TryGetValue(typeof(T), out var pool))
            {
                pool.Remove(entityId);
            }
        }

        /// <summary>
        /// Gets all active entities that have a component of type T1.
        /// </summary>
        /// <typeparam name="T1">The type of the component to filter by.</typeparam>
        /// <returns>An enumerable collection of matching Entity structs.</returns>
        public IEnumerable<IEntity> GetEntitiesWith<T1>() where T1 : struct, IComponent
        {
            if (_componentPools.TryGetValue(typeof(T1), out var pool1))
            {
                foreach (var entityId in pool1.GetEntityIds())
                {
                    if (_activeEntityIds.Contains(entityId))
                    {
                        yield return new Entity(entityId, this);
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
            if (_componentPools.TryGetValue(typeof(T1), out var pool1) &&
                _componentPools.TryGetValue(typeof(T2), out var pool2))
            {
                foreach (var entityId in pool1.GetEntityIds().Intersect(pool2.GetEntityIds()))
                {
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
            if (_componentPools.TryGetValue(typeof(T1), out var pool1) &&
                _componentPools.TryGetValue(typeof(T2), out var pool2) &&
                _componentPools.TryGetValue(typeof(T3), out var pool3))
            {
                foreach (var entityId in pool1.GetEntityIds().Intersect(pool2.GetEntityIds()).Intersect(pool3.GetEntityIds()))
                {
                    if (_activeEntityIds.Contains(entityId))
                    {
                        yield return new Entity(entityId, this);
                    }
                }
            }
        }

        /// <summary>
        /// Gets all active entities that have components of type T1, T2, T3, and T4.
        /// </summary>
        /// <typeparam name="T1">The first component type.</typeparam>
        /// <typeparam name="T2">The second component type.</typeparam>
        /// <typeparam name="T3">The third component type.</typeparam>
        /// <typeparam name="T4">The fourth component type.</typeparam>
        /// <returns>An enumerable collection of matching Entity structs.</returns>
        public IEnumerable<IEntity> GetEntitiesWith<T1, T2, T3, T4>() where T1 : struct, IComponent where T2 : struct, IComponent where T3 : struct, IComponent where T4 : struct, IComponent
        {
            if (_componentPools.TryGetValue(typeof(T1), out var pool1) &&
                _componentPools.TryGetValue(typeof(T2), out var pool2) &&
                _componentPools.TryGetValue(typeof(T3), out var pool3) &&
                _componentPools.TryGetValue(typeof(T4), out var pool4))
            {
                foreach (var entityId in pool1.GetEntityIds()
                    .Intersect(pool2.GetEntityIds())
                    .Intersect(pool3.GetEntityIds())
                    .Intersect(pool4.GetEntityIds()))
                {
                    if (_activeEntityIds.Contains(entityId))
                    {
                        yield return new Entity(entityId, this);
                    }
                }
            }
        }

        /// <summary>
        /// Disposes of the World, clearing all entities and components.
        /// This should be called when a World is no longer needed (e.g., when changing scenes).
        /// </summary>
        public void Dispose()
        {
            _componentPools.Clear();
            _activeEntityIds.Clear();
            _nextEntityId = 0;
            GC.SuppressFinalize(this);
        }
    }
}
