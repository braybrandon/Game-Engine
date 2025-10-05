using GameEngine.Common.Components;
using GameEngine.Common.Interfaces;
using GameEngine.Common.Physics;
using GameEngine.Common.Physics.Components;
using GameEngine.Common.Physics.Interfaces;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace GameEngine.Physics.CollisionDetection
{
    public class CollisionSystem : IUpdateSystem
    {
        private readonly ICollisionMap _collisionMap;
        private readonly IQuadTree _quadTree;

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
                var proposedBounds = CreateBounds(proposedPosition.Value, colliderBounds);
                var transformBounds = CreateBounds(transform.Position, colliderBounds);

                List<IEntity> nearbyEntities = new List<IEntity>();
                _quadTree.Retrieve(nearbyEntities, proposedBounds);

                var collides = false;
                foreach (var entity in nearbyEntities)
                {
                    if (movingEntity.Id == entity.Id) continue;
                    ref ColliderComponent entityCollider = ref entity.GetComponent<ColliderComponent>();
                    ref TransformComponent entTransform = ref entity.GetComponent<TransformComponent>();
                    var entBounds = CreateBounds(entTransform.Position, entityCollider.Bounds);

                    if (!CollisionFilters.ShouldCollide(collider.Filter, entityCollider.Filter))
                        continue;

                    // Handle projectile collision event/response
                    if (HandleProjectileCollision(movingEntity, entity, collider, entityCollider, transformBounds, entBounds, world))
                    {
                        collides = true;
                        continue;
                    }

                    if (!entityCollider.IsStatic)
                    {
                        if (proposedBounds.Intersects(entBounds))
                        {
                            collides = true;
                        }
                    }
                    else
                    {
                        var bottomBounds = proposedBounds;
                        if (entBounds.Intersects(bottomBounds))
                        {
                            collides = true;
                            break;
                        }
                    }
                }

                // Resolve collision (update position/quadtree if not solid and not colliding)
                ResolveCollision(movingEntity, ref transform, ref proposedPosition, ref collider, transformBounds, proposedBounds, collides);
            }
        }

        // Removed duplicate HandleCollision method, only HandleProjectileCollision remains
        private bool HandleProjectileCollision(
            IEntity a,
            IEntity b,
            ColliderComponent aCollider,
            ColliderComponent bCollider,
            Rectangle aBounds,
            Rectangle bBounds,
            IWorld world)
        {
            bool aProj = CollisionFilters.InCat(aCollider.Filter, CollisionCategory.Projectile);
            bool bProj = CollisionFilters.InCat(bCollider.Filter, CollisionCategory.Projectile);
            if (!(aProj || bProj))
                return false;

            var proj = aProj ? a : b;
            var target = aProj ? b : a;
            var projBounds = aProj ? aBounds : bBounds;
            var targetBounds = aProj ? bBounds : aBounds;

            CalculateDamage(target, world, targetBounds);
            _quadTree.Remove(proj, projBounds);
            world.DestroyEntity(proj);
            return true;
        }

        private bool CalculateDamage(IEntity entity, IWorld world, Rectangle bounds)
        {
            if (entity.HasComponent<HealthComponent>())
            {
                ref var health = ref entity.GetComponent<HealthComponent>();
                health.CurrentHealth -= 10; // or whatever damage value you want
                if (health.CurrentHealth <= 0)
                {
                    _quadTree.Remove(entity, bounds);
                    world.DestroyEntity(entity);
                    return true;
                }
            }
            return false;
        }

        private void ResolveCollision(
            IEntity movingEntity,
            ref TransformComponent transform,
            ref ProposedPositionComponent proposedPosition,
            ref ColliderComponent collider,
            Rectangle transformBounds,
            Rectangle proposedBounds,
            bool collides)
        {
            if (!_collisionMap.IsSolid(proposedBounds) && !collides)
            {
                transform.Position = proposedPosition.Value;
                _quadTree.Remove(movingEntity, transformBounds);
                _quadTree.Insert(movingEntity, proposedBounds);
            }
        }

        private static Rectangle CreateBounds(Vector2 position, Rectangle bounds)
        {
            return new Rectangle(
                (int)position.X - bounds.X,
                (int)position.Y - bounds.Y,
                bounds.Width,
                bounds.Height
            );
        }
    }
}
