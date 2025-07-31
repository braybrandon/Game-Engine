using Common.Interfaces;
using GameEngine.Core.Components;
using Microsoft.Xna.Framework.Input;

namespace GameEngine.IO.Controller
{
    public class PlayerInputSystem : IUpdateSystem
    {
        private float _playerSpeed = 300f;
        private MouseState _previousMouseState;

        public void Update(IWorld world)
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