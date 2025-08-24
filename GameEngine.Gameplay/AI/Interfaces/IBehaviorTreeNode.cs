using GameEngine.Gameplay.AI.Enum;

namespace GameEngine.Gameplay.AI.Interfaces
{
    /// <summary>
    /// Defines the interface for a behavior tree node, which can be evaluated to determine its status.
    /// </summary>
    public interface IBehaviorTreeNode
    {
        /// <summary>
        /// Evaluates the node and returns its current status (Success, Failure, or Running).
        /// </summary>
        /// <returns>The status of the node after evaluation.</returns>
        BehaviorTreeNodeStatus Tick();
    }
}
