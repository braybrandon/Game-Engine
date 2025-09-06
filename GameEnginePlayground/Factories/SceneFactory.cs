using GameEngine.Common.Components;
using GameEngine.Common.Config;
using GameEngine.Common.Interfaces;
using GameEngine.Common.IO.Interface;
using GameEngine.Common.Physics;
using GameEngine.Common.Physics.Components;
using GameEngine.Common.Physics.Interfaces;
using GameEngine.Gameplay.AI.BehaviorTree;
using GameEngine.Gameplay.AI.Components;
using GameEngine.Gameplay.AI.Systems;
using GameEngine.Gameplay.Combat.Systems;
using GameEngine.Gameplay.Scene;
using GameEngine.Graphics.Animations;
using GameEngine.Graphics.Camera;
using GameEngine.Graphics.Components;
using GameEngine.Graphics.Render;
using GameEngine.IO.Audio.Systems;
using GameEngine.IO.Controller;
using GameEngine.Physics;
using GameEngine.Physics.CollisionDetection;
using GameEngine.Physics.CollisionDetection.Models;
using GameEngine.Physics.Motion;
using GameEnginePlayground.Factories.DataObjects;
using GameEnginePlayground.Services;
using GameEnginePlayground.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace GameEnginePlayground.Factories
{
    /// <summary>
    /// Factory for creating game scenes with all required systems, entities, and components.
    /// Uses asset manager, audio manager, input manager, and other dependencies to fully configure the scene.
    /// </summary>
    public class SceneFactory : IFactory<IScene, SceneData>
    {
        private readonly IAssetManager _assetManager;
        private readonly IAudioManager _audioManager;
        private readonly IInputManager _inputManager;
        private readonly GraphicsDevice _graphicsDevice;
        private readonly ITimeManager _timeManager;
        private readonly IFactory<ITileMap, MapData> _mapFactory;
        private readonly SpriteBatch _spriteBatch;
        private readonly IEventManager _eventManager;

        /// <summary>
        /// Initializes a new instance of the SceneFactory with required dependencies.
        /// </summary>
        /// <param name="assetManager">The asset manager for loading assets.</param>
        /// <param name="audioManager">The audio manager for sound effects and music.</param>
        /// <param name="inputManager">The input manager for handling input.</param>
        /// <param name="graphicsDevice">The graphics device for rendering.</param>
        /// <param name="timeManager">The time manager for game timing.</param>
        /// <param name="spriteBatch">The sprite batch for rendering sprites.</param>
        /// <param name="eventManager">The event manager for event handling.</param>
        public SceneFactory(IAssetManager assetManager, IAudioManager audioManager, IInputManager inputManager, GraphicsDevice graphicsDevice, ITimeManager timeManager, SpriteBatch spriteBatch, IEventManager eventManager) {
            _assetManager = assetManager;
            _audioManager = audioManager;
            _inputManager = inputManager;
            _graphicsDevice = graphicsDevice;
            _timeManager = timeManager;
            _mapFactory = new GameMapFactory(_assetManager);
            _spriteBatch = spriteBatch;
            _eventManager = eventManager;
        }
        
        /// <summary>
        /// Creates a game scene with all required systems, entities, and components using the provided scene data.
        /// </summary>
        /// <param name="data">The scene data used for configuration (extend SceneData for more options).</param>
        /// <returns>The fully configured scene instance.</returns>
        public IScene Create(SceneData data)
        {
            IScene scene = new Scene();
            AddSystems(scene);
            return scene;
        }

        /// <summary>
        /// Adds all required systems, entities, and components to the scene, including player, camera, map, and update/draw systems.
        /// </summary>
        /// <param name="scene">The scene to configure.</param>
        private void AddSystems(IScene scene)
        {
            IWorld sceneWorld = scene.GetWorld();

            var gameMap = _mapFactory.Create(new MapData());
            var quadtree = new Quadtree(new Rectangle(0, 0, gameMap.Width, gameMap.Height), 4);
            PlayerFactory playerFactory = new PlayerFactory(_graphicsDevice, _assetManager, _eventManager, sceneWorld, gameMap.PlayerLayer, quadtree);
            IEntity playerEntity = playerFactory.Create(new PlayerData());
            IEntity cameraEntity = CreateCamera(sceneWorld, gameMap.PlayerLayer);
            SoundEffect fireballSfx = _assetManager.Load<SoundEffect>("fireball-sound");
            SoundEffect shovelSfx = _assetManager.Load<SoundEffect>("shovel-sound");
            _audioManager.RegisterSfx("fireball.shoot", fireballSfx);
            _audioManager.RegisterSfx("shovel.dig", shovelSfx);

            cameraEntity.AddComponent(new CullingComponent { MaxX = 0, MaxY = 0, MinX = 0, MinY = 0 });
            var grassTexture = _assetManager.Load<Texture2D>("tall-grass");
            AddObjects(gameMap, sceneWorld, gameMap.ObjectTileLayer, grassTexture, quadtree);

            var treeTexture = _assetManager.Load<Texture2D>("tree");
            AddObjects(gameMap, sceneWorld, gameMap.TreeLayer, treeTexture, quadtree);

            Texture2D _pixel = new Texture2D(_graphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });
            ICollisionMap collisionMap = new CollisionMap(gameMap, gameMap.Layers[0]);

            // Register EngineSystems with the GameLoop
            scene.RegisterUpdateSystem(new PlayerMovementInputSystem(_inputManager));
            scene.RegisterUpdateSystem(new MovementSystem(_timeManager));
            scene.RegisterUpdateSystem(new CollisionSystem(collisionMap, quadtree));
            scene.RegisterUpdateSystem(new AnimationStateSystem());
            scene.RegisterUpdateSystem(new AnimationSystem(_timeManager));
            scene.RegisterUpdateSystem(new CameraUpdateSystem(playerEntity.Id, _inputManager));
            scene.RegisterUpdateSystem(new CullingSystem(cameraEntity, gameMap));
            scene.RegisterUpdateSystem(new ProjectileLiftetimeSystem(_timeManager, quadtree));
            scene.RegisterUpdateSystem(new TargetingSystem());
            scene.RegisterUpdateSystem(new BehaviorTreeSystem());
            scene.RegisterUpdateSystem(new InputSystem(_inputManager));
            scene.RegisterUpdateSystem(new SoundFxSystem(_audioManager, _eventManager));

            IFactory<IEntity, ProjectileData> projectileFactory = new ProjectileFactory(sceneWorld, quadtree);
            AddEnemy(sceneWorld, _pixel, quadtree, projectileFactory);
            scene.RegisterEventSystem(new ProjectileSystem(_eventManager, _inputManager, _assetManager, projectileFactory, _graphicsDevice));
            scene.RegisterEventSystem(new MouseEventHandlerSystem(sceneWorld, cameraEntity, playerEntity, gameMap, _eventManager));

            var pp = _graphicsDevice.PresentationParameters;
            var mainTarget = new RenderTarget2D(_graphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);
            var lightTarget = new RenderTarget2D(_graphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);
            var lightEffect = _assetManager.Load<Effect>("lighteffect");
            var lightTexture = _assetManager.LoadTexture(FileNameConfig.LightMask);

            scene.RegisterDrawSystem(new TileMapRenderSystem(gameMap, cameraEntity, mainTarget, _graphicsDevice));
            scene.RegisterDrawSystem(new SpriteRenderSystem(_pixel));
            //scene.RegisterDrawSystem(new LightFxRenderSystem(_graphicsDevice, playerEntity, cameraEntity, lightTarget, mainTarget, lightTexture, lightEffect));
            scene.RegisterDrawSystem(new DebugRendererSystem(_pixel, cameraEntity, collisionMap));
        }

        /// <summary>
        /// Adds an enemy entity to the world with preconfigured components and behavior tree.
        /// </summary>
        /// <param name="world">The world to add the enemy to.</param>
        /// <param name="pixel">The texture used for the enemy sprite.</param>
        private void AddEnemy(IWorld world, Texture2D pixel, Quadtree quadtree, IFactory<IEntity, ProjectileData> projectileFactory)
        {

            Texture2D enemyTexture = _assetManager.LoadTexture("sasuke");
            for(int i = 0; i < 10; i++)
            {
                var randomx = new Random().Next(20, 1500);
                var randomy = new Random().Next(20, 1500);
                var position = new Vector2(randomx, randomy);
                var enemy = world.CreateEntity();
                var origin = new Vector2(enemyTexture.Width / 2, enemyTexture.Height / 2);
                var bounds = new Rectangle(enemyTexture.Width / 2, enemyTexture.Height / 2, enemyTexture.Width, enemyTexture.Height);
                enemy.AddComponent(new TransformComponent { Position = position, Scale = Vector2.One });
                enemy.AddComponent(new DirectionComponent { Value = Vector2.Zero });
                enemy.AddComponent(new PerceptionComponent { Radius = 200 });
                enemy.AddComponent(new AIComponent());
                enemy.AddComponent(new HealthComponent { CurrentHealth = 100, MaxHealth = 100 });
                enemy.AddComponent(new TargetComponent());
                enemy.AddComponent(new ProposedPositionComponent { Value = position });
                enemy.AddComponent(new SpeedComponent() { Value = 60f });
                enemy.AddComponent(new ColliderComponent { Bounds = bounds, IsTrigger = false, IsStatic = false, Filter = Filters.Enemy });
                var hasTarget = new HasTargetNode(enemy);
                var chasePlayer = new MoveToTargetNode(enemy);
                var chaseSequence = new SequenceNode(enemy, hasTarget, chasePlayer);
                var fireballNode = new FireballNode(enemy, projectileFactory, _eventManager, world);
                var inRange = new InRangeNode(enemy);
                var gcd = new GlobalCooldownNode(1f);
                var stopMovement = new StopMovementNode(enemy);
                var attackSequence = new SequenceNode(enemy, hasTarget, inRange, stopMovement, gcd, fireballNode);
                var rootSelector = new SelectorNode(enemy, attackSequence, chaseSequence);
                var transformBounds = new Rectangle(
                    (int)position.X - bounds.X,
                    (int)position.Y - bounds.Y,
                    bounds.Width,
                    bounds.Height
                );
                quadtree.Insert(enemy, transformBounds);

                enemy.AddComponent(new BehaviorTreeComponent(rootSelector));
                enemy.AddComponent(new SpriteComponent
                {
                    Texture = enemyTexture,
                    SourceRectangle = enemyTexture.Bounds,
                    Color = Color.White,
                    Scale = 1f,
                    Rotation = 0f,
                    Origin = origin,
                    Effects = SpriteEffects.None,
                    LayerDepth = 0f
                });
            }

        }

        /// <summary>
        /// Adds objects from a tile layer to the world, creating entities with transform, collider, and sprite components.
        /// Inserts entities into the quadtree for spatial partitioning.
        /// </summary>
        /// <param name="map">The tile map containing the layer.</param>
        /// <param name="world">The world to add objects to.</param>
        /// <param name="layer">The tile layer containing objects.</param>
        /// <param name="grassTexture">The texture to use for object sprites.</param>
        /// <param name="quadTree">The quadtree for spatial partitioning.</param>
        private void AddObjects(ITileMap map, IWorld world, ITileLayer layer, Texture2D grassTexture, IQuadTree quadTree)
        {
            foreach (var item in layer.Objects)
            {
                var grass = world.CreateEntity();
                grass.AddComponent(new TransformComponent { Position = new Vector2((float)item.X, (float)item.Y), Scale = Vector2.One });
                var tileset = map.GetTilesetForTile(item.Gid);
                var width = tileset.TileWidth;
                var height = tileset.TileHeight;
                var colliderHeight = 0.0;
                var colliderWidth = 0.0;
                if (tileset.Tiles?.Count > 0 && tileset.Tiles[0].Objects.Count > 0)
                {
                    colliderHeight = tileset.Tiles[0].Objects[0].Height;
                    colliderWidth = tileset.Tiles[0].Objects[0].Width;
                    var colliderX = tileset.Tiles[0].Objects[0].X;
                    var colliderY = tileset.Tiles[0].Objects[0].Y;
                    var cbounds = new Rectangle((int)(-(colliderX - width / 2)), (int)(-(colliderY - height / 2)), (int)colliderWidth, (int)colliderHeight);
                    grass.AddComponent(new ColliderComponent { Bounds = cbounds, IsStatic = true});
                    var transformBounds = new Rectangle(
                        (int)item.X - cbounds.X,
                        (int)item.Y - cbounds.Y,
                        cbounds.Width,
                        cbounds.Height
                    );
                    quadTree.Insert(grass, transformBounds);
                }

                // Correctly calculate the tile ID and its row/column
                var id = item.Gid - tileset.FirstGID;
                var tileX = id % tileset.Columns; // Correctly get the column
                var tileY = id / tileset.Columns; // Correctly get the row

                // Multiply by tile dimensions to get the pixel coordinates for the source rectangle
                var bounds = new Rectangle((int)tileX * width, (int)tileY * height, width, height);

                grass.AddComponent(new SpriteComponent
                {
                    Texture = grassTexture,
                    SourceRectangle = bounds,
                    Color = Color.White,
                    Scale = 1f,
                    Rotation = 0f,
                    Origin = new Vector2(width / 2, height / 2),
                    Effects = SpriteEffects.None,
                    LayerDepth = 0f
                });
            }
        }

        /// <summary>
        /// Creates a camera entity positioned at the player spawn location or a default position.
        /// Adds transform and camera components to the entity.
        /// </summary>
        /// <param name="world">The world to create the camera entity in.</param>
        /// <param name="playerLayer">The tile layer containing player spawn information.</param>
        /// <returns>The configured camera entity.</returns>
        private IEntity CreateCamera(IWorld world, ITileLayer playerLayer)
        {
            var positionObject = playerLayer.Objects.FirstOrDefault(o => o.Name == "PlayerSpawn");
            var position = new Vector2(400, 240);
            if(positionObject != default)
            {
                position.X = (int)positionObject.X;
                position.Y = (int)positionObject.Y;
            }
            IEntity cameraEntity = world.CreateEntity();
            cameraEntity.AddComponent(new TransformComponent { Position = position, Rotation = 0f, Scale = Vector2.One });
            cameraEntity.AddComponent(new CameraComponent(_graphicsDevice.Viewport) { Zoom = 1f });
            return cameraEntity;
        }
    }
}
