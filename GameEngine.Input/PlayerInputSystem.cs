using GameEngine.Core.Components;
using GameEngine.Core.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameEngine.Input
{
    public class PlayerInputSystem : EngineSystem
    {
        private float _playerSpeed = 300f;
        private MouseState _previousMouseState;

        public override void Update(World world)
        {

            foreach (var entity in world.GetEntitiesWith<PlayerInputComponent>())
            {
                ref PlayerInputComponent inputComponent = ref entity.GetComponent<PlayerInputComponent>();
                inputComponent.PreviousMouseState = inputComponent.CurrentMouseState;
                inputComponent.PreviousKeyboardState = inputComponent.CurrentKeyboardState;
                inputComponent.CurrentKeyboardState = Keyboard.GetState();
                inputComponent.CurrentMouseState = Mouse.GetState();
            }
        }
    }
}