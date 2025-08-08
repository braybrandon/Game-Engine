using Common.Interfaces;
using GameEngine.Core.Components;
using GameEngine.Core.Services;
using Microsoft.Xna.Framework;

namespace GameEngine.Physics.Motion
{
    public class MotionSystem : IUpdateSystem
    {
        ITimeManager _timeManager;
        CollisionMap _collisionMap;

        public MotionSystem(ITimeManager timeManager, ITileMap tileMap)
        {
            _timeManager = timeManager;
            _collisionMap = new CollisionMap(tileMap, tileMap.Layers[0]);
        }

        public void Update(IWorld world)
        {
            float dt = _timeManager.ScaledDeltaTime;

            foreach (var entity in world.GetEntitiesWith<TransformComponent, VelocityComponent>())
            {
                // Get copies of the components
                ref var velocity = ref entity.GetComponent<VelocityComponent>();
                ref var transform = ref entity.GetComponent<TransformComponent>();

                Vector2 proposedPosition = transform.Position + velocity.Value * dt;

                   // Default to 32x32 if no collider is available
                Rectangle colliderBounds = new Rectangle(-16, -16, 32, 32);

                if (entity.HasComponent<ColliderComponent>())
                {
                    ref var collider = ref entity.GetComponent<ColliderComponent>();
                    colliderBounds = collider.Bounds;
                }

                var bounds = new Rectangle(
                    (int)proposedPosition.X - colliderBounds.X,
                    (int)proposedPosition.Y - colliderBounds.Y,
                    colliderBounds.Width,
                    colliderBounds.Height
                );



                if (!_collisionMap.IsSolid(bounds, 16, 16))
                {
                    transform.Position = proposedPosition;
                }
                
            }
        }
    }
}
