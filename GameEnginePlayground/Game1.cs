using Common.Events;
using Common.Interfaces;
using GameEngine.Core.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

namespace GameEnginePlayground.Factories
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private IAssetManager _assetManager;
        private IEventManager _eventManager;

        private IFactory<IScene> _sceneFactory;
        private IScene _scene;

        private IInputManager _inputManager;
        private readonly IKeybindFactory _keybindFactory;
        private ITimeManager _timeManager;

        public Game1(IAssetManager assetManager, IEventManager eventManager, IInputManager inputManager, IKeybindFactory keybindFactory)
        {
            _assetManager = assetManager;
            _eventManager = eventManager;
            _inputManager = inputManager;
            _keybindFactory = keybindFactory;
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {

            _timeManager = new TimeManager();

            _graphics.SynchronizeWithVerticalRetrace = true;
            IsFixedTimeStep = true; 
            TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 60.0);
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _sceneFactory = new SceneFactory(_assetManager, _inputManager, GraphicsDevice, _timeManager, _spriteBatch, _eventManager);
            _assetManager.Initialize(Content);         
            _keybindFactory.LoadContent();
            // TODO: use this.Content to load your game content here
            _scene = _sceneFactory.Create();

        }

        protected override void Update(GameTime gameTime)
        {
            _timeManager.Update(gameTime);
            
            _inputManager.Update();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _scene.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here

            _scene.Draw(_spriteBatch);
            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
            _scene.UnloadContent();
            _spriteBatch.Dispose();
            base.UnloadContent();
        }
    }
}
