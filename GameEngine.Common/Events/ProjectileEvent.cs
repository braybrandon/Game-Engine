using GameEngine.Common.Interfaces;

namespace GameEngine.Common.Events
{
    /// <summary>
    /// Represents an event related to a projectile in the game, including its type, entity ID, world context, and timestamp.
    /// Used to signal projectile actions or state changes within the ECS event system.
    /// </summary>
    public class ProjectileEvent : IEvent
    {
        /// <summary>
        /// Gets the type or name of the projectile event.
        /// </summary>
        public string Type { get; }

        /// <summary>
        /// Gets the ID of the entity associated with the projectile event.
        /// </summary>
        public int EntityId { get; }

        /// <summary>
        /// Gets the world context in which the projectile event occurred.
        /// </summary>
        public IWorld World { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectileEvent"/> class.
        /// </summary>
        /// <param name="entityId">The ID of the entity associated with the projectile.</param>
        /// <param name="name">The type or name of the projectile event.</param>
        /// <param name="world">The world context for the event.</param>
        public ProjectileEvent(int entityId, string name, IWorld world)
        {
            EntityId = entityId;
            Type = name;
            World = world;
        }

        /// <summary>
        /// Gets the UTC timestamp when the event was created.
        /// </summary>
        public DateTime Timestamp { get; } = DateTime.UtcNow;
    }
}
