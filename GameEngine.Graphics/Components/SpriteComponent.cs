using GameEngine.Common.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Graphics.Components
{

    /// <summary>
    /// Represents the sprite rendering properties for an entity.
    /// Stores texture, source rectangle, color tint, scale, rotation, origin, sprite effects, and layer depth.
    /// Used by rendering systems to draw the entity's sprite with the specified visual attributes.
    /// </summary>
    public struct SpriteComponent : IComponent
    {
        /// <summary>
        /// The texture to render for the sprite.
        /// </summary>
        public Texture2D Texture;

        /// <summary>
        /// The source rectangle within the texture to render.
        /// </summary>
        public Rectangle SourceRectangle;

        /// <summary>
        /// The color tint to apply to the sprite.
        /// </summary>
        public Color Color;

        /// <summary>
        /// The scale factor to apply when rendering the sprite.
        /// </summary>
        public float Scale;

        /// <summary>
        /// The rotation angle (in radians) to apply to the sprite.
        /// </summary>
        public float Rotation;

        /// <summary>
        /// The origin point for rotation and scaling.
        /// </summary>
        public Vector2 Origin;

        /// <summary>
        /// The sprite effects (e.g., flip horizontally/vertically) to apply.
        /// </summary>
        public SpriteEffects Effects;

        /// <summary>
        /// The layer depth for rendering order.
        /// </summary>
        public float LayerDepth;
    }

}
