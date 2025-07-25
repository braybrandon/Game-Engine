using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core.Components
{
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
        public KeyboardState CurrentKeyboardState;
        public KeyboardState PreviousKeyboardState;
        public MouseState CurrentMouseState;
        public MouseState PreviousMouseState;
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

    public enum AnimationType
    {
        Idle,
        WalkUp,
        WalkDown,
        WalkLeft,
        WalkRight
    }

    /// <summary>
    /// Component holding animation data and current state for an entity.
    /// </summary>
    public struct AnimationComponent
    {
        public Dictionary<AnimationType, AnimationClip> Clips; // All animation clips by name
        public AnimationType CurrentClipName; // Key of the currently playing clip
        public float TimeInClip; // Time elapsed since current clip started
        public int CurrentFrameIndex; // Current frame being displayed

        public AnimationClip CurrentClip => Clips.ContainsKey(CurrentClipName) ? Clips[CurrentClipName] : default(AnimationClip);

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

    public struct TransformComponent
    {
        public Vector2 Position;
        public float Rotation;
        public Vector2 Scale;

        public Matrix GetWorldMatrix()
        {
            return Matrix.CreateScale(Scale.X, Scale.Y, 1f) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateTranslation(Position.X, Position.Y, 0f);
        }
    }
}
