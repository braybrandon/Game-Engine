namespace GameEngine.IO.Asset.models
{
    public class TileMap
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public List<TileLayer> Layers { get; set; } = new List<TileLayer>();
        public List<Tileset> Tilesets { get; set; } = new List<Tileset>();

        public Tileset GetTilesetForTile(int tileId)
        {
            Tileset result = null;
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
