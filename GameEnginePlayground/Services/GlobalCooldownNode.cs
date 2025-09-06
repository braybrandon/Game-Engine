using GameEngine.Gameplay.AI.Enum;
using GameEngine.Gameplay.AI.Interfaces;
using System;

namespace GameEnginePlayground.Services
{
    public class GlobalCooldownNode : IBehaviorTreeNode
    {
        private readonly float _cooldownSeconds;
        private DateTime _lastExecutionTime;

        public GlobalCooldownNode(float cooldownSeconds)
        {
            _cooldownSeconds = cooldownSeconds;
            _lastExecutionTime = DateTime.MinValue; // Initialize to a very old time
        }

        public BehaviorTreeNodeStatus Tick()
        {
            var elapsed = DateTime.Now - _lastExecutionTime;

            if (elapsed.TotalSeconds >= _cooldownSeconds)
            {
                // Cooldown has passed, allow execution and reset the timer
                _lastExecutionTime = DateTime.Now;
                return BehaviorTreeNodeStatus.Success;
            }

            // Still on cooldown
            return BehaviorTreeNodeStatus.Running;
        }
    }
}
