using GameEngine.Common.Interfaces;
using GameEngine.Gameplay.AI.Components;
using GameEngine.Gameplay.AI.Enum;
using GameEngine.Gameplay.AI.Interfaces;

namespace GameEngine.Gameplay.AI.BehaviorTree
{
    /// <summary>
    /// Behavior tree node that checks if the entity has a valid target assigned.
    /// Returns Success if the entity's TargetComponent is set; otherwise, returns Failure.
    /// </summary>
    public class HasTargetNode : IBehaviorTreeNode
    {
        private readonly IEntity _entity;

        /// <summary>
        /// Initializes a new instance of the HasTargetNode for the specified entity.
        /// </summary>
        /// <param name="entity">The entity to check for a target.</param>
        public HasTargetNode(IEntity entity)
        {
            _entity = entity;
        }

        /// <summary>
        /// Evaluates whether the entity has a valid target.
        /// </summary>
        /// <returns>Success if the entity has a target; otherwise, Failure.</returns>
        public BehaviorTreeNodeStatus Tick()
        {
            if (_entity.HasComponent<TargetComponent>())
            {
                ref var targetComponent = ref _entity.GetComponent<TargetComponent>();
                if (targetComponent.Value != null)
                {
                    return BehaviorTreeNodeStatus.Success;
                }
            }
            return BehaviorTreeNodeStatus.Failure;
        }
    }
}
