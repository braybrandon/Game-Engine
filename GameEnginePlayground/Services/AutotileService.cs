using GameEngine.Common.IO.Interface;
using System.Collections.Generic;

namespace GameEnginePlayground.Services
{
    /// <summary>
    /// Provides autotiling functionality for a tile map, updating tile IDs based on neighboring dirt tiles.
    /// Handles digging actions and automatically updates the bitmask and tile appearance for seamless terrain transitions.
    /// </summary>
    public sealed class AutotileService
    {
        private readonly TilePropertyCache _props;
        private readonly ITileMap _map;
        private readonly int _layer; 
        private const int N = 4, E = 2, S = 8, W = 1;

        /// <summary>
        /// Maps bitmask values to corresponding tile IDs for autotiling.
        /// </summary>
        private static readonly Dictionary<int, int> BitmaskToTileId = new()
        {
            {  0, 14 }, {  W,  7 }, {  E,  5 }, {  W|E,  6 },
            {  N, 24 }, {  N|W,20 }, {  N|E,18 }, { N|W|E,19 },
            {  S,  8 }, {  S|W, 4 }, {  S|E, 2 }, { S|W|E, 3 },
            { N|S,16 }, { N|S|W,12 }, { N|S|E,10 }, { N|E|S|W,11 },
        };

        /// <summary>
        /// Initializes a new instance of the AutotileService for the specified map, property cache, and layer.
        /// </summary>
        /// <param name="map">The tile map to apply autotiling to.</param>
        /// <param name="props">The property cache for tile properties.</param>
        /// <param name="layerIndex">The index of the layer to operate on (default is 0).</param>
        public AutotileService(ITileMap map, TilePropertyCache props, int layerIndex = 0)
        {
            _map = map;
            _props = props;
            _layer = layerIndex;
        }

        /// <summary>
        /// Performs a dig action at the specified tile coordinates, updating the tile and its neighbors for autotiling.
        /// </summary>
        /// <param name="tileX">The X coordinate of the tile.</param>
        /// <param name="tileY">The Y coordinate of the tile.</param>
        public void DigAt(int tileX, int tileY)
        {
            if (_props.IsSolid(_map, _layer, tileX, tileY))
                return;

            ApplyBitmaskAt(tileX, tileY);
            UpdateNeighbors(tileX, tileY);
        }

        /// <summary>
        /// Applies the autotile bitmask at the specified coordinates, updating the tile ID based on neighboring dirt tiles.
        /// </summary>
        /// <param name="x">The X coordinate of the tile.</param>
        /// <param name="y">The Y coordinate of the tile.</param>
        private void ApplyBitmaskAt(int x, int y)
        {
            int bm = 0;
            if (_props.IsDirt(_map, _layer, x, y - 1)) bm |= N;
            if (_props.IsDirt(_map, _layer, x + 1, y)) bm |= E;
            if (_props.IsDirt(_map, _layer, x, y + 1)) bm |= S;
            if (_props.IsDirt(_map, _layer, x - 1, y)) bm |= W;

            if (BitmaskToTileId.TryGetValue(bm, out var newId))
                _map.Layers[_layer].UpdateTileId(x, y, newId);
        }

        /// <summary>
        /// Updates the autotile bitmask for neighboring tiles after a dig action, ensuring seamless terrain transitions.
        /// </summary>
        /// <param name="x">The X coordinate of the tile.</param>
        /// <param name="y">The Y coordinate of the tile.</param>
        private void UpdateNeighbors(int x, int y)
        {
            var neighbors = new (int x, int y)[] { (x - 1, y), (x + 1, y), (x, y - 1), (x, y + 1) };
            foreach (var (nx, ny) in neighbors)
            {
                if (_props.IsDirt(_map, _layer, nx, ny))
                    ApplyBitmaskAt(nx, ny);
            }
        }
    }
}
