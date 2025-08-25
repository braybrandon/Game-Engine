namespace GameEngine.Common.IO.Interface
{
    public interface ITileMap
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int TileWidth { get; set; }
        public ITileLayer TreeLayer { get; set; }
        public int TileHeight { get; set; }
        public ITileLayer PlayerLayer { get; set; }
        public ITileLayer ObjectTileLayer { get; set; }
        public List<ITileLayer> Layers { get; set; }
        public List<ITileset> Tilesets { get; set; }

        public ITileset GetTilesetForTile(int tileId);
    }

}

