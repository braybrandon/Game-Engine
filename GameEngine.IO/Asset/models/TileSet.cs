using GameEngine.Common.IO.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.IO.Asset.models
{
    public class Tileset: ITileset
    {
        public int FirstGID { get; set; }
        public string Source { get; set; }          // external tsx file reference, if any
        public string Name { get; set; }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public string ImageSource { get; set; }     // corresponds to 'image' in raw JSON
        public int? ImageWidth { get; set; }
        public int? ImageHeight { get; set; }
        public int? Columns { get; set; }
        public int? TileCount { get; set; }
        public int? Margin { get; set; }
        public int? Spacing { get; set; }
        public string TransparentColor { get; set; }
        public string Type { get; set; }
        public double? Version { get; set; }
        public string TiledVersion { get; set; }
        public Texture2D? Texture { get; set; }

        public IGrid Grid { get; set; }

        public List<IProperty> Properties { get; set; }
        public List<ITile> Tiles { get; set; }
        // You could add wangsets and other collections if you want

        // Optional: add texture or other runtime data here if needed
    }

    public class Grid: IGrid
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public string Orientation { get; set; }
    }

    public class Property: IProperty
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public object Value { get; set; }
    }

    public class Tile: ITile
    {
        public int Id { get; set; }
        public List<ITileObject> Objects { get; set; }
        public List<IProperty> Properties { get; set; }
        public List<ITileAnimationFrame> Animation { get; set; }
        // Add objectgroup or others as needed
    }

    public class TileAnimationFrame: ITileAnimationFrame
    {
        public int Duration { get; set; }
        public int TileId { get; set; }
    }

}
