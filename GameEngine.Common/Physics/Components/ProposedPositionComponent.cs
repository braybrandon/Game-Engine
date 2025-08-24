using GameEngine.Common.Interfaces;
using Microsoft.Xna.Framework;

namespace GameEngine.Common.Physics.Components
{
    /// <summary>
    /// Represents a proposed position for an entity in the physics system.
    /// Stores a Vector2 value indicating where the entity is intended to move or be placed next.
    /// Useful for movement prediction, collision checks, or deferred position updates.
    /// </summary>
    /// <param name="position"></param>
    public struct ProposedPositionComponent(Vector2 position) : IComponent
    {
        /// <summary>
        /// The proposed position value for the entity.
        /// </summary>
        public Vector2 Value { get; set; } = position;
    }
}
