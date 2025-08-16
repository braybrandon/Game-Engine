using Common.Components;
using Common.Config;
using Common.Interfaces;
using Common.Physics.Components;
using Common.Physics.Interfaces;
using GameEngine.Core.Components;
using GameEngine.Engine;
using GameEngine.Graphics.Animations;
using GameEngine.Graphics.Camera;
using GameEngine.Graphics.Render;
using GameEngine.Physics;
using GameEngine.Physics.CollisionDetection;
using GameEngine.Physics.CollisionDetection.Models;
using GameEngine.Physics.Motion;
using GameEnginePlayground.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace GameEnginePlayground.Factories
{
    public class SceneFactory : IFactory<IScene>
    {
        private readonly IAssetManager _assetManager;
        private readonly IInputManager _inputManager;
        private readonly GraphicsDevice _graphicsDevice;
        private readonly ITimeManager _timeManager;
        private readonly IFactory<ITileMap> _mapFactory;
        private readonly SpriteBatch _spriteBatch;
        private readonly IEventManager _eventManager;

        public SceneFactory(IAssetManager assetManager, IInputManager inputManager, GraphicsDevice graphicsDevice, ITimeManager timeManager, SpriteBatch spriteBatch, IEventManager eventManager) {
            _assetManager = assetManager;
            _inputManager = inputManager;
            _graphicsDevice = graphicsDevice;
            _timeManager = timeManager;
            _mapFactory = new GameMapFactory(_assetManager);
            _spriteBatch = spriteBatch;
            _eventManager = eventManager;
        }
        
        public IScene Create()
        {
            IScene scene = new TestScene();
            AddSystems(scene);
            return scene;
        }

        private void AddSystems(IScene scene)
        {
            IWorld sceneWorld = scene.GetWorld();
            var gameMap = _mapFactory.Create();
            var quadtree = new Quadtree(new Rectangle(0, 0, gameMap.Width, gameMap.Height), 4);
            PlayerFactory playerFactory = new PlayerFactory(_graphicsDevice, _assetManager, sceneWorld, gameMap.PlayerLayer, quadtree);
            IEntity playerEntity = playerFactory.Create();
            IEntity cameraEntity = CreateCamera(sceneWorld, gameMap.PlayerLayer);



            cameraEntity.AddComponent(new CullingComponent { MaxX = 0, MaxY = 0, MinX = 0, MinY = 0 });
            var grassTexture = _assetManager.Load<Texture2D>("tall-grass");
            AddObjects(gameMap, sceneWorld, gameMap.ObjectTileLayer, grassTexture, quadtree);

            var treeTexture = _assetManager.Load<Texture2D>("tree");
            AddObjects(gameMap, sceneWorld, gameMap.TreeLayer, treeTexture, quadtree);

            Texture2D _pixel = new Texture2D(_graphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });
            ICollisionMap collisionMap = new CollisionMap(gameMap, gameMap.Layers[0]);

            // 3. Register EngineSystems with the GameLoop
            // Order matters for systems that depend on each other's output!
            scene.RegisterUpdateSystem(new PlayerMovementInputSystem(_inputManager));
            scene.RegisterUpdateSystem(new MovementSystem(_timeManager, gameMap));
            scene.RegisterUpdateSystem(new CollisionSystem(collisionMap, quadtree));
            scene.RegisterUpdateSystem(new AnimationStateSystem(_inputManager));
            scene.RegisterUpdateSystem(new AnimationSystem(_timeManager));
            scene.RegisterUpdateSystem(new CameraUpdateSystem(playerEntity.Id, _inputManager));

            scene.RegisterUpdateSystem(new CullingSystem(cameraEntity, gameMap));
            scene.RegisterUpdateSystem(new MouseEventHandlerSystem(sceneWorld, cameraEntity, gameMap, _eventManager, playerEntity, _assetManager, _inputManager, quadtree));
            scene.RegisterUpdateSystem(new ProjectileLiftetimeSystem(_timeManager, quadtree));

            var pp = _graphicsDevice.PresentationParameters;
            var mainTarget = new RenderTarget2D(_graphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);
            var lightTarget = new RenderTarget2D(_graphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);
            var lightEffect = _assetManager.Load<Effect>("lighteffect");
            var lightTexture = _assetManager.LoadTexture(FileNameConfig.LightMask);

            scene.RegisterDrawSystem(new TileMapRenderSystem(gameMap, cameraEntity, mainTarget, _graphicsDevice));
            scene.RegisterDrawSystem(new SpriteRenderSystem(_pixel));
            //scene.RegisterDrawSystem(new LightFxRenderSystem(_graphicsDevice, playerEntity, cameraEntity, lightTarget, mainTarget, lightTexture, lightEffect));
            //scene.RegisterDrawSystem(new DebugRendererSystem(_pixel, cameraEntity, collisionMap));
        }

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
