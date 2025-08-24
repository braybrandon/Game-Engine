using GameEngine.Common.Interfaces;

namespace GameEngine.Gameplay.AI.Components
{
    /// <summary>
    /// Represents the target entity for an AI-controlled entity.
    /// Used to store a reference to the entity that is currently being targeted by AI logic.
    /// </summary>
    public struct TargetComponent : IComponent
    {
        /// <summary>
        /// The entity currently targeted by the AI.
        /// </summary>
        public IEntity Value;
    }
}
