using GameEngine.Core.Components;
using GameEngine.Core.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameEngine.Physics
{
    public class CalculateVelocitySystem : EngineSystem
    {
        private float _playerSpeed = 300f;

        public override void Update(World world)
        {
            foreach (var entity in world.GetEntitiesWith<PlayerInputComponent, VelocityComponent>())
            {
                ref VelocityComponent velocity = ref entity.GetComponent<VelocityComponent>();
                ref PlayerInputComponent input = ref entity.GetComponent<PlayerInputComponent>();
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
            }
        }
    }
}
