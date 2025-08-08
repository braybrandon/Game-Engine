using Common.Config;
using Common.Interfaces;
using GameEngine.IO.Asset;
using GameEngine.IO.Asset.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var gameMap = _assetManager.Load<TileMap>(FileNameConfig.TestMap);

            foreach (var tileset in gameMap.Tilesets)
            {
                var texture = _assetManager.LoadTexture(tileset.Name);
                tileset.Texture = texture;
            }

            return gameMap;
        }
    }
}
