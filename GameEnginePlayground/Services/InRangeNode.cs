using GameEngine.Common.Interfaces;
using GameEngine.Common.Physics.Components;
using GameEngine.Gameplay.AI.Components;
using GameEngine.Gameplay.AI.Enum;
using GameEngine.Gameplay.AI.Interfaces;
using Microsoft.Xna.Framework;


namespace GameEnginePlayground.Services
{
    public class InRangeNode : IBehaviorTreeNode
    {
        private readonly IEntity _entity;

        /// <summary>
        /// Initializes a new instance of the MoveToTargetNode for the specified entity.
        /// </summary>
        /// <param name="entity">The entity to move towards its target.</param>
        public InRangeNode(IEntity entity)
        {
            _entity = entity;
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
            var distance = Vector2.Distance(transform.Position, targetTransform.Position);
            if (distance < 150)
            {
                return BehaviorTreeNodeStatus.Success;
            }
            return BehaviorTreeNodeStatus.Failure;
        }
    }
}
