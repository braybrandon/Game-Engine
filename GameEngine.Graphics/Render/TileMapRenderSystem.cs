using Common.Interfaces;
using GameEngine.Graphics.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Graphics.Render
{
    public class TileMapRenderSystem(ITileMap gameMap, IEntity cameraEntity, RenderTarget2D mainTarget, GraphicsDevice graphicsDevice, ICollisionMap collision, Texture2D pixelTexture) : IDrawSystem
    {
        private readonly ITileMap _gameMap = gameMap;
        private readonly IEntity _cameraEntity= cameraEntity;
        private readonly RenderTarget2D _mainTarget = mainTarget;
        private readonly GraphicsDevice _graphicsDevice = graphicsDevice;
        private ICollisionMap _collisionMap = collision;
        private Texture2D _pixel = pixelTexture;

        public void Draw(SpriteBatch _spriteBatch, IWorld world)
        {
            var camera = world.GetComponent<CameraComponent>(_cameraEntity.Id);
            var viewport = camera.Viewport;
            var viewMatrix = camera.ViewMatrix;

            _graphicsDevice.SetRenderTarget(_mainTarget);
            _graphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin(transformMatrix: viewMatrix);

            // Get camera position in world space (inverse of translation)
            Matrix inverseView = Matrix.Invert(viewMatrix);
            Vector2 cameraPosition = new Vector2(inverseView.Translation.X, inverseView.Translation.Y);

            // Tile culling bounds
            int tileWidth = _gameMap.TileWidth;
            int tileHeight = _gameMap.TileHeight;

            int mapWidth = _gameMap.Width;
            int mapHeight = _gameMap.Height;

            float zoom = camera.Zoom;
            float viewWidth = viewport.Width / zoom;
            float viewHeight = viewport.Height / zoom;

            int minX = Math.Max((int)(cameraPosition.X / tileWidth), 0);
            int minY = Math.Max((int)(cameraPosition.Y / tileHeight), 0);

            int maxX = Math.Min((int)((cameraPosition.X + viewWidth) / tileWidth) + 1, mapWidth);
            int maxY = Math.Min((int)((cameraPosition.Y + viewHeight) / tileHeight) + 1, mapHeight);

            foreach (var layer in _gameMap.Layers)
            {
                for (int y = minY; y < maxY; y++)
                {
                    for (int x = minX; x < maxX; x++)
                    {
                        int tileId = layer.GetTileId(x, y);
                        if (tileId == 0)
                            continue;

                        // Find the tileset for this tile
                        ITileset tileset = null;
                        for (int i = _gameMap.Tilesets.Count - 1; i >= 0; i--)
                        {
                            if (tileId >= _gameMap.Tilesets[i].FirstGID)
                            {
                                tileset = _gameMap.Tilesets[i];
                                break;
                            }
                        }

                        if (tileset == null)
                            continue;

                        int localTileId = tileId - tileset.FirstGID;
                        int tilesPerRow = tileset.Texture.Width / tileset.TileWidth;

                        int tileX = localTileId % tilesPerRow;
                        int tileY = localTileId / tilesPerRow;

                        Rectangle sourceRect = new Rectangle(
                            tileX * tileset.TileWidth,
                            tileY * tileset.TileHeight,
                            tileset.TileWidth,
                            tileset.TileHeight
                        );

                        Vector2 position = new Vector2(
                            x * tileset.TileWidth,
                            y * tileset.TileHeight
                        );

                        _spriteBatch.Draw(tileset.Texture, position, sourceRect, Color.White);
                        if (_collisionMap.IsSolid(x, y))
                        {
                            Rectangle outline = new Rectangle(
                                x * tileWidth,
                                y * tileHeight,
                                tileWidth,
                                tileHeight
                            );

                            DrawRectangleOutline(_spriteBatch, outline, Color.Red, 1);
                        }
                    }
                }
            }

            _spriteBatch.End();
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
