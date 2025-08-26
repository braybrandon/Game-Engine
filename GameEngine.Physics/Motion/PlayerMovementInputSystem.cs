using GameEngine.Common.Enums;
using GameEngine.Common.Interfaces;
using GameEngine.Common.IO.Components;
using GameEngine.Common.Physics.Components;
using Microsoft.Xna.Framework;

namespace GameEngine.Physics.Motion
{
    /// <summary>
    /// System that processes player movement input and updates the direction component of entities accordingly.
    /// Uses the input manager to query movement keys and normalizes movement vectors for consistent speed.
    /// </summary>
    public class PlayerMovementInputSystem : IUpdateSystem
    {
        private IInputManager _inputManager;

        /// <summary>
        /// Initializes a new instance of the PlayerMovementInputSystem with the specified input manager.
        /// </summary>
        /// <param name="inputManager">The input manager used to query key states.</param>
        public PlayerMovementInputSystem(IInputManager inputManager)
        {
            _inputManager = inputManager;
        }

        /// <summary>
        /// Updates the direction of entities with PlayerInputComponent and DirectionComponent based on input.
        /// Normalizes the movement vector to ensure consistent movement speed in all directions.
        /// </summary>
        /// <param name="world">The world containing entities and components.</param>
        public void Update(IWorld world)
        {
            foreach (var entity in world.GetEntitiesWith<PlayerInputComponent, DirectionComponent>())
            {
                ref DirectionComponent direction = ref entity.GetComponent<DirectionComponent>();
                ref PlayerInputComponent input = ref entity.GetComponent<PlayerInputComponent>();

                direction.Value = Vector2.Zero;
                Vector2 movement = Vector2.Zero;

                if (_inputManager.IsKeyDown(input.MovementKeys[InputAction.MoveUp])) movement.Y -= 1;
                if (_inputManager.IsKeyDown(input.MovementKeys[InputAction.MoveDown])) movement.Y += 1;
                if (_inputManager.IsKeyDown(input.MovementKeys[InputAction.MoveLeft])) movement.X -= 1;
                if (_inputManager.IsKeyDown(input.MovementKeys[InputAction.MoveRight])) movement.X += 1;

                if (movement != Vector2.Zero)
                {
                    movement.Normalize();
                }
                direction.Value = movement;
            }
        }
    }
}
