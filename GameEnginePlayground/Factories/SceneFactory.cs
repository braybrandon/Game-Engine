using Common.Config;
using Common.Interfaces;
using GameEngine.Core.Components;
using GameEngine.Graphics.Animations;
using GameEngine.Graphics.Camera;
using GameEngine.Graphics.Render;
using GameEngine.Physics;
using GameEngine.Physics.Motion;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEnginePlayground.Factories
{
    public class SceneFactory : IFactory<IScene>
    {
        private readonly IAssetManager _assetManager;
        private readonly IInputManager _inputManager;
        private readonly GraphicsDevice _graphicsDevice;
        private readonly ITimeManager _timeManager;
        private readonly IFactory<ITileMap> _mapFactory;

        public SceneFactory(IAssetManager assetManager, IInputManager inputManager, GraphicsDevice graphicsDevice, ITimeManager timeManager) {
            _assetManager = assetManager;
            _inputManager = inputManager;
            _graphicsDevice = graphicsDevice;
            _timeManager = timeManager;
            _mapFactory = new GameMapFactory(_assetManager);
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
            PlayerFactory playerFactory = new PlayerFactory(_graphicsDevice, _assetManager, sceneWorld);
            IEntity playerEntity = playerFactory.Create();
            IEntity cameraEntity = CreateCamera(sceneWorld);

            var gameMap = _mapFactory.Create();
            ICollisionMap collisionMap = new CollisionMap(gameMap, gameMap.Layers[0]);

            // 3. Register EngineSystems with the GameLoop
            // Order matters for systems that depend on each other's output!
            scene.RegisterUpdateSystem(new MotionSystem(_timeManager, gameMap));
            scene.RegisterUpdateSystem(new AnimationStateSystem(_inputManager));
            scene.RegisterUpdateSystem(new AnimationSystem(_timeManager));
            scene.RegisterUpdateSystem(new CameraUpdateSystem(playerEntity.Id, _inputManager));
            scene.RegisterUpdateSystem(new CalculateVelocitySystem(_inputManager));

            var pp = _graphicsDevice.PresentationParameters;
            var mainTarget = new RenderTarget2D(_graphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);
            var lightTarget = new RenderTarget2D(_graphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);
            var lightEffect = _assetManager.Load<Effect>("lighteffect");
            var lightTexture = _assetManager.LoadTexture(FileNameConfig.LightMask);

            Texture2D _pixel = new Texture2D(_graphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });

            scene.RegisterDrawSystem(new TileMapRenderSystem(gameMap, cameraEntity, mainTarget, _graphicsDevice, collisionMap, _pixel));
            scene.RegisterDrawSystem(new SpriteRenderSystem(_pixel));
            scene.RegisterDrawSystem(new LightFxRenderSystem(_graphicsDevice, playerEntity, cameraEntity, lightTarget, mainTarget, lightTexture, lightEffect));
        }

        private IEntity CreateCamera(IWorld world)
        {
            IEntity cameraEntity = world.CreateEntity();
            cameraEntity.AddComponent(new TransformComponent { Position = new Vector2(400, 240), Rotation = 0f, Scale = Vector2.One });
            cameraEntity.AddComponent(new CameraComponent(_graphicsDevice.Viewport) { Zoom = 1f });
            return cameraEntity;
        }
    }
}
