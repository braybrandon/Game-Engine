using Common.Interfaces;
using GameEngine.Graphics.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Graphics.Render
{
    public class TileMapRenderSystem(ITileMap gameMap, Dictionary<ITileset, Texture2D> tilesetTextures, IEntity cameraEntity, RenderTarget2D mainTarget, GraphicsDevice graphicsDevice) : IDrawSystem
    {
        private readonly ITileMap _gameMap = gameMap;
        private readonly Dictionary<ITileset, Texture2D> _tilesetTextures = tilesetTextures;
        private readonly IEntity _cameraEntity= cameraEntity;
        private readonly RenderTarget2D _mainTarget = mainTarget;
        private readonly GraphicsDevice _graphicsDevice = graphicsDevice;

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

                        if (tileset == null || !_tilesetTextures.TryGetValue(tileset, out var tilesetTexture))
                            continue;

                        int localTileId = tileId - tileset.FirstGID;
                        int tilesPerRow = tilesetTexture.Width / tileset.TileWidth;

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

                        _spriteBatch.Draw(tilesetTexture, position, sourceRect, Color.White);
                    }
                }
            }

            _spriteBatch.End();
        }
    }
}
