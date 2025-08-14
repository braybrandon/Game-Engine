using Common.Components;
using Common.Interfaces;
using GameEngine.Core.Components;
using GameEngine.Core.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Engine
{
    public class ProjectileLiftetimeSystem(ITimeManager timeManager) : IUpdateSystem
    {
        private readonly ITimeManager _timeManager = timeManager;
        public void Update(IWorld world)
        {
            foreach (var entity in world.GetEntitiesWith<LifetimeComponent, VelocityComponent>())
            {
                ref var velocity = ref entity.GetComponent<VelocityComponent>();
                ref var lifetime = ref entity.GetComponent<LifetimeComponent>();

                // Calculate the distance moved this frame
                float distanceThisFrame = velocity.Value.Length() * (float)_timeManager.UnscaledDeltaTime;

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
