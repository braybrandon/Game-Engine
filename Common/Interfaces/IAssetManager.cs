using Microsoft.Xna.Framework.Graphics;

namespace Common.Interfaces
{
    public interface IAssetManager
    {
        Texture2D LoadTexture(string path);
        ITileMapData LoadTileMap(string path);

    }
}
