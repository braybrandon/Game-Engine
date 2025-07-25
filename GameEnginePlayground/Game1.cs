using GameEngine.Animation;
using GameEngine.Core;
using GameEngine.Core.Components;
using GameEngine.Core.Entities;
using GameEngine.Input;
using GameEngine.Physics;
using GameEngine.Rendering;
using GameEngine.Rendering.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace GameEnginePlayground
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameLoop _gameLoop;

        // Entities
        private Entity _playerEntity;
        private Entity _cameraEntity;

        // Textures
        private Texture2D _playerTexture;
        private Texture2D _playerWalkUpTexture;
        private Texture2D _playerWalkDownTexture;
        private Texture2D _playerWalkLeftTexture;
        private Texture2D _playerWalkRightTexture;

        private const int SPRITE_SIZE = 96;

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
            ComponentManager.AddComponent(_playerEntity, new TransformComponent { Position = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Rotation = 0f, Scale = Vector2.One });
            ComponentManager.AddComponent(_playerEntity, new VelocityComponent { Value = Vector2.Zero });
            ComponentManager.AddComponent(_playerEntity, new HealthComponent { CurrentHealth = 100, MaxHealth = 100 });
            ComponentManager.AddComponent(_playerEntity, new PlayerInputComponent { IsPlayerControlled = true });
            ComponentManager.AddComponent(_playerEntity, new ColliderComponent { Bounds = new Rectangle(0, 0, 96, 96), IsTrigger = false, IsStatic = false });
            ComponentManager.AddComponent(_playerEntity, new AnimationComponent { Clips = new Dictionary<AnimationType, AnimationClip>(), CurrentClipName = AnimationType.Idle });

            _cameraEntity = Entity.Create();
            ComponentManager.AddComponent(_cameraEntity, new TransformComponent { Position = new Vector2(400, 240), Rotation = 0f, Scale = Vector2.One });
            ComponentManager.AddComponent(_cameraEntity, new CameraComponent(GraphicsDevice.Viewport) { Zoom = 1f});

            // 3. Register EngineSystems with the GameLoop
            // Order matters for systems that depend on each other's output!
            _gameLoop.RegisterSystem(new PlayerInputSystem(this));
            _gameLoop.RegisterSystem(new CalculateVelocitySystem(this));
            _gameLoop.RegisterSystem(new MovementSystem(this));
            _gameLoop.RegisterSystem(new AnimationStateSystem(this));
            _gameLoop.RegisterSystem(new AnimationSystem(this));
            _gameLoop.RegisterSystem(new CameraUpdateSystem(this, _playerEntity));

            _gameLoop.Initialize();
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            _playerTexture = Content.Load<Texture2D>("player");
            _playerWalkUpTexture = Content.Load<Texture2D>("walkUp");
            _playerWalkDownTexture = Content.Load<Texture2D>("walkDown");
            _playerWalkLeftTexture = Content.Load<Texture2D>("walkLeft");
            _playerWalkRightTexture = Content.Load<Texture2D>("walkRight");

            if (ComponentManager.HasComponent<TransformComponent>(_playerEntity))
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

            // --- Define Player Animations ---
            var playerAnimationComponent = ComponentManager.GetComponent<AnimationComponent>(_playerEntity);
            var playerSpriteComponent = ComponentManager.GetComponent<SpriteComponent>(_playerEntity);

            // Player Idle Animation (using first frame of walk_down as idle for simplicity)
            playerAnimationComponent.Clips[AnimationType.Idle] = new AnimationClip
            {
                Name = "Idle",
                Texture = _playerWalkDownTexture, // Idle uses the down-facing sheet
                Frames = new List<Rectangle> { new Rectangle(0, 0, SPRITE_SIZE, SPRITE_SIZE) },
                FrameDuration = 0.2f,
                IsLooping = true
            };

            // Player Walk Up Animation (e.g., 4 frames, 96x96 each)
            playerAnimationComponent.Clips[AnimationType.WalkUp] = new AnimationClip
            {
                Name = "WalkUp",
                Texture = _playerWalkUpTexture,
                Frames = new List<Rectangle>
            {
                new Rectangle(0 * SPRITE_SIZE, 0, SPRITE_SIZE, SPRITE_SIZE),
                new Rectangle(1 * SPRITE_SIZE, 0, SPRITE_SIZE, SPRITE_SIZE),
                new Rectangle(2 * SPRITE_SIZE, 0, SPRITE_SIZE, SPRITE_SIZE),
                new Rectangle(3 * SPRITE_SIZE, 0, SPRITE_SIZE, SPRITE_SIZE)
            },
                FrameDuration = 0.1f,
                IsLooping = true
            };

            // Player Walk Down Animation (e.g., 4 frames, 96x96 each)
            playerAnimationComponent.Clips[AnimationType.WalkDown] = new AnimationClip
            {
                Name = "WalkDown",
                Texture = _playerWalkDownTexture,
                Frames = new List<Rectangle>
            {
                new Rectangle(0 * SPRITE_SIZE, 0, SPRITE_SIZE, SPRITE_SIZE),
                new Rectangle(1 * SPRITE_SIZE, 0, SPRITE_SIZE, SPRITE_SIZE),
                new Rectangle(2 * SPRITE_SIZE, 0, SPRITE_SIZE, SPRITE_SIZE),
                new Rectangle(3 * SPRITE_SIZE, 0, SPRITE_SIZE, SPRITE_SIZE)
            },
                FrameDuration = 0.1f,
                IsLooping = true
            };

            // Player Walk Left Animation (e.g., 4 frames, 96x96 each)
            playerAnimationComponent.Clips[AnimationType.WalkLeft] = new AnimationClip
            {
                Name = "WalkLeft",
                Texture = _playerWalkLeftTexture,
                Frames = new List<Rectangle>
            {
                new Rectangle(0 * SPRITE_SIZE, 0, SPRITE_SIZE, SPRITE_SIZE),
                new Rectangle(1 * SPRITE_SIZE, 0, SPRITE_SIZE, SPRITE_SIZE),
                new Rectangle(2 * SPRITE_SIZE, 0, SPRITE_SIZE, SPRITE_SIZE),
                new Rectangle(3 * SPRITE_SIZE, 0, SPRITE_SIZE, SPRITE_SIZE)
            },
                FrameDuration = 0.1f,
                IsLooping = true
            };

            // Player Walk Right Animation (e.g., 4 frames, 96x96 each)
            playerAnimationComponent.Clips[AnimationType.WalkRight] = new AnimationClip
            {
                Name = "WalkRight",
                Texture = _playerWalkRightTexture,
                Frames = new List<Rectangle>
            {
                new Rectangle(0 * SPRITE_SIZE, 0, SPRITE_SIZE, SPRITE_SIZE),
                new Rectangle(1 * SPRITE_SIZE, 0, SPRITE_SIZE, SPRITE_SIZE),
                new Rectangle(2 * SPRITE_SIZE, 0, SPRITE_SIZE, SPRITE_SIZE),
                new Rectangle(3 * SPRITE_SIZE, 0, SPRITE_SIZE, SPRITE_SIZE)
            },
                FrameDuration = 0.1f,
                IsLooping = true
            };

            playerSpriteComponent.Texture = playerAnimationComponent.CurrentClip.Texture;
            playerSpriteComponent.SourceRectangle = playerAnimationComponent.CurrentClip.Frames[0]; 
            playerSpriteComponent.Origin = new Vector2(SPRITE_SIZE / 2, SPRITE_SIZE / 2); 

            ComponentManager.AddComponent(_playerEntity, playerAnimationComponent);
            ComponentManager.AddComponent(_playerEntity, playerSpriteComponent);



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
