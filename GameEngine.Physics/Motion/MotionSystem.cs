using Common.Components;
using Common.Events;
using Common.Interfaces;
using GameEngine.Core.Components;
using GameEngine.Core.Services;
using Microsoft.Xna.Framework;
using System.Diagnostics;

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

            foreach (var player in world.GetEntitiesWith<TransformComponent, VelocityComponent>())
            {
                // Get copies of the components
                ref var velocity = ref player.GetComponent<VelocityComponent>();
                ref var transform = ref player.GetComponent<TransformComponent>();

                Vector2 proposedPosition = transform.Position + velocity.Value * dt;

                   // Default to 32x32 if no collider is available
                Rectangle colliderBounds = new Rectangle(-16, -16, 32, 32);

                if (player.HasComponent<ColliderComponent>())
                {
                    ref var collider = ref player.GetComponent<ColliderComponent>();
                    colliderBounds = collider.Bounds;
                }

                var bounds = new Rectangle(
                    (int)proposedPosition.X - colliderBounds.X,
                    (int)proposedPosition.Y - colliderBounds.Y,
                    colliderBounds.Width,
                    colliderBounds.Height
                );
                var collides = false;
                foreach(var entity in world.GetEntitiesWith<ColliderComponent>())
                {
                    
                    if (player.Id == entity.Id) continue;
                    ref var collider = ref entity.GetComponent<ColliderComponent>();
                    if (!collider.IsStatic) continue;
                    ref var entTransform = ref entity.GetComponent<TransformComponent>();
                    var bottomBounds = new Rectangle(bounds.X, bounds.Y + bounds.Height - 1, colliderBounds.Width, 1);
                    var entBounds = new Rectangle((int)entTransform.Position.X - collider.Bounds.X, (int)entTransform.Position.Y - collider.Bounds.Y, collider.Bounds.Width, collider.Bounds.Height);
                    collides = entBounds.Intersects(bottomBounds);
                    if (collides) break;
                }
  
                if (!_collisionMap.IsSolid(bounds) && !collides)
                {
                    transform.Position = proposedPosition;
                }

            }
        }
    }
}
