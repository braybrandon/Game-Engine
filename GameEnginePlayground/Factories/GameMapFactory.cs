using GameEngine.Common.Config;
using GameEngine.Common.Interfaces;
using GameEngine.IO.Asset.models;

namespace GameEnginePlayground.Factories
{
    public class GameMapFactory: IFactory<ITileMap>
    {
        IAssetManager _assetManager;
        public GameMapFactory(IAssetManager assetManager) { 
             _assetManager = assetManager;
        }
        public ITileMap Create()
        {
            var gameMap = _assetManager.Load<TileMap>(FileNameConfig.GrassMap);

            foreach (var tileset in gameMap.Tilesets)
            {
                var texture = _assetManager.LoadTexture(tileset.Name);
                tileset.Texture = texture;
            }

            return gameMap;
        }
    }
}
