namespace GameEngine.Gameplay.AI.Enum
{
    /// <summary>
    /// Represents the possible statuses returned by behavior tree nodes during evaluation.
    /// </summary>
    public enum BehaviorTreeNodeStatus
    {
        /// <summary>
        /// Indicates the node has completed successfully.
        /// </summary>
        Success,
        /// <summary>
        /// Indicates the node has failed.
        /// </summary>
        Failure,
        /// <summary>
        /// Indicates the node is still running and has not completed.
        /// </summary>
        Running
    }
}
