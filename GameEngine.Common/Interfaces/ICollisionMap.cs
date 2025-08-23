using Microsoft.Xna.Framework;

namespace Common.Interfaces
{
    public interface ICollisionMap
    {
        public int TileWidth
        {
            get;
        }

        public int TileHeight { get; }
        public int LayerWidth
        {
            get;
        }

        public int LayerHeight { get; }
        public bool IsSolid(Rectangle boundingBox);

        public bool IsSolid(Vector2 position);

        public bool IsSolid(int x, int y);
    }
}
