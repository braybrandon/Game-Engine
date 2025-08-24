using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Common.Interfaces
{

        /// <summary>
        /// The central data store for the Entity-Component-System.
        /// It manages the creation and destruction of entities, and the storage and retrieval of their components
        /// using specialized component pools for performance.
        /// </summary>
        public interface IWorld : IDisposable
        {
            // --- Entity Management Methods ---

            /// <summary>
            /// Creates a new unique entity in this World.
            /// </summary>
            /// <returns>The newly created Entity struct, which acts as a handle.</returns>
            IEntity CreateEntity();

            /// <summary>
            /// Destroys an entity and removes all its associated components from this World.
            /// </summary>
            /// <param name="entity">The entity to destroy.</param>
            void DestroyEntity(IEntity entity);

            void RemoveInactiveEntities();

            /// <summary>
            /// Checks if a given entity ID currently represents an active entity in this World.
            /// </summary>
            /// <param name="entityId">The ID of the entity to check.</param>
            /// <returns>True if the entity is active, false otherwise.</returns>
            bool IsEntityActive(int entityId);

            // --- Component Management Methods ---

            /// <summary>
            /// Adds a new component to the specified entity in this World, or replaces an existing one.
            /// </summary>
            /// <typeparam name="T">The type of the component (must be a struct implementing IComponent).</typeparam>
            /// <param name="entityId">The ID of the entity to add the component to.</param>
            /// <param name="component">The component instance to add.</param>
            /// <returns>A reference to the added/updated component in the World's storage.</returns>
            /// <exception cref="InvalidOperationException">Thrown if attempting to add a component to an inactive entity.</exception>
            ref T AddComponent<T>(int entityId, T component) where T : struct, IComponent;

            /// <summary>
            /// Retrieves a component of a specific type from the specified entity in this World.
            /// </summary>
            /// <typeparam name="T">The type of the component to retrieve (must be a struct implementing IComponent).</typeparam>
            /// <param name="entityId">The ID of the entity to get the component from.</param>
            /// <returns>A reference to the component in the World's storage.</returns>
            /// <exception cref="InvalidOperationException">Thrown if attempting to get a component from an inactive entity.</exception>
            /// <exception cref="KeyNotFoundException">Thrown if the component is not found for the entity.</exception>
            ref T GetComponent<T>(int entityId) where T : struct, IComponent;

            /// <summary>
            /// Checks if the specified entity has a component of a specific type in this World.
            /// </summary>
            /// <typeparam name="T">The type of the component to check for (must be a struct implementing IComponent).</typeparam>
            /// <param name="entityId">The ID of the entity to check.</param>
            /// <returns>True if the entity has the component, false otherwise.</returns>
            bool HasComponent<T>(int entityId) where T : struct, IComponent;

            /// <summary>
            /// Removes a component of a specific type from the specified entity in this World.
            /// </summary>
            /// <typeparam name="T">The type of the component to remove (must be a struct implementing IComponent).</typeparam>
            /// <param name="entityId">The ID of the entity to remove the component from.</param>
            void RemoveComponent<T>(int entityId) where T : struct, IComponent;


            // --- Entity Querying Methods ---
            // These methods allow systems to efficiently find entities that possess specific component combinations.

            /// <summary>
            /// Gets all active entities that have a component of type T1.
            /// </summary>
            /// <typeparam name="T1">The type of the component to filter by.</typeparam>
            /// <returns>An enumerable collection of matching Entity structs.</returns>
            IEnumerable<IEntity> GetEntitiesWith<T1>() where T1 : struct, IComponent;

            /// <summary>
            /// Gets all active entities that have components of both type T1 and T2.
            /// </summary>
            /// <typeparam name="T1">The first component type.</typeparam>
            /// <typeparam name="T2">The second component type.</typeparam>
            /// <returns>An enumerable collection of matching Entity structs.</returns>
            IEnumerable<IEntity> GetEntitiesWith<T1, T2>() where T1 : struct, IComponent where T2 : struct, IComponent;

            /// <summary>
            /// Gets all active entities that have components of type T1, T2, and T3.
            /// </summary>
            /// <typeparam name="T1">The first component type.</typeparam>
            /// <typeparam name="T2">The second component type.</typeparam>
            /// <typeparam name="T3">The third component type.</typeparam>
            /// <returns>An enumerable collection of matching Entity structs.</returns>
             IEnumerable<IEntity> GetEntitiesWith<T1, T2, T3>() where T1 : struct, IComponent where T2 : struct, IComponent where T3 : struct, IComponent;
        /// <summary>
        ///  Gets all active entities that have components of type T1, T2, T3, and T4.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <returns></returns>
        IEnumerable<IEntity> GetEntitiesWith<T1, T2, T3, T4>() where T1 : struct, IComponent where T2 : struct, IComponent where T3 : struct, IComponent where T4: struct, IComponent;

            // --- IDisposable Implementation ---

            /// <summary>
            /// Disposes of the World, clearing all entities and components.
            /// This should be called when a World is no longer needed (e.g., when changing scenes).
            /// </summary>
            void Dispose();
            }
    }