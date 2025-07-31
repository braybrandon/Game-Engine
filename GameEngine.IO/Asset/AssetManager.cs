using Common.Interfaces;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.IO.Asset
{
    public class AssetManager : IAssetManager
    {
        private ContentManager _contentManager;
        private ITileMapManager _tileMapManager;
        private Dictionary<string, object> _assetCache;
        public AssetManager(ContentManager contentManager, ITileMapManager tileMapManager)
        {
            _contentManager = contentManager;
            _assetCache = new Dictionary<string, object>();
            _tileMapManager = tileMapManager;
        }

        public Texture2D LoadTexture(string path)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException("path");
            if (_assetCache.TryGetValue(path, out object cachedAsset))
            {
                return (Texture2D)cachedAsset;
            }
            Texture2D texture = _contentManager.Load<Texture2D>(path);
            _assetCache[path] = texture;
            return texture;
        }

        public ITileMapData LoadTileMap(string path)
        {
            if(string.IsNullOrEmpty(path)) throw new ArgumentNullException("path");
            return _tileMapManager.LoadTileMap( path);
        }

        public void UnloadAsset(string assetName)
        {
            if (string.IsNullOrEmpty(assetName)) throw new ArgumentNullException("assetName");
            _assetCache.Remove(assetName);
        }

        public void UnloadAll()
        {
            _contentManager.Unload();
            _assetCache.Clear();
        }

        public T Load<T>(string path)
        {
            throw new NotImplementedException();
        }
    }
}
