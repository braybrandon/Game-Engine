namespace Common.Interfaces
{
    public interface ITileMapData
    {
        /// <summary>
        /// The name of the tilemap (e.g., "Level1", "ForestDungeon").
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The width of a single tile in pixels.
        /// </summary>
        public int TileWidth { get; set; }

        /// <summary>
        /// The height of a single tile in pixels.
        /// </summary>
        public int TileHeight { get; set; }

        /// <summary>
        /// The list of all layers that make up this tilemap.
        /// </summary>
        public List<ITileLayerData> Layers { get; set; }

        /// <summary>
        /// The asset name of the tileset texture to be loaded by ContentManager.
        /// This is the string used in Content.Load<Texture2D>("Textures/MyTileset").
        /// </summary>
        public string TileSetTextureName { get; set; }

    }
}
