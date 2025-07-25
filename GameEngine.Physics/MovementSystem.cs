using GameEngine.Core.Components;
using GameEngine.Core.Systems;
using Microsoft.Xna.Framework;

namespace GameEngine.Physics
{
    public class MovementSystem : EngineSystem
    {
        public MovementSystem(Game game) : base(game)
        {
        }

        public override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (var entity in ComponentManager.GetEntitiesWith<TransformComponent, VelocityComponent>())
            {
                // Get copies of the components
                var velocity = ComponentManager.GetComponent<VelocityComponent>(entity);
                var transform = ComponentManager.GetComponent<TransformComponent>(entity);  

                transform.Position += velocity.Value * dt;

                // Add the modified position component back to update it
                ComponentManager.AddComponent(entity, transform);
            }
        }
    }
}
