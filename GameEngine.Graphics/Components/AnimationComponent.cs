using Common.Interfaces;
using GameEngine.Graphics.Enums;
using GameEngine.Graphics.Models;

namespace GameEngine.Graphics.Components
{
    /// <summary>
    /// Manages sprite animation state for an entity, including available animation clips, current animation, timing, and frame index.
    /// Used by animation systems to control playback and switching between different animations.
    /// </summary>
    public struct AnimationComponent : IComponent
    {
        /// <summary>
        /// A dictionary of available animation clips, keyed by animation type.
        /// </summary>
        public Dictionary<AnimationType, AnimationClip> Clips;

        /// <summary>
        /// The type of the currently playing animation clip.
        /// </summary>
        public AnimationType CurrentClipName;

        /// <summary>
        /// The elapsed time (in seconds) within the current animation clip.
        /// </summary>
        public float TimeInClip;

        /// <summary>
        /// The index of the current frame in the animation clip.
        /// </summary>
        public int CurrentFrameIndex;

        /// <summary>
        /// Gets the currently active animation clip, or the default value if not found.
        /// </summary>
        public AnimationClip CurrentClip => Clips.ContainsKey(CurrentClipName) ? Clips[CurrentClipName] : default(AnimationClip);

        /// <summary>
        /// Switches to a new animation clip if available and resets timing and frame index.
        /// </summary>
        /// <param name="clipName">The animation type to play.</param>
        public void Play(AnimationType clipName)
        {
            if (Clips.ContainsKey(clipName) && CurrentClipName != clipName)
            {
                CurrentClipName = clipName;
                TimeInClip = 0f;
                CurrentFrameIndex = 0;
            }
        }
    }
}
