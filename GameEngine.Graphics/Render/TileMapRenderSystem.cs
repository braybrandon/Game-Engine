using GameEngine.Common.Components;
using GameEngine.Common.Interfaces;
using GameEngine.Common.IO.Interface;
using GameEngine.Graphics.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Graphics.Render
{
    public class TileMapRenderSystem(ITileMap gameMap, IEntity cameraEntity, RenderTarget2D mainTarget, GraphicsDevice graphicsDevice) : IDrawSystem
    {
        private readonly ITileMap _gameMap = gameMap;
        private readonly IEntity _cameraEntity= cameraEntity;
        private readonly RenderTarget2D _mainTarget = mainTarget;
        private readonly GraphicsDevice _graphicsDevice = graphicsDevice;

        public void Draw(SpriteBatch _spriteBatch, IWorld world)
        {
            var camera = world.GetComponent<CameraComponent>(_cameraEntity.Id);
            var culling = world.GetComponent<CullingComponent>(_cameraEntity.Id);
            var viewport = camera.Viewport;
            var viewMatrix = camera.ViewMatrix;

            //_graphicsDevice.SetRenderTarget(_mainTarget);
            _graphicsDevice.Clear(Color.DarkGreen);
            _spriteBatch.Begin(transformMatrix: viewMatrix);

            foreach (var layer in _gameMap.Layers)
            {
                for (int y = culling.MinY; y < culling.MaxY; y++)
                {
                    for (int x = culling.MinX; x < culling.MaxX; x++)
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
                        int tilesPerRow = tileset.Texture.Width / (tileset.TileWidth +2);

                        int tileX = localTileId % tilesPerRow;
                        int tileY = localTileId / tilesPerRow;

                        Rectangle sourceRect = new Rectangle(
                            tileX * (tileset.TileWidth+ 2) + 1,
                            tileY * (tileset.TileHeight + 2) + 1,
                            tileset.TileWidth,
                            tileset.TileHeight
                        );

                        Vector2 position = new Vector2(
                            x * tileset.TileWidth,
                            y * tileset.TileHeight
                        );

                        _spriteBatch.Draw(tileset.Texture, position, sourceRect, Color.White);
                    }
                }
            }

            _spriteBatch.End();
        }


    }
}
