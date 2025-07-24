using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace rpg
{
    class Player
    {
        private Vector2 position = new Vector2(0, 0);
        private Vector2 _prevPosition = new Vector2(0, 0);
        private int speed = 300;
        private Dir direction = Dir.Down;
        private bool isMoving = false;
        private Texture2D _textureSprite;

        private MouseState mStateOld = Mouse.GetState();
        public bool dead = false;

        public SpriteAnimation anim;

        public SpriteAnimation[] animations = new SpriteAnimation[4];

        private Rectangle boundingBox;
        public Rectangle BoundingBox { get { return boundingBox; } }

        public Vector2 Position { get { return position; } }

        public Player(Vector2 initialPosition, int width, int height)
        {
            this.position = initialPosition;
            boundingBox = new Rectangle((int)position.X - 48, (int)position.Y - 48, width, height);
        }

        public void LoadContent(Texture2D pixelSprite)
        {
            _textureSprite = pixelSprite;
        }

        public void setx(float newx)
        {
            position.X = newx;
        }

        public void setY(float newy)
        {
            position.Y = newy;
        }
        public void UpdateBoundingBox()
        {
            boundingBox.X = (int)position.X - 48;
            boundingBox.Y = (int)position.Y - 48;
        }

        public void Update(GameTime gameTime, Camera2D camera, Building building)
        { 
            Vector2 inputDirection = Vector2.Zero;
            KeyboardState kState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            isMoving = false;

            if (kState.IsKeyDown(Keys.D))
            {
                inputDirection.X += 1;
                direction = Dir.Right;
            }
            if (kState.IsKeyDown(Keys.A))
            {
                inputDirection.X -= 1;
                direction = Dir.Left;
            }


            if (kState.IsKeyDown(Keys.W))
            {
                inputDirection.Y -= 1;
                direction = Dir.Up;
            }
            if (kState.IsKeyDown(Keys.S))
            {
                inputDirection.Y += 1;
                direction = Dir.Down;
            }

            isMoving = inputDirection != Vector2.Zero;

            if (boundingBox.Intersects(building.BoundingBox))
            {
                if(!building.IsTraversable)
                {
                    isMoving = false;
                    position = _prevPosition;
                }
            }
            _prevPosition = position;

            if (mouseState.LeftButton == ButtonState.Pressed) isMoving = false;

            if (dead) isMoving = false;

            if (isMoving)
            {
                inputDirection.Normalize();
                position += inputDirection * speed * dt;
            }

            anim = animations[(int)direction];

            anim.Position = new Vector2(position.X - 48, position.Y - 48);
            if (mouseState.LeftButton == ButtonState.Pressed) anim.setFrame(0);
            else if (isMoving) anim.Update(gameTime);
            else anim.setFrame(1);
            if (mouseState.LeftButton == ButtonState.Pressed && mStateOld.LeftButton == ButtonState.Released)
            {
                Vector2 screenMousePosition = new Vector2(mouseState.X, mouseState.Y);
                // Convert screen coordinates to world coordinates using your custom camera
                Vector2 worldMousePosition = camera.ScreenToWorld(screenMousePosition);
                Vector2 moveDir = position - worldMousePosition;
                moveDir.Normalize();
                Projectile.projectiles.Add(new Projectile(position, moveDir));
            }
            mStateOld = mouseState;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            UpdateBoundingBox();
            spriteBatch.Draw(_textureSprite, BoundingBox, Color.Blue * 0.5f);
        }
    }
}
