using Common.Interfaces;
using Common.Physics.Components;
using Common.Physics.Interfaces;
using Microsoft.Xna.Framework;

namespace GameEngine.Physics.CollisionDetection
{
    public class CollisionSystem : IUpdateSystem
    {
        private readonly ICollisionMap _collisionMap;
        private IQuadTree _quadTree;

        public CollisionSystem(ICollisionMap collisionMap, IQuadTree quadTree)
        {
            _collisionMap = collisionMap;
            _quadTree = quadTree;
        }

        public void Update(IWorld world)
        {
            var movingEntities = world.GetEntitiesWith<TransformComponent, ProposedPositionComponent, ColliderComponent>();

            foreach (var movingEntity in movingEntities)
            {
                ref var transform = ref movingEntity.GetComponent<TransformComponent>();
                ref var proposedPosition = ref movingEntity.GetComponent<ProposedPositionComponent>();
                ref var collider = ref movingEntity.GetComponent<ColliderComponent>();

                Rectangle colliderBounds = collider.Bounds;
                var proposedBounds = new Rectangle(
                    (int)proposedPosition.Value.X - colliderBounds.X,
                    (int)proposedPosition.Value.Y - colliderBounds.Y,
                    colliderBounds.Width,
                    colliderBounds.Height
                );
                var transformBounds = new Rectangle(
                     (int)transform.Position.X - colliderBounds.X,
                     (int)transform.Position.Y - colliderBounds.Y,
                     colliderBounds.Width,
                     colliderBounds.Height
                 );

                List<IEntity> nearbyEntities = new List<IEntity>();
                _quadTree.Retrieve(nearbyEntities, proposedBounds);

                var collides = false;
                foreach (var entity in nearbyEntities)
                {

                    if (movingEntity.Id == entity.Id) continue;
                    ref ColliderComponent entityCollider = ref entity.GetComponent<ColliderComponent>();
                    if (!entityCollider.IsStatic) continue;
                    ref TransformComponent entTransform = ref entity.GetComponent<TransformComponent>();
                    var bottomBounds = new Rectangle(
                        proposedBounds.X,
                        proposedBounds.Y + proposedBounds.Height - 1,
                        colliderBounds.Width,
                        1
                    );
                    var entBounds = new Rectangle(
                        (int)entTransform.Position.X - entityCollider.Bounds.X,
                        (int)entTransform.Position.Y - entityCollider.Bounds.Y,
                        entityCollider.Bounds.Width,
                        entityCollider.Bounds.Height
                    );
                    collides = entBounds.Intersects(bottomBounds);
                    if (collides) break;
                }

                if (!_collisionMap.IsSolid(proposedBounds) && !collides)
                {
                    transform.Position = proposedPosition.Value; 
                    _quadTree.Remove(movingEntity, transformBounds);
                    _quadTree.Insert(movingEntity, proposedBounds);
                }
            }
        }
    }
}
