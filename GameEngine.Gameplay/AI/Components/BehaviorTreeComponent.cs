using GameEngine.Common.Interfaces;
using GameEngine.Gameplay.AI.Interfaces;

namespace GameEngine.Gameplay.AI.Components
{
    /// <summary>
    /// Component that stores the root node of an entity's behavior tree for AI decision-making.
    /// Used to associate a behavior tree with an entity, enabling complex AI logic and state management.
    /// </summary>
    public struct BehaviorTreeComponent : IComponent
    {
        /// <summary>
        /// Gets the root node of the behavior tree associated with this entity.
        /// </summary>
        public IBehaviorTreeNode RootNode { get; private set; }

        /// <summary>
        /// Initializes a new instance of the BehaviorTreeComponent with the specified root node.
        /// </summary>
        /// <param name="rootNode">The root node of the behavior tree.</param>
        public BehaviorTreeComponent(IBehaviorTreeNode rootNode)
        {
            RootNode = rootNode;
        }
    }
}
