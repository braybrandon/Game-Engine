using GameEngine.Common.Components;
using GameEngine.Common.Interfaces;
using GameEngine.Graphics.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Graphics.Render
{
    public class DebugRendererSystem: IDrawSystem
    {

        private Texture2D _pixel;
        private IEntity _cameraEntity;
        private ICollisionMap _collisionMap;
        public DebugRendererSystem(Texture2D pixel, IEntity cameraEntity, ICollisionMap collisionMap)
        {

            _pixel = pixel;
            _cameraEntity = cameraEntity;
            _collisionMap = collisionMap;
        }

        public void Draw(SpriteBatch spriteBatch, IWorld world)
        {
            var camera = world.GetComponent<CameraComponent>(_cameraEntity.Id);
            var culling = world.GetComponent<CullingComponent>(_cameraEntity.Id);
            float zoom = camera.Zoom;
            var viewport = camera.Viewport;
            var viewMatrix = camera.ViewMatrix;

            float viewWidth = viewport.Width / zoom;
            float viewHeight = viewport.Height / zoom;

            spriteBatch.Begin(transformMatrix: viewMatrix);
            for (int y = culling.MinY; y < culling.MaxY; y++)
            {
                for (int x = culling.MinX; x < culling.MaxX; x++)
                {
                    if (_collisionMap.IsSolid(x, y))
                    {
                        Rectangle outline = new Rectangle(
                            x * _collisionMap.TileWidth,
                            y * _collisionMap.TileHeight,
                            _collisionMap.TileWidth,
                            _collisionMap.TileHeight
                        );

                        DrawRectangleOutline(spriteBatch, outline, Color.Red, 1);
                    }
                }
            }
            spriteBatch.End();
        }

        private void DrawRectangleOutline(SpriteBatch spriteBatch, Rectangle rect, Color color, int thickness)
        {
            // Top
            spriteBatch.Draw(_pixel, new Rectangle(rect.X, rect.Y, rect.Width, thickness), color);
            // Bottom
            spriteBatch.Draw(_pixel, new Rectangle(rect.X, rect.Y + rect.Height - thickness, rect.Width, thickness), color);
            // Left
            spriteBatch.Draw(_pixel, new Rectangle(rect.X, rect.Y, thickness, rect.Height), color);
            // Right
            spriteBatch.Draw(_pixel, new Rectangle(rect.X + rect.Width - thickness, rect.Y, thickness, rect.Height), color);
        }
    }
}
