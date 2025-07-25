using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.Components
{
    public struct PositionComponent
    {
        public Vector2 Position;
    }

    public struct VelocityComponent
    {
        public Vector2 Value; 
    }

    public struct HealthComponent
    {
        public int CurrentHealth;
        public int MaxHealth;
    }

    public struct SpriteComponent
    {
        public Texture2D Texture;
        public Rectangle SourceRectangle; 
        public Color Color;
        public float Scale;
        public float Rotation;
        public Vector2 Origin;
        public SpriteEffects Effects;
        public float LayerDepth;
    }

    public struct PlayerInputComponent
    {
        public bool IsPlayerControlled; 
    }

    public struct ColliderComponent
    {
        public Rectangle Bounds; 
        public bool IsTrigger;
        public bool IsStatic;
    }

    /// <summary>
    /// Represents a single animation clip.
    /// </summary>
    public struct AnimationClip
    {
        public string Name;
        public Texture2D Texture; // The specific texture sheet for this animation clip
        public List<Rectangle> Frames; // Source rectangles for each frame
        public float FrameDuration; // How long each frame is displayed (in seconds)
        public bool IsLooping;
    }

    /// <summary>
    /// Component holding animation data and current state for an entity.
    /// </summary>
    public struct AnimationComponent
    {
        public Dictionary<string, AnimationClip> Clips; // All animation clips by name
        public string CurrentClipName; // Key of the currently playing clip
        public float TimeInClip; // Time elapsed since current clip started
        public int CurrentFrameIndex; // Current frame being displayed

        public AnimationClip CurrentClip => Clips.ContainsKey(CurrentClipName) ? Clips[CurrentClipName] : default(AnimationClip);

        public void Play(string clipName)
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
