using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interfaces
{
    public interface ITileInfo
    {
        /// <summary>
        /// The unique ID of the tile type within its tileset.
        /// This usually corresponds to an index in a texture or a GID from Tiled.
        /// A value of 0 typically represents an empty tile.
        /// </summary>
        public int TileId { get; set; }

        /// <summary>
        /// Example: Indicates if this tile should block movement.
        /// Add other properties here as needed for your game logic.
        /// </summary>
        public bool IsCollidable { get; set; }
    }
}
