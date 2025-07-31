using GameEngine.IO.Asset.models;
using System.Collections.Generic;

namespace GameEngine.ContentPipeline
{

    public class TiledMapRaw
    {
        public int width { get; set; }
        public int height { get; set; }
        public int tilewidth { get; set; }
        public int tileheight { get; set; }
        public List<TiledTileset> tilesets { get; set; }
        public List<TiledLayer> layers { get; set; }
    }

    public class TiledTileset
    {
        public int? columns { get; set; }
        public int firstgid { get; set; }
        public string source { get; set; }        // for external TSX tilesets
        public string name { get; set; }
        public int tilewidth { get; set; }
        public int tileheight { get; set; }
        public string image { get; set; }
        public int? imageheight { get; set; }
        public int? imagewidth { get; set; }
        public int? margin { get; set; }
        public int? spacing { get; set; }
        public int? tilecount { get; set; }
        public string transparentcolor { get; set; }
        public string type { get; set; }
        public double? version { get; set; }
        public string tiledversion { get; set; }

        public Grid grid { get; set; }

        public List<TiledProperty> properties { get; set; }
        public List<TiledTile> tiles { get; set; }

        public List<object> wangsets { get; set; }  // can be refined later
    }

    public class Grid
    {
        public int height { get; set; }
        public int width { get; set; }
        public string orientation { get; set; }
    }

    public class TiledProperty
    {
        public string name { get; set; }
        public string type { get; set; }
        public object value { get; set; }  // object to handle bool, int, string, etc.
    }

    public class TiledTile
    {
        public int id { get; set; }
        public List<TiledProperty> properties { get; set; }
        public List<TileAnimationFrame> animation { get; set; }
        public object objectgroup { get; set; }  // can be expanded into a class if needed
    }

    public class TileAnimationFrame
    {
        public int duration { get; set; }
        public int tileid { get; set; }
    }

    public class TiledLayer
    {
        public string name { get; set; }
        public int width { get; set; }
        public int height { get; set; } 

        public string type { get; set; } 
        public int[] data { get; set; }  
                                         
    }
}
