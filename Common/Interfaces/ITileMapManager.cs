using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Common.Interfaces
{
    public interface ITileMapManager
    { /// Loads a tilemap asset processed by the Content Pipeline.
      /// </summary>
      /// <param name="assetName">The asset name of the compiled TileMapData (e.g., "Maps/Level1").</param>
      /// <returns>The loaded TileMapData object.</returns>
        ITileMapData LoadTileMap(string assetName);

        /// <summary>
        /// Gets the currently loaded tilemap data.
        /// </summary>
        /// <returns>The current TileMapData object, or null if no map is loaded.</returns>
        ITileMapData GetCurrentTileMap();

        /// <summary>
        /// Sets the current tilemap data. This might trigger loading of associated textures
        /// and pre-calculation of source rectangles.
        /// </summary>
        /// <param name="tileMapData">The TileMapData to set as current.</param>
        void SetCurrentTileMap(ITileMapData tileMapData);

        /// <summary>
        /// Draws the current tilemap layers.
        /// </summary>
        /// <param name="spriteBatch">The SpriteBatch instance for drawing.</param>
        /// <param name="cameraViewMatrix">The view matrix of the camera to apply transformations.</param>
        void Draw(SpriteBatch spriteBatch, Matrix cameraViewMatrix);
    }
}
