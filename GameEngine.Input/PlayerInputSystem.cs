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

        public PlayerInputSystem(Game game) : base(game)
        {
        }

        public override void Update(GameTime gameTime)
        {

            foreach (var entity in ComponentManager.GetEntitiesWith<PlayerInputComponent>())
            {
                PlayerInputComponent inputComponent = ComponentManager.GetComponent<PlayerInputComponent>(entity);
                inputComponent.PreviousMouseState = inputComponent.CurrentMouseState;
                inputComponent.PreviousKeyboardState = inputComponent.CurrentKeyboardState;
                inputComponent.CurrentKeyboardState = Keyboard.GetState();
                inputComponent.CurrentMouseState = Mouse.GetState();
                ComponentManager.AddComponent(entity, inputComponent);
            }
        }
    }
}
