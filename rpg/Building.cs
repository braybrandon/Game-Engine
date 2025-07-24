using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace rpg
{
    class Building
    {
        private Vector2 position = new Vector2(0, 0);
        public Vector2 Position { get { return position; } }

        private Rectangle boundingBox;
        public Rectangle BoundingBox { get { return boundingBox;  } }

        private bool isTraversable;
        public bool IsTraversable { get { return isTraversable; } }

        private Texture2D _buildingTexture;
        private Texture2D _textureSprite;

        public Building(Vector2 initialPosition, int width, int height, bool traversable)
        {
            position = initialPosition;
            boundingBox = new Rectangle((int)position.X + 11, (int)position.Y + 199, width, height);
            isTraversable = traversable;
        }

        public void LoadContent(Texture2D buildingSprite, Texture2D pixelSprite)
        {
            _buildingTexture = buildingSprite;
            _textureSprite = pixelSprite;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_buildingTexture != null)
            {
                spriteBatch.Draw(_buildingTexture, position, Color.White);
            }

            // Optional: Draw the bounding box for debugging
            // This requires a 1x1 white texture pixel to draw rectangles

            spriteBatch.Draw(_textureSprite, BoundingBox, Color.Red * 0.5f);
        }
    }
}
