using GameEngine.Common.Interfaces;
using GameEngine.Common.Physics.Components;
using Microsoft.Xna.Framework;

namespace GameEngine.Physics.Motion
{
    public class MovementSystem : IUpdateSystem
    {
        ITimeManager _timeManager;

        public MovementSystem(ITimeManager timeManager)
        {
            _timeManager = timeManager;
        }


        public void Update(IWorld world)
        {
            float dt = _timeManager.ScaledDeltaTime;

            foreach (var entity in world.GetEntitiesWith<TransformComponent, ProposedPositionComponent, DirectionComponent, SpeedComponent>())
            {
                // Get copies of the components
                ref var direction = ref entity.GetComponent<DirectionComponent>();
                ref var transform = ref entity.GetComponent<TransformComponent>();
                ref var speed = ref entity.GetComponent<SpeedComponent>();
                ref var proposedPositionComponent = ref entity.GetComponent<ProposedPositionComponent>();

                Vector2 velocity = direction.Value * speed.Value;

                proposedPositionComponent.Value = transform.Position + velocity * dt;
            }
        }
    }
}
