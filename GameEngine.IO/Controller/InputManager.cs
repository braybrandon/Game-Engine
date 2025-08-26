using GameEngine.Common.Enums;
using GameEngine.Common.Events;
using GameEngine.Common.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameEngine.IO.Controller
{
    /// <summary>
    /// Manages input state for mouse and keyboard, and publishes input events to the event manager.
    /// Implements IInputManager to provide input queries and update logic for the game engine.
    /// </summary>
    public class InputManager : IInputManager
    {
        private MouseState _currentMouseState;
        private MouseState _previousMouseState;
        private KeyboardState _currentKeyboardState;
        private KeyboardState _previousKeyboardState;
        private IEventManager _eventManager;

        /// <summary>
        /// Initializes a new instance of the InputManager class with the specified event manager.
        /// </summary>
        /// <param name="manager">The event manager used to publish input events.</param>
        public InputManager(IEventManager manager)
        {
            _eventManager = manager;
        }

        /// <summary>
        /// Checks if the specified key is currently held down.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>True if the key is down; otherwise, false.</returns>
        public bool IsKeyDown(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Checks if the specified key was pressed during the current update.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>True if the key was pressed; otherwise, false.</returns>
        public bool IsKeyPressed(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key) && !_previousKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Checks if the middle mouse button is currently pressed.
        /// </summary>
        /// <returns>True if the middle mouse button is pressed; otherwise, false.</returns>
        public bool IsMiddleMousePressed()
        {
            return _currentMouseState.MiddleButton == ButtonState.Pressed;
        }

        /// <summary>
        /// Gets the current mouse position in screen coordinates.
        /// </summary>
        /// <returns>The mouse position as a <see cref="Vector2"/>.</returns>
        public Vector2 GetMousePosition()
        {
            return new Vector2(_currentMouseState.X, _currentMouseState.Y);
        }

        /// <summary>
        /// Gets the change in mouse position since the last update.
        /// </summary>
        /// <returns>The mouse position delta as a <see cref="Vector2"/>.</returns>
        public Vector2 GetMousePositionDelta()
        {
            Vector2 previousPosition = new Vector2(_previousMouseState.X, _previousMouseState.Y);
            Vector2 currentPosition = new Vector2(_currentMouseState.X, _currentMouseState.Y);
            return currentPosition - previousPosition;
        }

        /// <summary>
        /// Gets the change in the scroll wheel value since the last update.
        /// </summary>
        /// <returns>The delta value of the scroll wheel.</returns>
        public float GetScrollWheelDelta()
        {
            return _currentMouseState.ScrollWheelValue - _previousMouseState.ScrollWheelValue;
        }

        /// <summary>
        /// Updates the input manager's internal state and publishes mouse click events.
        /// Should be called once per frame.
        /// </summary>
        public void Update()
        {
            _previousMouseState = _currentMouseState;
            _previousKeyboardState = _currentKeyboardState;
            _currentMouseState = Mouse.GetState();
            _currentKeyboardState = Keyboard.GetState();
            if(_currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton != ButtonState.Pressed)
            {
                var mouseX = _currentMouseState.X;
                var mouseY = _currentMouseState.Y;
                _eventManager.Publish(new MouseClickEvent(mouseX, mouseY));
            }
        }
    }
}
