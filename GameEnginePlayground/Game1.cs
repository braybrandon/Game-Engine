using Common.Interfaces;
using GameEngine.Core;
using GameEngine.Core.Components;
using GameEngine.Core.Services;
using GameEngine.Graphics.Animations;
using GameEngine.Graphics.Camera;
using GameEngine.Graphics.Render;
using GameEngine.IO.Asset;
using GameEngine.IO.Asset.models;
using GameEngine.IO.Controller;
using GameEngine.Physics.Motion;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace GameEnginePlayground
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Engine _gameLoop;
        private IAssetManager _assetManager;
        private ITileMapManager _tileMapManager;
        private IServiceLocator _serviceLocator;

        private IWorld _world;
        private ITimeManager _timeManager;

        private TileMap _gameMap;
        Dictionary<Tileset, Texture2D> _tilesetTextures = new Dictionary<Tileset, Texture2D>();

        // Entities
        private IEntity _playerEntity;
        private IEntity _cameraEntity;

        // Textures
        private Texture2D _playerTexture;
        private Texture2D _playerWalkUpTexture;
        private Texture2D _playerWalkDownTexture;
        private Texture2D _playerWalkLeftTexture;
        private Texture2D _playerWalkRightTexture;
        private Texture2D _playerWalkUpLeftTexture;
        private Texture2D _playerWalkUpRightTexture;
        private Texture2D _playerWalkDownLeftTexture;
        private Texture2D _playerWalkDownRightTexture;

        private const int SPRITE_WIDTH = 32;
        private const int SPRITE_HEIGHT = 32;

        // Declare instances of your parsed map and renderer
        private Texture2D _dungeonTilesetTexture;
        //private TileMapRenderer _mapRenderer;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _serviceLocator  = new ServiceLocator();
        }

        protected override void Initialize()
        {
            _timeManager = new TimeManager();
            _serviceLocator.Register<ITimeManager>(_timeManager);

            _graphics.SynchronizeWithVerticalRetrace = true; // Enable VSync
            IsFixedTimeStep = true;                         // Cap update rate
            TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 60.0); // 60 FPS

            // TODO: Add your initialization logic here
            _gameLoop = new Engine(this, _serviceLocator);
            _assetManager = new AssetManager(Content, _tileMapManager);
            

            _world = new World();

            _playerEntity = _world.CreateEntity();
            _playerEntity.AddComponent(new TransformComponent { Position = new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Rotation = 0f, Scale = Vector2.One });
            _playerEntity.AddComponent(new VelocityComponent { Value = Vector2.Zero });
            _playerEntity.AddComponent(new HealthComponent { CurrentHealth = 130, MaxHealth = 100 });
            _playerEntity.AddComponent(new PlayerInputComponent { IsPlayerControlled = true });
            _playerEntity.AddComponent(new ColliderComponent { Bounds = new Rectangle(0, 0, 96, 96), IsTrigger = false, IsStatic = false });
            _playerEntity.AddComponent(new AnimationComponent { Clips = new Dictionary<AnimationType, AnimationClip>(), CurrentClipName = AnimationType.Idle });

            _cameraEntity = _world.CreateEntity();
            _cameraEntity.AddComponent(new TransformComponent { Position = new Vector2(400, 240), Rotation = 0f, Scale = Vector2.One });
            _cameraEntity.AddComponent(new CameraComponent(GraphicsDevice.Viewport) { Zoom = 1f});

            // 3. Register EngineSystems with the GameLoop
            // Order matters for systems that depend on each other's output!
            _gameLoop.RegisterUpdateSystem(new PlayerInputSystem());
            _gameLoop.RegisterUpdateSystem(new CalculateVelocitySystem());
            _gameLoop.RegisterUpdateSystem(new MotionSystem(_serviceLocator));
            _gameLoop.RegisterUpdateSystem(new AnimationStateSystem());
            _gameLoop.RegisterUpdateSystem(new AnimationSystem(_serviceLocator));
            _gameLoop.RegisterUpdateSystem(new CameraUpdateSystem(_playerEntity.Id));

            //_gameLoop.Initialize();
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            //_gameMap = Content.Load<TiledMap>('')

            _playerTexture = _assetManager.LoadTexture("player");
            _playerWalkUpTexture = _assetManager.LoadTexture("Player/Character_Up");
            _playerWalkDownTexture = _assetManager.LoadTexture("Player/Character_Down");
            _playerWalkLeftTexture = _assetManager.LoadTexture("Player/Character_Left");
            _playerWalkRightTexture = _assetManager.LoadTexture("Player/Character_Right");
            _playerWalkUpRightTexture = _assetManager.LoadTexture("Player/Character_UpRight");
            _playerWalkDownRightTexture = _assetManager.LoadTexture("Player/Character_DownRight");
            _playerWalkUpLeftTexture = _assetManager.LoadTexture("Player/Character_UpLeft");
            _playerWalkDownLeftTexture = _assetManager.LoadTexture("Player/Character_DownLeft");

            _gameMap = Content.Load<TileMap>("test_map");
            foreach (var tileset in _gameMap.Tilesets)
            {
                var texture = _assetManager.LoadTexture(tileset.Name);
                _tilesetTextures[tileset] = texture;
            }

            if (_playerEntity.HasComponent<TransformComponent>())
            {
                _playerEntity.AddComponent(new SpriteComponent
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
            ref var playerAnimationComponent = ref _playerEntity.GetComponent<AnimationComponent>();
            ref var playerSpriteComponent = ref _playerEntity.GetComponent<SpriteComponent>();

            // Player Idle Animation (using first frame of walk_down as idle for simplicity)
            playerAnimationComponent.Clips[AnimationType.Idle] = new AnimationClip
            {
                Name = "Idle",
                Texture = _playerWalkDownTexture, // Idle uses the down-facing sheet
                Frames = new List<Rectangle> { new Rectangle(0, 0, SPRITE_WIDTH, SPRITE_HEIGHT) },
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
                new Rectangle(0 * SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT),
                new Rectangle(1 * SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT),
                new Rectangle(2 * SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT),
                new Rectangle(3 * SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT)
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
                new Rectangle(0 * SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT),
                new Rectangle(1 * SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT),
                new Rectangle(2 * SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT),
                new Rectangle(3 * SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT),
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
                new Rectangle(0 * SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT),
                new Rectangle(1 * SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT),
                new Rectangle(2 * SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT),
                new Rectangle(3 * SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT),

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
                new Rectangle(0 * SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT),
                new Rectangle(1 * SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT),
                new Rectangle(2 * SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT),
                new Rectangle(3 * SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT)
            },
                FrameDuration = 0.1f,
                IsLooping = true
            };
            playerAnimationComponent.Clips[AnimationType.WalkUpRight] = new AnimationClip
            {
                Name = "WalkUpRight",
                Texture = _playerWalkUpRightTexture,
                Frames = new List<Rectangle>
            {
                new Rectangle(0 * SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT),
                new Rectangle(1 * SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT),
                new Rectangle(2 * SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT),
                new Rectangle(3 * SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT)
            },
                FrameDuration = 0.1f,
                IsLooping = true
            };
            playerAnimationComponent.Clips[AnimationType.WalkDownRight] = new AnimationClip
            {
                Name = "WalkDownRight",
                Texture = _playerWalkDownRightTexture,
                Frames = new List<Rectangle>
            {
                new Rectangle(0 * SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT),
                new Rectangle(1 * SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT),
                new Rectangle(2 * SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT),
                new Rectangle(3 * SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT)
            },
                FrameDuration = 0.1f,
                IsLooping = true
            };
            playerAnimationComponent.Clips[AnimationType.WalkUpLeft] = new AnimationClip
            {
                Name = "WalkUpLeft",
                Texture = _playerWalkUpLeftTexture,
                Frames = new List<Rectangle>
            {
                new Rectangle(0 * SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT),
                new Rectangle(1 * SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT),
                new Rectangle(2 * SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT),
                new Rectangle(3 * SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT)
            },
                FrameDuration = 0.1f,
                IsLooping = true
            };
            playerAnimationComponent.Clips[AnimationType.WalkDownLeft] = new AnimationClip
            {
                Name = "WalkDownLeft",
                Texture = _playerWalkDownLeftTexture,
                Frames = new List<Rectangle>
            {
                new Rectangle(0 * SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT),
                new Rectangle(1 * SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT),
                new Rectangle(2 * SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT),
                new Rectangle(3 * SPRITE_WIDTH, 0, SPRITE_WIDTH, SPRITE_HEIGHT)
            },
                FrameDuration = 0.1f,
                IsLooping = true
            };

            playerSpriteComponent.Texture = playerAnimationComponent.CurrentClip.Texture;
            playerSpriteComponent.SourceRectangle = playerAnimationComponent.CurrentClip.Frames[0]; 
            playerSpriteComponent.Origin = new Vector2(SPRITE_WIDTH / 2, SPRITE_HEIGHT / 2); 

            _playerEntity.AddComponent(playerAnimationComponent);
            _playerEntity.AddComponent(playerSpriteComponent);



            _gameLoop.RegisterDrawSystem(new SpriteRenderSystem());
            _gameLoop.LoadContent(_spriteBatch);
        }

        protected override void Update(GameTime gameTime)
        {
            _timeManager.Update(gameTime);
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            _gameLoop.Update(_world);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            var camera = _world.GetComponent<CameraComponent>(_cameraEntity.Id);
            var viewport = camera.Viewport;
            var viewMatrix = camera.ViewMatrix;

            _spriteBatch.Begin(transformMatrix: viewMatrix);

            // Get camera position in world space (inverse of translation)
            Matrix inverseView = Matrix.Invert(viewMatrix);
            Vector2 cameraPosition = new Vector2(inverseView.Translation.X, inverseView.Translation.Y);

            // Tile culling bounds
            int tileWidth = _gameMap.TileWidth;
            int tileHeight = _gameMap.TileHeight;

            int mapWidth = _gameMap.Width;
            int mapHeight = _gameMap.Height;

            float zoom = camera.Zoom;
            float viewWidth = viewport.Width / zoom;
            float viewHeight = viewport.Height / zoom;

            int minX = Math.Max((int)(cameraPosition.X / tileWidth), 0);
            int minY = Math.Max((int)(cameraPosition.Y / tileHeight), 0);

            int maxX = Math.Min((int)((cameraPosition.X + viewWidth) / tileWidth) + 1, mapWidth);
            int maxY = Math.Min((int)((cameraPosition.Y + viewHeight) / tileHeight) + 1, mapHeight);

            foreach (var layer in _gameMap.Layers)
            {
                for (int y = minY; y < maxY; y++)
                {
                    for (int x = minX; x < maxX; x++)
                    {
                        int tileId = layer.GetTileId(x, y);
                        if (tileId == 0)
                            continue;

                        // Find the tileset for this tile
                        Tileset tileset = null;
                        for (int i = _gameMap.Tilesets.Count - 1; i >= 0; i--)
                        {
                            if (tileId >= _gameMap.Tilesets[i].FirstGID)
                            {
                                tileset = _gameMap.Tilesets[i];
                                break;
                            }
                        }

                        if (tileset == null || !_tilesetTextures.TryGetValue(tileset, out var tilesetTexture))
                            continue;

                        int localTileId = tileId - tileset.FirstGID;
                        int tilesPerRow = tilesetTexture.Width / tileset.TileWidth;

                        int tileX = localTileId % tilesPerRow;
                        int tileY = localTileId / tilesPerRow;

                        Rectangle sourceRect = new Rectangle(
                            tileX * tileset.TileWidth,
                            tileY * tileset.TileHeight,
                            tileset.TileWidth,
                            tileset.TileHeight
                        );

                        Vector2 position = new Vector2(
                            x * tileset.TileWidth,
                            y * tileset.TileHeight
                        );

                        _spriteBatch.Draw(tilesetTexture, position, sourceRect, Color.White);
                    }
                }
            }

            _spriteBatch.End();




            // TODO: Add your drawing code here

            _gameLoop.Draw(gameTime, _world);
     


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
