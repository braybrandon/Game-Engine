using GameEngine.Common.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text.Json.Serialization;

namespace GameEnginePlayground.Factories.DataObjects
{
    /// <summary>
    /// Represents the data required to create a projectile entity, including speed, sound, texture, lifetime, and rendering properties.
    /// Extend this class to include additional configuration as needed for different projectile types.
    /// </summary>
    public class ProjectileData
    {
        public CollisionFilter Tag { get; set; } 
        /// <summary>Speed of the projectile.</summary>
        public float ProjectileSpeed { get; set; }
        /// <summary>Sound file to play when the projectile is created or fired.</summary>
        public string SoundFile { get; set; }
        /// <summary>Volume of the sound effect.</summary>
        public float SoundVolume { get; set; }
        /// <summary>Stereo pan value for the sound effect.</summary>
        public float SoundPan { get; set; }
        /// <summary>Pitch adjustment for the sound effect.</summary>
        public float SoundPitch { get; set; }
        /// <summary>Texture to use for the projectile sprite.</summary>
        public Texture2D Texture { get; set; }
        /// <summary>Lifetime of the projectile in frames or seconds.</summary>
        public int Lifetime { get; set; }
        /// <summary>Whether the projectile's collider is a trigger.</summary>
        public bool IsTrigger { get; set; }
        /// <summary>Whether the projectile's collider is static.</summary>
        public bool IsStatic { get; set; }
        /// <summary>Color tint for the projectile sprite.</summary>
        public Color SpriteColor { get; set; }
        /// <summary>Scale factor for the projectile sprite.</summary>
        public float SpriteScale { get; set; }
        /// <summary>Rotation angle for the projectile sprite.</summary>
        public float SpriteRotation { get; set; }
        /// <summary>Layer depth for rendering the projectile sprite.</summary>
        public float SpriteLayerDepth { get; set; }
        /// <summary>Sprite effects (e.g., flip horizontally/vertically).</summary>
        public SpriteEffects SpriteEffects { get; set; }
        /// <summary>Initial position of the projectile in world space.</summary>
        [JsonIgnore]
        public Vector2 InitialPosition { get; set; }
        /// <summary>Initial rotation of the projectile.</summary>
        [JsonIgnore]
        public float InitialRotation { get; set; }
        /// <summary>Initial scale of the projectile.</summary>
        [JsonIgnore]
        public Vector2 InitialScale { get; set; }
        /// <summary>Direction vector for the projectile's movement.</summary>
        [JsonIgnore]
        public Vector2 Direction { get; set; }
        /// <summary>Bounding rectangle for the projectile's collider.</summary>
        [JsonIgnore]
        public Rectangle ColliderBounds { get; set; }
        /// <summary>Source rectangle for the projectile's sprite texture.</summary>
        [JsonIgnore]
        public Rectangle SpriteSourceRectangle { get; set; }
        /// <summary>Origin point for the projectile's sprite.</summary>
        [JsonIgnore]
        public Vector2 SpriteOrigin { get; set; }
    }
}
