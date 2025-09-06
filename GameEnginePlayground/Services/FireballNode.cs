using GameEngine.Common.Events;
using GameEngine.Common.Interfaces;
using GameEngine.Common.Physics.Components;
using GameEngine.Core.Services;
using GameEngine.Gameplay.AI.Components;
using GameEngine.Gameplay.AI.Enum;
using GameEngine.Gameplay.AI.Interfaces;
using GameEnginePlayground.Factories;
using GameEnginePlayground.Factories.DataObjects;
using System;

namespace GameEnginePlayground.Services
{
    public class FireballNode : IBehaviorTreeNode
    {
        private readonly IEntity _entity;
        private readonly IFactory<IEntity, ProjectileData> _projectileFactory;
        private readonly IEventManager _eventManager;
        private readonly IWorld _world;

        /// <summary>
        /// Initializes a new instance of the MoveToTargetNode for the specified entity.
        /// </summary>
        /// <param name="entity">The entity to move towards its target.</param>
        public FireballNode(IEntity entity, IFactory<IEntity, ProjectileData> projectileFactory, IEventManager eventManager, IWorld world)
        {
            _entity = entity;
            _eventManager = eventManager ?? throw new ArgumentNullException(nameof(eventManager));
            _world = world ?? throw new ArgumentNullException(nameof(world));
            _projectileFactory = projectileFactory ?? throw new ArgumentNullException(nameof(projectileFactory));
        }

        /// <summary>
        /// Updates the entity's direction to move towards its target.
        /// Returns Success if the target is valid and movement is performed, otherwise Failure.
        /// </summary>
        /// <returns>BehaviorTreeNodeStatus.Success if movement is possible, otherwise BehaviorTreeNodeStatus.Failure.</returns>
        public BehaviorTreeNodeStatus Tick()
        {
            ref var transform = ref _entity.GetComponent<TransformComponent>();
            ref var targetComponent = ref _entity.GetComponent<TargetComponent>();
            ref DirectionComponent directionComponent = ref _entity.GetComponent<DirectionComponent>();
            if (targetComponent.Value == null || !targetComponent.Value.HasComponent<TransformComponent>())
            {
                return BehaviorTreeNodeStatus.Failure;
            }
            ref var targetTransform = ref targetComponent.Value.GetComponent<TransformComponent>();
            var direction = targetTransform.Position - transform.Position;
            direction.Normalize();
            _eventManager.Publish(new ProjectileEvent(_entity.Id, "fireball", _world, direction));
            return BehaviorTreeNodeStatus.Success;
        }
    }
}
