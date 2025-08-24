using GameEngine.Common.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Graphics.Camera
{
    public struct CameraComponent : IComponent
    {
        public float Zoom;
        public Viewport Viewport;
        public Matrix ViewMatrix;

        public CameraComponent(Viewport viewport)
        {
            this.Viewport = viewport;
            Zoom = 0.1f;
            ViewMatrix = Matrix.Identity;
        }

        public Matrix CalculateViewMatrix(Vector2 cameraPosition, float cameraRotation, Vector2 cameraScale)
        {
            return Matrix.CreateTranslation(-cameraPosition.X, -cameraPosition.Y, 0f) *
                Matrix.CreateRotationZ(-cameraRotation) *
                Matrix.CreateScale(cameraScale.X * Zoom, cameraScale.Y * Zoom, 1f) *
                Matrix.CreateTranslation(Viewport.Width / 2f, Viewport.Height / 2f, 0f);
        }
    }
}
