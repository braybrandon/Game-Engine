using GameEngine.Common.Interfaces;

namespace GameEngine.Common.Physics.Components
{
    /// <summary>
    /// Represents the speed of an entity in the physics system.
    /// Encapsulates a single float value indicating the entity's speed, which can be used for movement calculations.
    /// </summary>
    public struct SpeedComponent : IComponent
    {
        /// <summary>
        /// The speed value of the entity.
        /// </summary>
        public float Value { get; set; }
    }
}
