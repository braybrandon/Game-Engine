using GameEngine.Common.Interfaces;
using GameEngine.Common.Physics.Components;
using GameEngine.Gameplay.AI.Components;
using GameEngine.Gameplay.AI.Enum;
using GameEngine.Gameplay.AI.Interfaces;
using Microsoft.Xna.Framework;

namespace GameEngine.Gameplay.AI.BehaviorTree
{
    /// <summary>
    /// Behavior tree node that moves the entity towards its target by updating its direction.
    /// Returns Success if movement is possible, otherwise returns Failure.
    /// </summary>
    public class MoveToTargetNode : IBehaviorTreeNode
    {
        private readonly IEntity _entity;

        /// <summary>
        /// Initializes a new instance of the MoveToTargetNode for the specified entity.
        /// </summary>
        /// <param name="entity">The entity to move towards its target.</param>
        public MoveToTargetNode(IEntity entity)
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
            Vector2 direction = targetTransform.Position - transform.Position;
            direction.Normalize();
            directionComponent.Value = direction;
            return BehaviorTreeNodeStatus.Success;
        }
    }
}
