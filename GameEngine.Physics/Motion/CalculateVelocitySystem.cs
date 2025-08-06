using Common.Enums;
using Common.Interfaces;
using GameEngine.Core.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameEngine.Physics.Motion
{
    public class CalculateVelocitySystem(IInputManager inputManager) : IUpdateSystem
    {
        private float _playerSpeed = 100f;
        private IInputManager _inputManager = inputManager;

        public void Update(IWorld world)
        {
            foreach (var entity in world.GetEntitiesWith<PlayerInputComponent, VelocityComponent>())
            {
                ref VelocityComponent velocity = ref entity.GetComponent<VelocityComponent>();
                ref PlayerInputComponent input = ref entity.GetComponent<PlayerInputComponent>();
                // Player Movement Logic
                velocity.Value = Vector2.Zero;
                Vector2 movement = Vector2.Zero;

                if (_inputManager.IsKeyDown(InputAction.MoveUp)) movement.Y -= 1;
                if (_inputManager.IsKeyDown(InputAction.MoveDown)) movement.Y += 1;
                if (_inputManager.IsKeyDown(InputAction.MoveLeft)) movement.X -= 1;
                if (_inputManager.IsKeyDown(InputAction.MoveRight)) movement.X += 1;

                if (movement != Vector2.Zero)
                {
                    movement.Normalize();
                    velocity.Value = movement * _playerSpeed;
                }
            }
        }
    }
}
