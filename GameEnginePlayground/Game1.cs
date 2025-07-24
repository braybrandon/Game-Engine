using GameEngine.Core;
using GameEngine.Core.Components;
using GameEngine.Core.Entities;
using GameEngine.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameEnginePlayground
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameLoop _gameLoop;

        // Entities
        private Entity _playerEntity;

        // Textures
        private Texture2D _playerTexture;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _gameLoop = new GameLoop(this);

            _playerEntity = Entity.Create();
            ComponentManager.AddComponent(_playerEntity, new PositionComponent { Position = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2) });
            ComponentManager.AddComponent(_playerEntity, new VelocityComponent { Value = Vector2.Zero });
            ComponentManager.AddComponent(_playerEntity, new HealthComponent { CurrentHealth = 100, MaxHealth = 100 });
            ComponentManager.AddComponent(_playerEntity, new PlayerInputComponent { IsPlayerControlled = true });
            ComponentManager.AddComponent(_playerEntity, new ColliderComponent { Bounds = new Rectangle(0, 0, 96, 96), IsTrigger = false, IsStatic = false });

            _gameLoop.Initialize();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            _playerTexture = Content.Load<Texture2D>("player");

            if (ComponentManager.HasComponent<PositionComponent>(_playerEntity))
            {
                ComponentManager.AddComponent(_playerEntity, new SpriteComponent
                {
                    Texture = _playerTexture,
                    SourceRectangle = _playerTexture.Bounds,
                    Color = Color.White,
                    Scale = 1f,
                    Rotation = 0f,
                    Origin = new Vector2(_playerTexture.Width / 2, _playerTexture.Height / 2),
                    Effects = SpriteEffects.None,
                    LayerDepth = 0f
                });
            }

            _gameLoop.RegisterSystem(new SpriteRenderSystem(this));
            _gameLoop.LoadContent(_spriteBatch);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            _gameLoop.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _gameLoop.Draw(gameTime);
            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
            // Dispose of the GameLoop and its systems
            _gameLoop.Dispose();
            _spriteBatch.Dispose();
            base.UnloadContent();
        }
    }
}
