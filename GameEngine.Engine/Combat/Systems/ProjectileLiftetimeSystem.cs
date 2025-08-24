using Common.Components;
using Common.Interfaces;
using Common.Physics.Components;
using Common.Physics.Interfaces;
using Microsoft.Xna.Framework;

namespace GameEngine.Engine.Combat.Systems
{
    /// <summary>
    /// System that manages the lifetime of projectile entities, removing them when they exceed their maximum distance.
    /// </summary>
    /// <param name="timeManager">The time manager used for delta time calculations.</param>
    /// <param name="quadTree">The quadtree used for spatial partitioning and removal.</param>
    public class ProjectileLiftetimeSystem : IUpdateSystem
    {
        private readonly ITimeManager _timeManager;
        private IQuadTree _quadTree;

        /// <summary>
        /// Initializes a new instance of the ProjectileLiftetimeSystem class.
        /// </summary>
        /// <param name="timeManager">The time manager used for delta time calculations.</param>
        /// <param name="quadTree">The quadtree used for spatial partitioning and removal.</param>
        public ProjectileLiftetimeSystem(ITimeManager timeManager, IQuadTree quadTree)
        {
            _timeManager = timeManager;
            _quadTree = quadTree;
        }

        /// <summary>
        /// Updates the system, checking projectile lifetimes and removing entities that have exceeded their maximum distance.
        /// </summary>
        /// <param name="world">The world containing entities and components.</param>
        public void Update(IWorld world)
        {
            foreach (var entity in world.GetEntitiesWith<LifetimeComponent, DirectionComponent, SpeedComponent>())
            {
                ref var direction = ref entity.GetComponent<DirectionComponent>();
                ref var lifetime = ref entity.GetComponent<LifetimeComponent>();
                ref var speed = ref entity.GetComponent<SpeedComponent>();

                var velocity = direction.Value * speed.Value;
                // Calculate the distance moved this frame
                float distanceThisFrame = velocity.Length() * (float)_timeManager.UnscaledDeltaTime;

                // Add to total distance traveled
                lifetime.DistanceTraveled += distanceThisFrame;

                // Check if the projectile has traveled its maximum distance
                if (lifetime.DistanceTraveled >= lifetime.MaxDistance)
                {
                    ref var transform = ref entity.GetComponent<TransformComponent>();
                    ref var collider = ref entity.GetComponent<ColliderComponent>();


                    Rectangle colliderBounds = collider.Bounds;
                    var transformBounds = new Rectangle(
                        (int)transform.Position.X - colliderBounds.X,
                        (int)transform.Position.Y - colliderBounds.Y,
                        colliderBounds.Width,
                        colliderBounds.Height
                    );
                    _quadTree.Remove(entity, transformBounds);
                    // Mark the entity for removal
                    world.DestroyEntity(entity); // or whatever method you use to remove entities
                    
                }
            }
        }
    }
}
