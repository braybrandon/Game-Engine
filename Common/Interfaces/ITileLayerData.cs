namespace Common.Interfaces
{
    public interface ITileLayerData
    {
        /// <summary>
        /// The name of the layer (e.g., "Ground", "Collision").
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The width of the layer in tiles.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// The height of the layer in tiles.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// The flattened 1D array of tile IDs for this layer.
        /// Access as: TileIds[y * Width + x].
        /// </summary>
        public int[] TileIds { get; set; }
    }
}
