using Common.Interfaces;
using Microsoft.Xna.Framework;

namespace GameEngine.Physics
{
    public class CollisionMap: ICollisionMap
    {
        private readonly bool[,] _solidTiles;

        public CollisionMap(ITileMap map, ITileLayer layer)
        {
            _solidTiles = new bool[layer.Width, layer.Height];

            for (int y = 0; y < layer.Height; y++)
            {
                for (int x = 0; x < layer.Width; x++)
                {
                    int tileId = layer.GetTileId(x, y);

                    var tileset = map.GetTilesetForTile(tileId);
                    if (tileset == null) continue;

                    var tile = tileset.Tiles?.FirstOrDefault(t => t.Id == tileId);
                    if (tile == null) continue;

                    var prop = tile.Properties?.FirstOrDefault(p => p.Name == "IsSolid");
                    if (prop?.Value is bool solid && solid)
                    {
                        _solidTiles[x, y] = true;
                    }
                }
            }
        }

        public bool IsSolid(Rectangle boundingBox, int tileWidth, int tileHeight)
        {
            int left = boundingBox.Left / tileWidth;
            int right = (boundingBox.Right - 1) / tileWidth;
            int top = boundingBox.Top / tileHeight;
            int bottom = (boundingBox.Bottom - 1) / tileHeight;

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

        public bool IsSolid(Vector2 position, int tileWidth, int tileHeight)
        {
            int x = (int)(position.X / tileWidth);
            int y = (int)(position.Y / tileHeight);
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
