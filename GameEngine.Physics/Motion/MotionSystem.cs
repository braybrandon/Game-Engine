using Common.Interfaces;
using GameEngine.Core.Components;
using GameEngine.Core.Services;

namespace GameEngine.Physics.Motion
{
    public class MotionSystem(IServiceLocator serviceLocator) : IUpdateSystem
    {
        IServiceLocator _serviceLocator = serviceLocator;

        public void Update(IWorld world)
        {
            ITimeManager timeManager = _serviceLocator.GetService<ITimeManager>();
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
