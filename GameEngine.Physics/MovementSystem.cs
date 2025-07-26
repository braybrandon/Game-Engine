using GameEngine.Core.Components;
using GameEngine.Core.Services;
using GameEngine.Core.Systems;
using Microsoft.Xna.Framework;

namespace GameEngine.Physics
{
    public class MovementSystem : EngineSystem
    {

        public override void Update(World world)
        {
            ITimeManager timeManager = ServiceLocator.GetService<ITimeManager>();
            float dt = timeManager.ScaledDeltaTime;

            foreach (var entity in world.GetEntitiesWith<TransformComponent, VelocityComponent>())
            {
                // Get copies of the components
                ref var velocity = ref entity.GetComponent<VelocityComponent>();
                ref var transform = ref entity.GetComponent<TransformComponent>();  

                transform.Position += velocity.Value * dt;
            }
        }
    }
}
