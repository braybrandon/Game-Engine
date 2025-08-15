using Common.Interfaces;
using Common.Physics.Components;
using Microsoft.Xna.Framework;

namespace GameEngine.Physics.CollisionDetection
{
    public class CollisionSystem : IUpdateSystem
    {
        private readonly ICollisionMap _collisionMap;
        public CollisionSystem(ICollisionMap collisionMap)
        {
            _collisionMap = collisionMap;
        }
        public void Update(IWorld world)
        {
            foreach (var movingEntity in world.GetEntitiesWith<TransformComponent, ProposedPositionComponent, ColliderComponent>())
            {
                ref var transform = ref movingEntity.GetComponent<TransformComponent>();
                ref var proposedPosition = ref movingEntity.GetComponent<ProposedPositionComponent>();
                ref var collider = ref movingEntity.GetComponent<ColliderComponent>();


                Rectangle colliderBounds = collider.Bounds;
                var bounds = new Rectangle(
                (int)proposedPosition.Value.X - colliderBounds.X,
                (int)proposedPosition.Value.Y - colliderBounds.Y,
                colliderBounds.Width,
                colliderBounds.Height
                );

                var collides = false;
                foreach (var entity in world.GetEntitiesWith<ColliderComponent>())
                {

                    if (movingEntity.Id == entity.Id) continue;
                    ref var entityCollider = ref entity.GetComponent<ColliderComponent>();
                    if (!entityCollider.IsStatic) continue;
                    ref var entTransform = ref entity.GetComponent<TransformComponent>();
                    var bottomBounds = new Rectangle(bounds.X, bounds.Y + bounds.Height - 1, colliderBounds.Width, 1);
                    var entBounds = new Rectangle((int)entTransform.Position.X - entityCollider.Bounds.X, (int)entTransform.Position.Y - entityCollider.Bounds.Y, entityCollider.Bounds.Width, entityCollider.Bounds.Height);
                    collides = entBounds.Intersects(bottomBounds);
                    if (collides) break;
                }

                if (!_collisionMap.IsSolid(bounds) && !collides)
                {
                    transform.Position = proposedPosition.Value;
                }
            }


            }
    }
}
