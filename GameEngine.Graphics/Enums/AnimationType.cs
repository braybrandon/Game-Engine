namespace GameEngine.Graphics.Enums
{
    /// <summary>
    /// Specifies the different types of animations that can be applied to an entity's sprite.
    /// Used to control the visual state and movement direction of animated entities in the game.
    /// </summary>
    public enum AnimationType
    {
        /// <summary>
        /// The entity is idle and not moving.
        /// </summary>
        Idle,
        /// <summary>
        /// The entity is walking upward.
        /// </summary>
        WalkUp,
        /// <summary>
        /// The entity is walking downward.
        /// </summary>
        WalkDown,
        /// <summary>
        /// The entity is walking to the left.
        /// </summary>
        WalkLeft,
        /// <summary>
        /// The entity is walking to the right.
        /// </summary>
        WalkRight,
        /// <summary>
        /// The entity is walking upward and to the right.
        /// </summary>
        WalkUpRight,
        /// <summary>
        /// The entity is walking upward and to the left.
        /// </summary>
        WalkUpLeft,
        /// <summary>
        /// The entity is walking downward and to the right.
        /// </summary>
        WalkDownRight,
        /// <summary>
        /// The entity is walking downward and to the left.
        /// </summary>
        WalkDownLeft
    }
}
