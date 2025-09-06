
using GameEngine.Common.Interfaces;
using Microsoft.Xna.Framework;

namespace GameEngine.Common.Physics.Components
{
    /// <summary>
    /// Defines the physical collider properties for an entity, enabling collision detection and response in the physics system.
    /// Stores bounding box, offset, trigger/static flags, friction, and bounciness to control how the entity interacts with other colliders.
    /// Used for hit detection, physics simulation, and gameplay logic.
    /// </summary>
    public struct ColliderComponent : IComponent
    {
        /// <summary>
        /// The bounding box of the collider, relative to the entity's position.
        /// </summary>
        public Rectangle Bounds;

        /// <summary>
        /// An offset from the entity's position, allowing the collider to be placed off-center.
        /// </summary>
        public Vector2 Offset;

        public CollisionFilter Filter;

        /// <summary>
        /// Indicates if this collider is a trigger. Triggers detect collisions but do not cause a physical response.
        /// </summary>
        public bool IsTrigger;

        /// <summary>
        /// Indicates if this collider is static. Static colliders do not move and are used for level geometry.
        /// </summary>
        public bool IsStatic;

        /// <summary>
        /// The friction value of the collider's surface (0.0 to 1.0).
        /// </summary>
        public float Friction;

        /// <summary>
        /// The bounciness (restitution) value of the collider's surface (0.0 to 1.0).
        /// </summary>
        public float Bounciness;
    }
}
