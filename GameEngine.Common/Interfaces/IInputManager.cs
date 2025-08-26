using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameEngine.Common.Interfaces
{
    /// <summary>
    /// Provides an interface for input management, including mouse and keyboard state queries and updates.
    /// </summary>
    public interface IInputManager
    {
        /// <summary>
        /// Gets the change in the scroll wheel value since the last update.
        /// </summary>
        /// <returns>The delta value of the scroll wheel.</returns>
        float GetScrollWheelDelta();

        /// <summary>
        /// Gets the current mouse position in screen coordinates.
        /// </summary>
        /// <returns>The mouse position as a <see cref="Vector2"/>.</returns>
        Vector2 GetMousePosition();

        /// <summary>
        /// Gets the change in mouse position since the last update.
        /// </summary>
        /// <returns>The mouse position delta as a <see cref="Vector2"/>.</returns>
        Vector2 GetMousePositionDelta();

        /// <summary>
        /// Checks if the middle mouse button is currently pressed.
        /// </summary>
        /// <returns>True if the middle mouse button is pressed; otherwise, false.</returns>
        bool IsMiddleMousePressed();

        /// <summary>
        /// Checks if the specified key is currently held down.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>True if the key is down; otherwise, false.</returns>
        bool IsKeyDown(Keys key);

        /// <summary>
        /// Checks if the specified key was pressed during the current update.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>True if the key was pressed; otherwise, false.</returns>
        bool IsKeyPressed(Keys key);

        /// <summary>
        /// Updates the input manager's internal state. Should be called once per frame.
        /// </summary>
        void Update();
    }
}
