using GameEngine.Common.Interfaces;
using GameEngine.Common.IO.Interface;
using Microsoft.Xna.Framework;

namespace GameEngine.Physics
{
    public class CollisionMap: ICollisionMap
    {
        private readonly bool[,] _solidTiles;
        private int _layerWidth;
        private int _layerHeight;
        private int _tileWidth;
        private int _tileHeight;

        public int TileWidth
        {
            get => _tileWidth;
        }

        public int TileHeight { get => _tileHeight; }

        public int LayerWidth
        {
            get => _layerWidth;
        }

        public int LayerHeight { get => _layerHeight; }

        public CollisionMap(ITileMap map, ITileLayer layer)
        {
            _solidTiles = new bool[layer.Width, layer.Height];
            _tileWidth = map.TileWidth;
            _tileHeight = map.TileHeight;
            _layerWidth = layer.Width;
            _layerHeight = layer.Height;

            for (int y = 0; y < layer.Height; y++)
            {
                for (int x = 0; x < layer.Width; x++)
                {

                    int tileId = layer.GetTileId(x, y);

                    var tileset = map.GetTilesetForTile(tileId);
                    if (tileset == null) continue;

                    var tile = tileset.Tiles?.FirstOrDefault(t => t.Id == tileId - 1);
                    if (tile == null) continue;

                    var prop = tile.Properties?.FirstOrDefault(p => p.Name == "IsSolid");
                    if (prop?.Value is bool solid && solid)
                    {
                        _solidTiles[x, y] = true;
                    }
                }
            }
        }

        public bool IsSolid(Rectangle boundingBox)
        {
            int left = boundingBox.Left / _tileWidth;
            int right = (boundingBox.Right - 1) / _tileWidth;
            int top = boundingBox.Top / _tileHeight;
            int bottom = (boundingBox.Bottom - 1) / _tileHeight;

            for (int y = top; y <= bottom; y++)
            {
                for (int x = left; x <= right; x++)
                {
                    if (IsSolid(x, y))
                        return true;
                }
            }

            return false;
        }

        public bool IsSolid(Vector2 position)
        {
            int x = (int)(position.X / _tileWidth);
            int y = (int)(position.Y / _tileHeight);
            return IsSolid(x, y);
        }

        public bool IsSolid(int x, int y)
        {
            if (x < 0 || y < 0 || x >= _solidTiles.GetLength(0) || y >= _solidTiles.GetLength(1))
                return true; // treat out-of-bounds as solid

            return _solidTiles[x, y];
        }
    }
}
