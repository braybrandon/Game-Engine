using GameEngine.Common.Interfaces;
using GameEngine.Gameplay.AI.Components;

namespace GameEngine.Gameplay.AI.Systems
{
    /// <summary>
    /// System that updates behavior trees for AI entities, allowing them to make decisions and perform actions each frame.
    /// </summary>
    public class BehaviorTreeSystem : IUpdateSystem
    {
        /// <summary>
        /// Updates all AI entities with behavior trees by ticking their root node.
        /// </summary>
        /// <param name="world">The world containing entities and components.</param>
        public void Update(IWorld world)
        {
            foreach (var aiEntity in world.GetEntitiesWith<AIComponent, BehaviorTreeComponent>())
            {
                var btComponent = aiEntity.GetComponent<BehaviorTreeComponent>();
                btComponent.RootNode.Tick();
            }
        }
    }
}
