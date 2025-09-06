using GameEngine.Common.Components;
using GameEngine.Common.Interfaces;
using GameEngine.Common.Physics;
using GameEngine.Common.Physics.Components;
using GameEngine.Common.Physics.Interfaces;
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
                    ref TransformComponent entTransform = ref entity.GetComponent<TransformComponent>();
                    if (!entityCollider.IsStatic)
                    {
                      
                        var entBounds = new Rectangle(
                            (int)entTransform.Position.X - entityCollider.Bounds.X,
                            (int)entTransform.Position.Y - entityCollider.Bounds.Y,
                            entityCollider.Bounds.Width,
                            entityCollider.Bounds.Height
                        );
                        if (proposedBounds.Intersects(entBounds))
                        {
                            if (!CollisionFilters.ShouldCollide(collider.Filter, entityCollider.Filter))
                                continue;

                            // projectiles vs anything they’re allowed to hit
                            bool aProj = CollisionFilters.InCat(collider.Filter, CollisionCategory.Projectile);
                            bool bProj = CollisionFilters.InCat(entityCollider.Filter, CollisionCategory.Projectile);
                            if (aProj || bProj)
                            {
                                var proj = aProj ? movingEntity : entity;
                                var target = aProj ? entity : movingEntity;
                                var projBounds = aProj ? transformBounds : entBounds;
                                // NEW: skip pairs that aren't allowed to collide

                                CalculateDamage(target, world, entBounds);
                                _quadTree.Remove(proj, projBounds);
                                // Mark the entity for removal
                                world.DestroyEntity(proj); // or whatever method you use to remove entities
                                collides = true;
                            }

                        }
                        continue;
                    }
                    else
                    {

                        var bottomBounds = new Rectangle(
                            proposedBounds.X,
                            proposedBounds.Y,
                            colliderBounds.Width,
                            colliderBounds.Height
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
                }

                if (!_collisionMap.IsSolid(proposedBounds) && !collides)
                {
                    transform.Position = proposedPosition.Value;
                    _quadTree.Remove(movingEntity, transformBounds);
                    _quadTree.Insert(movingEntity, proposedBounds);
                }
            }
        }

        private bool CalculateDamage(IEntity entity, IWorld world, Rectangle bounds)
        {
            if (entity.HasComponent<HealthComponent>())
            {
                ref var health = ref entity.GetComponent<HealthComponent>();
                health.CurrentHealth -= 10; // or whatever damage value you want
                if(health.CurrentHealth <= 0)
                {
                    _quadTree.Remove(entity, bounds);
                    // Mark the entity for removal
                    world.DestroyEntity(entity);
                    return true;
                }

            }
            return false;
        }
    }
}
