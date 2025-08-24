using Common.Interfaces;
using Common.IO.Components;
using Common.Physics.Components;
using GameEngine.Engine.AI.Components;
using Microsoft.Xna.Framework;
using System.Linq;

namespace GameEngine.Engine.AI.Systems
{
    /// <summary>
    /// System that assigns player entities as targets to AI entities when within a certain distance.
    /// </summary>
    public class TargetingSystem : IUpdateSystem
    {
        /// <summary>
        /// Updates the targeting logic for AI entities, assigning the nearest player entity as a target if within range.
        /// </summary>
        /// <param name="world">The world containing entities and components.</param>
        public void Update(IWorld world)
        {
            foreach (var aiEntity in world.GetEntitiesWith<AIComponent, TargetComponent, PerceptionComponent, TransformComponent>())
            {
                ref var aiTransform = ref aiEntity.GetComponent<TransformComponent>();
                ref var perception = ref aiEntity.GetComponent<PerceptionComponent>();
                var player = world.GetEntitiesWith<PlayerInputComponent, TransformComponent>().FirstOrDefault();
                ref var playerTransform = ref player.GetComponent<TransformComponent>();
                if (Vector2.Distance(aiTransform.Position, playerTransform.Position) < perception.Radius)
                {
                    ref var target = ref aiEntity.GetComponent<TargetComponent>();
                    target.Value = player;
                }
            }
        }
    }
}
