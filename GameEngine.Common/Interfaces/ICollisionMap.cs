using Microsoft.Xna.Framework;

namespace GameEngine.Common.Interfaces
{
    /// <summary>
    /// Represents a collision map for tile-based collision detection.
    /// </summary>
    public interface ICollisionMap
    {
        /// <summary>
        /// Gets the width of a single tile in pixels.
        /// </summary>
        int TileWidth { get; }

        /// <summary>
        /// Gets the height of a single tile in pixels.
        /// </summary>
        int TileHeight { get; }

        /// <summary>
        /// Gets the number of tiles horizontally in the layer.
        /// </summary>
        int LayerWidth { get; }

        /// <summary>
        /// Gets the number of tiles vertically in the layer.
        /// </summary>
        int LayerHeight { get; }

        /// <summary>
        /// Determines if the specified bounding box collides with a solid tile.
        /// </summary>
        /// <param name="boundingBox">The bounding box to check.</param>
        /// <returns>True if the bounding box collides with a solid tile; otherwise, false.</returns>
        bool IsSolid(Rectangle boundingBox);

        /// <summary>
        /// Determines if the specified position collides with a solid tile.
        /// </summary>
        /// <param name="position">The position to check.</param>
        /// <returns>True if the position collides with a solid tile; otherwise, false.</returns>
        bool IsSolid(Vector2 position);

        /// <summary>
        /// Determines if the tile at the specified coordinates is solid.
        /// </summary>
        /// <param name="x">The x-coordinate of the tile.</param>
        /// <param name="y">The y-coordinate of the tile.</param>
        /// <returns>True if the tile is solid; otherwise, false.</returns>
        bool IsSolid(int x, int y);
    }
}
