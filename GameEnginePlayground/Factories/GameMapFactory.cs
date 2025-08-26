using GameEngine.Common.Config;
using GameEngine.Common.Interfaces;
using GameEngine.Common.IO.Interface;
using GameEngine.IO.Asset.models;
using GameEnginePlayground.Factories.DataObjects;

namespace GameEnginePlayground.Factories
{
    /// <summary>
    /// Factory for creating ITileMap instances using asset manager and map data.
    /// Loads the map and its tileset textures, returning a fully configured ITileMap.
    /// </summary>
    public class GameMapFactory : IFactory<ITileMap, MapData>
    {
        private IAssetManager _assetManager;

        /// <summary>
        /// Initializes a new instance of the GameMapFactory with the specified asset manager.
        /// </summary>
        /// <param name="assetManager">The asset manager used to load map and texture assets.</param>
        public GameMapFactory(IAssetManager assetManager)
        {
            _assetManager = assetManager;
        }

        /// <summary>
        /// Creates an ITileMap instance using the provided map data.
        /// Loads the map and its tileset textures, returning a fully configured map.
        /// </summary>
        /// <param name="data">The map data used for configuration (extend MapData for more options).</param>
        /// <returns>A fully configured ITileMap instance.</returns>
        public ITileMap Create(MapData data)
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
