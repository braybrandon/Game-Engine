using Common.Components;
using Common.Interfaces;
using Common.Physics.Components;

namespace GameEngine.Engine
{
    public class ProjectileLiftetimeSystem(ITimeManager timeManager) : IUpdateSystem
    {
        private readonly ITimeManager _timeManager = timeManager;
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
                    // Mark the entity for removal
                    world.DestroyEntity(entity); // or whatever method you use to remove entities
                }
            }
        }
    }
}
