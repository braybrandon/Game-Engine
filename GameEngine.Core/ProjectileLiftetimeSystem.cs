using Common.Components;
using Common.Interfaces;
using Common.Physics.Components;
using Common.Physics.Interfaces;
using Microsoft.Xna.Framework;

namespace GameEngine.Engine
{
    public class ProjectileLiftetimeSystem(ITimeManager timeManager, IQuadTree quadTree) : IUpdateSystem
    {
        private readonly ITimeManager _timeManager = timeManager;
        private IQuadTree _quadTree = quadTree;
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
