namespace Common.Interfaces
{
    public interface ITileLayer
    {
        public string Name { get; set; }
        public string Type { get; set; } 
        public int Width { get; set; }
        public int Height { get; set; }
        public int[] Tiles { get; set; }

        public int GetTileId(int x, int y);
    }
}
