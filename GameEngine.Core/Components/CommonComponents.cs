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
}
