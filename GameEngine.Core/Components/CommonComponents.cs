using Common.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace GameEngine.Core.Components
{
    public struct HealthComponent : IComponent
    
    {
        public int CurrentHealth;
        public int MaxHealth;
    }

    public struct SpriteComponent : IComponent
    
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




    /// <summary>
    /// Represents a single animation clip.
    /// </summary>
    public struct AnimationClip : IComponent
    {
        public string Name;
        public Texture2D Texture; // The specific texture sheet for this animation clip
        public List<Rectangle> Frames; // Source rectangles for each frame
        public float FrameDuration; // How long each frame is displayed (in seconds)
        public bool IsLooping;
    }

    public struct EnemyTypeComponent: IComponent
    {
        public string Type;
    }

    public struct TileComponent : IComponent
    {
        public int GID; // Global Tile ID (0 for empty)
        public string LayerName; // Name of the layer this tile belongs to

        public override string ToString() => $"GID: {GID}, Layer: {LayerName}";
    }

    public struct TilesetInfo
    {
        public int FirstGID;
        public string Name;
        public int TileWidth;
        public int TileHeight;
        public string ImageSource; // Path to the tileset image
        public int ImageWidth;
        public int ImageHeight;
    }


    public enum AnimationType
    {
        Idle,
        WalkUp,
        WalkDown,
        WalkLeft,
        WalkRight,
        WalkUpRight,
        WalkUpLeft,
        WalkDownRight,
        WalkDownLeft
    }

    /// <summary>
    /// Component holding animation data and current state for an entity.
    /// </summary>
    public struct AnimationComponent : IComponent
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


    
}
