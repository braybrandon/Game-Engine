using GameEngine.Common.IO.Interface;
using System.Collections.Generic;
using System.Linq;

namespace GameEnginePlayground.Services
{
    /// <summary>
    /// Caches tile properties for quick lookup, such as whether a tile is dirt or solid.
    /// Used to optimize autotiling and terrain logic by storing property flags for each tile ID.
    /// </summary>
    public sealed class TilePropertyCache
    {
        private readonly Dictionary<int, bool> _isDirt = new();
        private readonly Dictionary<int, bool> _isSolid = new();

        /// <summary>
        /// Initializes a new instance of the TilePropertyCache, extracting dirt and solid properties from the map's tileset.
        /// </summary>
        /// <param name="map">The tile map containing tilesets and tile properties.</param>
        public TilePropertyCache(ITileMap map)
        {
            var ts = map.Tilesets[0];

            foreach (var t in ts.Tiles)
            {
                var dirt = t.Properties?.FirstOrDefault(p => p.Name == "IsDirt");
                var solid = t.Properties?.FirstOrDefault(p => p.Name == "IsSolid");

                if (dirt != null && dirt.Value is bool db) _isDirt[t.Id] = db;
                if (solid != null && solid.Value is bool sb) _isSolid[t.Id] = sb;
            }
        }

        /// <summary>
        /// Determines if the given tileset-local ID represents a dirt tile.
        /// </summary>
        /// <param name="tilesetLocalId">The local ID of the tile in the tileset.</param>
        /// <returns>True if the tile is dirt; otherwise, false.</returns>
        public bool IsDirtId(int tilesetLocalId) => _isDirt.TryGetValue(tilesetLocalId, out var v) && v;

        /// <summary>
        /// Determines if the given tileset-local ID represents a solid tile.
        /// </summary>
        /// <param name="tilesetLocalId">The local ID of the tile in the tileset.</param>
        /// <returns>True if the tile is solid; otherwise, false.</returns>
        public bool IsSolidId(int tilesetLocalId) => _isSolid.TryGetValue(tilesetLocalId, out var v) && v;

        /// <summary>
        /// Determines if the tile at the specified map coordinates is dirt.
        /// </summary>
        /// <param name="map">The tile map.</param>
        /// <param name="layerIndex">The index of the layer.</param>
        /// <param name="x">The X coordinate of the tile.</param>
        /// <param name="y">The Y coordinate of the tile.</param>
        /// <returns>True if the tile is dirt; otherwise, false.</returns>
        public bool IsDirt(ITileMap map, int layerIndex, int x, int y)
        {
            var id = map.Layers[layerIndex].GetTileId(x, y) - 1;
            return IsDirtId(id);
        }

        /// <summary>
        /// Determines if the tile at the specified map coordinates is solid.
        /// </summary>
        /// <param name="map">The tile map.</param>
        /// <param name="layerIndex">The index of the layer.</param>
        /// <param name="x">The X coordinate of the tile.</param>
        /// <param name="y">The Y coordinate of the tile.</param>
        /// <returns>True if the tile is solid; otherwise, false.</returns>
        public bool IsSolid(ITileMap map, int layerIndex, int x, int y)
        {
            var id = map.Layers[layerIndex].GetTileId(x, y) - 1;
            return IsSolidId(id);
        }
    }
}
