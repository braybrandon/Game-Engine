using GameEngine.Core.Components;
using GameEngine.Core.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameEngine.Physics
{
    public class CalculateVelocitySystem : EngineSystem
    {
        private float _playerSpeed = 300f;
        public CalculateVelocitySystem(Game game) : base(game)
        {
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entity in ComponentManager.GetEntitiesWith<PlayerInputComponent, VelocityComponent>())
            {
                VelocityComponent velocity = ComponentManager.GetComponent<VelocityComponent>(entity);
                PlayerInputComponent input = ComponentManager.GetComponent<PlayerInputComponent>(entity);
                // Player Movement Logic
                velocity.Value = Vector2.Zero;
                Vector2 movement = Vector2.Zero;

                if (input.CurrentKeyboardState.IsKeyDown(Keys.W)) movement.Y -= 1;
                if (input.CurrentKeyboardState.IsKeyDown(Keys.S)) movement.Y += 1;
                if (input.CurrentKeyboardState.IsKeyDown(Keys.A)) movement.X -= 1;
                if (input.CurrentKeyboardState.IsKeyDown(Keys.D)) movement.X += 1;

                if (movement != Vector2.Zero)
                {
                    movement.Normalize();
                    velocity.Value = movement * _playerSpeed;
                }

                ComponentManager.AddComponent(entity, velocity);
            }
        }
    }
}
