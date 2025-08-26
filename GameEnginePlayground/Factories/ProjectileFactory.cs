using GameEngine.Common.Components;
using GameEngine.Common.Interfaces;
using GameEngine.Common.Physics.Components;
using GameEngine.Common.Physics.Interfaces;
using GameEngine.Graphics.Components;
using GameEnginePlayground.Factories.DataObjects;
using Microsoft.Xna.Framework;

namespace GameEnginePlayground.Factories
{
    /// <summary>
    /// Factory for creating projectile entities with all required components, including transform, direction, speed, collider, and sprite.
    /// Uses world and quadtree dependencies to fully configure and insert the projectile entity.
    /// </summary>
    public class ProjectileFactory : IFactory<IEntity, ProjectileData>
    {
        private readonly IWorld _world;
        private readonly IQuadTree _quadTree;

        /// <summary>
        /// Initializes a new instance of the ProjectileFactory with the specified world and quadtree.
        /// </summary>
        /// <param name="world">The world to create the projectile entity in.</param>
        /// <param name="quadTree">The quadtree for spatial partitioning and collision.</param>
        public ProjectileFactory(IWorld world, IQuadTree quadTree)
        {
            _world = world;
            _quadTree = quadTree;
        }

        /// <summary>
        /// Creates a projectile entity with all required components, using the provided projectile data.
        /// Sets up transform, direction, speed, collider, and sprite, and inserts the entity into the quadtree.
        /// </summary>
        /// <param name="data">The projectile data used for configuration.</param>
        /// <returns>The fully configured projectile entity.</returns>
        public IEntity Create(ProjectileData data)
        {
            var fireball = _world.CreateEntity();
            fireball.AddComponent(new TransformComponent { Position = data.InitialPosition, Rotation = data.InitialRotation, Scale = data.InitialScale });
            fireball.AddComponent(new DirectionComponent { Value = data.Direction });
            fireball.AddComponent(new SpeedComponent { Value = data.ProjectileSpeed });
            fireball.AddComponent(new ProposedPositionComponent());
            fireball.AddComponent(new LifetimeComponent(data.Lifetime));

            fireball.AddComponent(new ColliderComponent { Bounds = data.ColliderBounds, IsTrigger = data.IsTrigger, IsStatic = data.IsStatic });

            _quadTree.Insert(fireball, new Rectangle((int)data.InitialPosition.X - data.ColliderBounds.X, (int)data.InitialPosition.Y - data.ColliderBounds.Y, data.ColliderBounds.Width, data.ColliderBounds.Height));

            fireball.AddComponent(new SpriteComponent
            {
                Texture = data.Texture,
                SourceRectangle = data.Texture.Bounds,
                Color = data.SpriteColor,
                Scale = data.SpriteScale,
                Rotation = data.SpriteRotation,
                Origin = data.SpriteOrigin,
                Effects = data.SpriteEffects,
                LayerDepth = data.SpriteLayerDepth
            });
            return fireball;
        }
    }
}
