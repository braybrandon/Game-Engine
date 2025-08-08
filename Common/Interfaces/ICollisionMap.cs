using Microsoft.Xna.Framework;

namespace Common.Interfaces
{
    public interface ICollisionMap
    {

        public bool IsSolid(Rectangle boundingBox, int tileWidth, int tileHeight);

        public bool IsSolid(Vector2 position, int tileWidth, int tileHeight);

        public bool IsSolid(int x, int y);
    }
}
