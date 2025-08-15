using Common.Enums;
using Common.Interfaces;
using Common.IO.Components;
using Common.Physics.Components;
using Microsoft.Xna.Framework;

namespace GameEngine.Physics.Motion
{
    public class PlayerMovementInputSystem(IInputManager inputManager) : IUpdateSystem
    {
        private float _playerSpeed = 100f;
        private IInputManager _inputManager = inputManager;

        public void Update(IWorld world)
        {
            foreach (var entity in world.GetEntitiesWith<PlayerInputComponent, DirectionComponent>())
            {
                ref DirectionComponent direction = ref entity.GetComponent<DirectionComponent>();

                direction.Value = Vector2.Zero;
                Vector2 movement = Vector2.Zero;

                if (_inputManager.IsKeyDown(InputAction.MoveUp)) movement.Y -= 1;
                if (_inputManager.IsKeyDown(InputAction.MoveDown)) movement.Y += 1;
                if (_inputManager.IsKeyDown(InputAction.MoveLeft)) movement.X -= 1;
                if (_inputManager.IsKeyDown(InputAction.MoveRight)) movement.X += 1;

                if (movement != Vector2.Zero)
                {
                    movement.Normalize();
                }
                direction.Value = movement;
            }
        }
    }
}
