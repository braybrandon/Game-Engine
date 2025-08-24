using GameEngine.Common.Interfaces;

namespace GameEngine.Gameplay.AI.Components
{
    /// <summary>
    /// Represents the perception properties of an AI entity, including detection radius, field of view, and targeting mask.
    /// Used to control how AI senses and interacts with targets in the game world.
    /// </summary>
    public struct PerceptionComponent : IComponent
    {
        /// <summary>
        /// The radius within which the AI can detect targets.
        /// </summary>
        public float Radius;
        /// <summary>
        /// The radius at which the AI will drop its target.
        /// </summary>
        public float DropRadius;
        /// <summary>
        /// The field of view angle in degrees for target detection.
        /// </summary>
        public float FieldOfViewDeg;
        /// <summary>
        /// The leash radius, limiting how far the AI can pursue a target.
        /// </summary>
        public float LeashRadius;
        /// <summary>
        /// The mask used to filter which targets the AI can perceive.
        /// </summary>
        public int TargetMask;
    }
}
