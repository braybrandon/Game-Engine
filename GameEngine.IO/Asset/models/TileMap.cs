using Common.Interfaces;

namespace GameEngine.IO.Asset.models
{
    public class TileMap : ITileMap
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public List<ITileLayer> Layers { get; set; } = new();
        public List<ITileset> Tilesets { get; set; } = new();

        public ITileset GetTilesetForTile(int tileId)
        {
            ITileset result = null;
            foreach (var tileset in Tilesets)
            {
                if (tileset.FirstGID <= tileId)
                {
                    if (result == null || tileset.FirstGID > result.FirstGID)
                        result = tileset;
                }
            }
            return result;
        }
    }
}
