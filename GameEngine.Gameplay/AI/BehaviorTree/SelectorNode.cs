using GameEngine.Common.Interfaces;
using GameEngine.Gameplay.AI.Enum;
using GameEngine.Gameplay.AI.Interfaces;
using System.Collections.Generic;

namespace GameEngine.Gameplay.AI.BehaviorTree
{
    /// <summary>
    /// Behavior tree node that evaluates its child nodes in order and returns the first non-failure status.
    /// Returns Success if any child succeeds, Running if any child is running, otherwise Failure.
    /// </summary>
    public class SelectorNode : IBehaviorTreeNode
    {
        private readonly IEntity _entity;
        private readonly List<IBehaviorTreeNode> _children = new List<IBehaviorTreeNode>();

        /// <summary>
        /// Initializes a new instance of the SelectorNode with the specified entity and child nodes.
        /// </summary>
        /// <param name="entity">The entity associated with this node.</param>
        /// <param name="children">The child behavior tree nodes to evaluate.</param>
        public SelectorNode(IEntity entity, params IBehaviorTreeNode[] children)
        {
            _entity = entity;
            _children.AddRange(children);
        }

        /// <summary>
        /// Evaluates child nodes in order and returns the first non-failure status.
        /// </summary>
        /// <returns>
        /// Success if any child returns Success, Running if any child returns Running, otherwise Failure.
        /// </returns>
        public BehaviorTreeNodeStatus Tick()
        {
            foreach (var child in _children)
            {
                var status = child.Tick();
                if (status == BehaviorTreeNodeStatus.Success)
                {
                    return BehaviorTreeNodeStatus.Success;
                }
                if (status == BehaviorTreeNodeStatus.Running)
                {
                    return BehaviorTreeNodeStatus.Running;
                }
            }
            return BehaviorTreeNodeStatus.Failure;
        }
    }
}
