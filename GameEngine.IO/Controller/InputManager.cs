using Common.Enums;
using Common.Interfaces;
using GameEngine.Core.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameEngine.IO.Controller
{
    public class InputManager : IInputManager
    {
        private MouseState _currentMouseState;
        private MouseState _previousMouseState;
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

        public bool IsMiddleMousePressed()
        {
            return _currentMouseState.MiddleButton == ButtonState.Pressed;
        }

        public Vector2 GetMousePositionDelta()
        {
            Vector2 previousPosition = new Vector2(_previousMouseState.X, _previousMouseState.Y);
            Vector2 currentPosition = new Vector2(_currentMouseState.X, _currentMouseState.Y);
            return currentPosition - previousPosition;
        }

        public float GetScrollWheelDelta()
        {
            return _currentMouseState.ScrollWheelValue - _previousMouseState.ScrollWheelValue;
        }

        public void Update()
        {
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();
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
