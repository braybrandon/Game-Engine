using GameEngine.Common.Interfaces;
using GameEngine.Gameplay.AI.Enum;
using GameEngine.Gameplay.AI.Interfaces;
using System.Collections.Generic;

namespace GameEngine.Gameplay.AI.BehaviorTree
{
    /// <summary>
    /// Behavior tree node that evaluates its child nodes in order and returns Failure if any child fails.
    /// Returns Running if any child is running, otherwise returns Success if all children succeed.
    /// </summary>
    public class SequenceNode : IBehaviorTreeNode
    {
        private readonly IEntity _entity;
        private readonly List<IBehaviorTreeNode> _children = new List<IBehaviorTreeNode>();

        /// <summary>
        /// Initializes a new instance of the SequenceNode with the specified entity and child nodes.
        /// </summary>
        /// <param name="entity">The entity associated with this node.</param>
        /// <param name="children">The child behavior tree nodes to evaluate.</param>
        public SequenceNode(IEntity entity, params IBehaviorTreeNode[] children)
        {
            _entity = entity;
            _children.AddRange(children);
        }

        /// <summary>
        /// Evaluates child nodes in order and returns Failure if any child fails, Running if any child is running, otherwise Success.
        /// </summary>
        /// <returns>
        /// Failure if any child returns Failure, Running if any child returns Running, otherwise Success.
        /// </returns>
        public BehaviorTreeNodeStatus Tick()
        {
            foreach (var child in _children)
            {
                var status = child.Tick();
                if (status == BehaviorTreeNodeStatus.Failure)
                {
                    return BehaviorTreeNodeStatus.Failure;
                }
                if (status == BehaviorTreeNodeStatus.Running)
                {
                    return BehaviorTreeNodeStatus.Running;
                }
            }
            return BehaviorTreeNodeStatus.Success;
        }
    }
}
