using GameEngine.Common.Interfaces;
using Microsoft.Xna.Framework;

namespace GameEngine.Common.Physics.Components
{
    /// <summary>
    /// Represents the direction of an entity in the physics system.
    /// Stores a Vector2 value indicating the entity's movement or facing direction, which can be used for navigation, movement calculations, or orientation.
    /// </summary>
    public struct DirectionComponent(Vector2 direction) : IComponent
    {
        /// <summary>
        /// The direction vector value for the entity.
        /// </summary>
        public Vector2 Value { get; set; } = direction;
    }
}
