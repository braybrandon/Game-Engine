using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.IO.Asset.models
{
public class TileLayer
{
    public string Name { get; set; }
    public string Type { get; set; }  // Important to know it's "tilelayer"
    public int Width { get; set; }
    public int Height { get; set; }
    public int[] Tiles { get; set; }

    public int GetTileId(int x, int y)
    {
        return Tiles[y * Width + x];
    }
}


}
