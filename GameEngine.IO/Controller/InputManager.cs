using Common.Enums;
using Common.Interfaces;
using Microsoft.Xna.Framework.Input;

namespace GameEngine.IO.Controller
{
    public class InputManager : IInputManager
    {
        private KeyboardState _currentKeyboardState;
        private Dictionary<InputAction, Keys> _movementKeys = new Dictionary<InputAction, Keys>();
        private Dictionary<Keys, Action> _keyDownBinds = new Dictionary<Keys, Action>();

        public void BindKeyDown(Keys key, Action action)
        {
            _keyDownBinds[key] = action;
        }

        public void BindMovementKeys(Keys key, InputAction action)
        {
            _movementKeys[action] = key;
        }

        public bool IsKeyDown(InputAction action)
        {
            if(_movementKeys.ContainsKey(action))
            {
                return _currentKeyboardState.IsKeyDown(_movementKeys[action]);
            }
            
            return false;
        }

        public void Update()
        {
            MouseState currentMouseState = Mouse.GetState();
            _currentKeyboardState = Keyboard.GetState();

            // Handle Events

            // set previous states to current states
            foreach (Keys key in _keyDownBinds.Keys)
            {
                if (_currentKeyboardState.IsKeyDown(key))
                {
                        _keyDownBinds[key].Invoke();
                   
                }
            }
        }
    }
}
