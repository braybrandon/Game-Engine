using Common.Interfaces;
using Common.Physics.Components;
using GameEngine.Core.Components;
using Microsoft.Xna.Framework;

namespace GameEngine.Physics.Motion
{
    public class MovementSystem : IUpdateSystem
    {
        ITimeManager _timeManager;
        CollisionMap _collisionMap;

        public MovementSystem(ITimeManager timeManager, ITileMap tileMap)
        {
            _timeManager = timeManager;
            _collisionMap = new CollisionMap(tileMap, tileMap.Layers[0]);
        }


        public void Update(IWorld world)
        {
            float dt = _timeManager.ScaledDeltaTime;

            foreach (var player in world.GetEntitiesWith<TransformComponent, ProposedPositionComponent, DirectionComponent, SpeedComponent>())
            {
                // Get copies of the components
                ref var direction = ref player.GetComponent<DirectionComponent>();
                ref var transform = ref player.GetComponent<TransformComponent>();
                ref var speed = ref player.GetComponent<SpeedComponent>();
                ref var proposedPositionComponent = ref player.GetComponent<ProposedPositionComponent>();

                Vector2 velocity = direction.Value * speed.Value;

                proposedPositionComponent.Value = transform.Position + velocity * dt;
            }
        }
    }
}
