using Common.Enums;
using Common.Events;
using Common.Interfaces;
using GameEngine.Core.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameEngine.IO.Controller
{
    public class InputManager(IEventManager manager) : IInputManager
    {
        private MouseState _currentMouseState;
        private MouseState _previousMouseState;
        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;
        private Dictionary<InputAction, Keys> _movementKeys = new Dictionary<InputAction, Keys>();
        private Dictionary<Keys, Action> _keyDownBinds = new Dictionary<Keys, Action>();
        private IEventManager _eventManager = manager;

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

        public Vector2 GetMousePosition()
        {
            return new Vector2(_currentMouseState.X, _currentMouseState.Y);
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
            _previousKeyboardState = _currentKeyboardState;
            _currentMouseState = Mouse.GetState();
            _currentKeyboardState = Keyboard.GetState();

            // Handle Events

            // set previous states to current states
            foreach (Keys key in _keyDownBinds.Keys)
            {
                if (_currentKeyboardState.IsKeyDown(key) && !_previousKeyboardState.IsKeyDown(key))
                {
                        _keyDownBinds[key].Invoke();
                   
                }
            }
            if(_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton != ButtonState.Pressed)
            {
                var mouseX = _currentMouseState.X;
                var mouseY = _currentMouseState.Y;
                _eventManager.Publish(new MouseClickEvent(mouseX, mouseY));
            }
        }
    }
}
