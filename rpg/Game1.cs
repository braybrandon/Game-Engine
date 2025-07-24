using Comora;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace rpg
{

    enum Dir
    {
        Down, Up, Left, Right
    }


    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D playerSprite;
        Texture2D walkDown;
        Texture2D walkUp;
        Texture2D walkRight;
        Texture2D walkLeft;

        Texture2D background;
        Texture2D ball;
        Texture2D skull;

        Player player = new Player(new Vector2(0, 0), 96, 96);
        Building building = new Building(new Vector2(300, 300), 325, 178, false);

        private Camera2D _camera;

        private MouseState _previousMouseState;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Set your desired back buffer size (internal game resolution)
            // This helps with consistency regardless of window size.
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            // Initialize the camera here after GraphicsDevice is ready
            _camera = new Camera2D(GraphicsDevice.Viewport);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            playerSprite = Content.Load<Texture2D>("Player/player");
            walkDown = Content.Load<Texture2D>("Player/walkDown");
            walkUp = Content.Load<Texture2D>("Player/walkUp");
            walkLeft = Content.Load<Texture2D>("Player/walkLeft");
            walkRight = Content.Load<Texture2D>("Player/walkRight");

            ball = Content.Load<Texture2D>("ball");
            skull = Content.Load<Texture2D>("skull");
            background = Content.Load<Texture2D>("background");
            Texture2D buildingSprite = Content.Load<Texture2D>("building");
            Texture2D pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
            building.LoadContent(buildingSprite, pixel);

            Texture2D playerPixel = new Texture2D(GraphicsDevice, 1, 1);
            playerPixel.SetData(new[] { Color.White });
            player.LoadContent(playerPixel);

            player.animations[(int)Dir.Down] = new SpriteAnimation(walkDown, 4, 8);
            player.animations[(int)Dir.Up] = new SpriteAnimation(walkUp, 4, 8);
            player.animations[(int)Dir.Left] = new SpriteAnimation(walkLeft, 4, 8);
            player.animations[(int)Dir.Right] = new SpriteAnimation(walkRight, 4, 8);

            player.anim = player.animations[0];
            player.anim.Position = player.Position;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            MouseState currentMouseState = Mouse.GetState();
            //KeyboardState currentKeyboardState = Keyboard.GetState(); // Get current keyboard state

            // --- Game Logic Updates ---
            player.Update(gameTime, _camera, building);
            //if (!player.dead) Controller.Update(gameTime, skull);

            // --- Camera Update ---
            // Tell the camera to look at the player's position
            _camera.LookAt(player.Position);

            // Camera Zoom Input (using mouse scroll wheel)
            float scrollDelta = currentMouseState.ScrollWheelValue - _previousMouseState.ScrollWheelValue;
            if (scrollDelta != 0)
            {
                _camera.AdjustZoom(scrollDelta * 0.001f);
            }

            _previousMouseState = currentMouseState;


            // --- Projectile and Enemy Logic ---
            foreach (Projectile proj in Projectile.projectiles) proj.Update(gameTime);
            foreach (Enemy e in Enemy.enemies)
            {
                e.Update(gameTime, player.Position, player.dead);
                int sum = 32 + e.radius;
                if (Vector2.Distance(player.Position, e.Position) < sum) player.dead = true;
            }

            foreach (Projectile proj in Projectile.projectiles)
            {
                foreach (Enemy e in Enemy.enemies)
                {
                    int sum = proj.radius + e.radius;
                    if (Vector2.Distance(proj.Position, e.Position) < sum)
                    {
                        proj.Collided = true;
                        e.Dead = true;
                    }
                }
            }

            Projectile.projectiles.RemoveAll(p => p.Collided || p.TraveledMaxDistance);
            Enemy.enemies.RemoveAll(e => e.Dead);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            _spriteBatch.Begin(transformMatrix: _camera.GetViewMatrix());


            _spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            building.Draw(_spriteBatch);
            foreach (Enemy e in Enemy.enemies) e.anim.Draw(_spriteBatch);
            foreach (Projectile proj in Projectile.projectiles) _spriteBatch.Draw(ball, new Vector2(proj.Position.X - 48, proj.Position.Y - 48), Color.White);
            if (!player.dead) player.anim.Draw(_spriteBatch);
            player.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

