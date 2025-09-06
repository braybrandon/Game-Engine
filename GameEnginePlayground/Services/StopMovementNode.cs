using GameEngine.Common.Interfaces;
using GameEngine.Common.Physics.Components;
using GameEngine.Gameplay.AI.Enum;
using GameEngine.Gameplay.AI.Interfaces;
using Microsoft.Xna.Framework;

namespace GameEnginePlayground.Services
{
    public class StopMovementNode : IBehaviorTreeNode
    {
        private readonly IEntity _entity;

        public StopMovementNode(IEntity entity)
        {
            _entity = entity;
        }

        public BehaviorTreeNodeStatus Tick()
        {
            // Check if the entity has the component responsible for movement
            if (_entity.HasComponent<DirectionComponent>())
            {
                // Get the component and set the movement vector to zero
                ref var directionComponent = ref _entity.GetComponent<DirectionComponent>();
                directionComponent.Value = Vector2.Zero;
            }

            // This action always succeeds
            return BehaviorTreeNodeStatus.Success;
        }
    }
}
