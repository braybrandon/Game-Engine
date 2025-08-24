using GameEngine.Common.Interfaces;
using Microsoft.Xna.Framework;

namespace GameEngine.Common.Physics.Components
{
    /// <summary>
    /// Represents an entity's transformation in 2D space, including position, rotation, and scale.
    /// Provides a method to calculate the world transformation matrix by combining scale, rotation, and translation,
    /// allowing conversion from local to world coordinates. Used for managing spatial properties in physics or rendering systems.
    /// </summary>
    public struct TransformComponent : IComponent
    {
        /// <summary>
        /// Vector position of the Transformation
        /// </summary>
        public Vector2 Position;
        /// <summary>
        /// Rotation of the transformation
        /// </summary>
        public float Rotation;
        /// <summary>
        /// Scale of the transformation
        /// </summary>
        public Vector2 Scale;

        /// <summary>
        /// Calculates the world transformation matrix for the entity.
        /// This matrix combines the entity's scale, rotation, and position to transform local coordinates into world space coordinates.
        /// The transformations are applied in the order of Scale, then Rotation, then Translation, which is the standard
        /// right-to-left multiplication order for matrices.
        /// </summary>
        /// <returns>A new Matrix that represents the combined world transformation of the entity.</returns>
        public Matrix GetWorldMatrix()
        {
            return Matrix.CreateScale(Scale.X, Scale.Y, 1f) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateTranslation(Position.X, Position.Y, 0f);
        }
    }
}
