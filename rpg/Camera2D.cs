using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace rpg 
{
    public class Camera2D
    {
        // Public properties to control the camera
        public Vector2 Position { get; set; } // The world position the camera is centered on
        public float Rotation { get; set; } = 0.0f; // Rotation in radians
        public float Zoom { get; set; } = 1.0f; // Zoom level (1.0f is no zoom)

        // Internal fields
        private Viewport _viewport; // The game's viewport
        private Vector2 _origin;    // The screen-space origin for rotation and zoom (usually center of viewport)
        private Matrix _transformMatrix; // The cached transformation matrix

        // Constructor
        public Camera2D(Viewport viewport)
        {
            _viewport = viewport;
            // Default position is (0,0) in world space
            Position = Vector2.Zero;
            // Origin for transformations is the center of the viewport
            _origin = new Vector2(viewport.Width / 2f, viewport.Height / 2f);
        }

        /// <summary>
        /// Gets the transformation matrix for SpriteBatch.Begin().
        /// This matrix transforms world coordinates to screen coordinates.
        /// </summary>
        public Matrix GetViewMatrix()
        {
            // The order of operations is crucial:
            // 1. Move the world so the camera's position is at (0,0) relative to the screen.
            //    We use -Position because moving the camera right makes the world appear to move left.
            // 2. Rotate the world around the camera's origin.
            // 3. Scale the world (zoom) around the camera's origin.
            // 4. Translate the world back so the camera's origin is at the center of the viewport.

            _transformMatrix =
                Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) * // Translate to origin
                Matrix.CreateRotationZ(Rotation) * // Rotate
                Matrix.CreateScale(Zoom, Zoom, 1) * // Zoom
                Matrix.CreateTranslation(new Vector3(_origin.X, _origin.Y, 0));     // Translate back to screen center

            return _transformMatrix;
        }

        /// <summary>
        /// Converts a screen-space position (e.g., mouse coordinates) to a world-space position.
        /// </summary>
        /// <param name="screenPosition">The position in screen coordinates.</param>
        /// <returns>The corresponding position in world coordinates.</returns>
        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            // Get the inverse of the camera's transformation matrix.
            // This matrix transforms screen coordinates back to world coordinates.
            Matrix inverseViewMatrix = Matrix.Invert(GetViewMatrix());

            // Transform the screen position using the inverse matrix
            return Vector2.Transform(screenPosition, inverseViewMatrix);
        }

        /// <summary>
        /// Converts a world-space position to a screen-space position.
        /// (Useful for UI elements that follow world objects)
        /// </summary>
        /// <param name="worldPosition">The position in world coordinates.</param>
        /// <returns>The corresponding position in screen coordinates.</returns>
        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            // This is simply applying the camera's view matrix to the world position
            return Vector2.Transform(worldPosition, GetViewMatrix());
        }

        /// <summary>
        /// Gets the world-space center of the camera's current view.
        /// </summary>
        public Vector2 GetCenter()
        {
            return ScreenToWorld(_origin);
        }

        // --- Optional helper methods ---

        /// <summary>
        /// Pans the camera by a given amount.
        /// </summary>
        public void Pan(Vector2 amount)
        {
            Position += amount;
        }

        /// <summary>
        /// Zooms the camera in or out.
        /// </summary>
        /// <param name="deltaZoom">Amount to change zoom by. Positive to zoom in, negative to zoom out.</param>
        public void AdjustZoom(float deltaZoom)
        {
            Zoom += deltaZoom;
            // Optional: Clamp zoom to prevent extreme values
            Zoom = MathHelper.Clamp(Zoom, 0.1f, 5.0f); // Example limits
        }

        /// <summary>
        /// Rotates the camera.
        /// </summary>
        /// <param name="deltaRotation">Amount to change rotation by in radians.</param>
        public void AdjustRotation(float deltaRotation)
        {
            Rotation += deltaRotation;
        }

        /// <summary>
        /// Centers the camera on a specific world position.
        /// </summary>
        public void LookAt(Vector2 worldPosition)
        {
            Position = worldPosition;
        }
    }
}