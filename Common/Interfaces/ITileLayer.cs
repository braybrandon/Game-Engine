namespace Common.Interfaces
{
    public interface ITileLayer
    {
        public string Name { get; set; }
        public string Type { get; set; } 
        public int Width { get; set; }
        public int Height { get; set; }
        public int[] Tiles { get; set; }
        public string Draworder { get; set; }
        public int Id { get; set; }
        public List<ITileObject> Objects { get; set; }
        public int Opacity { get; set; }
        public bool Visible { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public int GetTileId(int x, int y);
        public void UpdateTileId(int x, int y, int id);
    }

    public interface ITileObject
    {
        public double Height { get; set; }
        public int Id { get; set; }
        public int Gid { get; set; }
        public string Name { get; set; }
        public bool Point { get; set; }
        public int Rotation { get; set; }
        public string Type { get; set; }
        public bool Visible { get; set; }
        public double Width { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
    }
}
