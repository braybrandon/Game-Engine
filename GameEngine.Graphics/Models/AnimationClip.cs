using GameEngine.Common.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Graphics.Models
{
    /// <summary>
    /// Represents an animation clip for a sprite, containing frames, timing, and playback settings.
    /// Used to define a sequence of frames and playback behavior for sprite animations in the rendering system.
    /// </summary>
    public struct AnimationClip : IComponent
    {
        /// <summary>
        /// The name of the animation clip.
        /// </summary>
        public string Name;

        /// <summary>
        /// The texture containing the animation frames.
        /// </summary>
        public Texture2D Texture;

        /// <summary>
        /// The list of rectangles representing individual animation frames within the texture.
        /// </summary>
        public List<Rectangle> Frames;

        /// <summary>
        /// The duration (in seconds) of each frame in the animation.
        /// </summary>
        public float FrameDuration;

        /// <summary>
        /// Indicates whether the animation should loop when it reaches the end.
        /// </summary>
        public bool IsLooping;
    }
}
