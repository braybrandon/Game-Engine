using GameEngine.Core.Components;
using GameEngine.Core.Services;
using GameEngine.Core.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Core
{
    public class Engine(Game game) : IDisposable
    {

        private Game _game = game;
        private List<EngineSystem> _systems = new List<EngineSystem>();
        private SpriteBatch? _spriteBatch;

        private TimeSpan _accumulator = TimeSpan.Zero;
        private TimeSpan _fixedUpdateStep = TimeSpan.FromSeconds(1.0 / 60.0);

        public void RegisterSystem(EngineSystem system)
        {
            if (!_systems.Contains(system))
            {
                _systems.Add(system);
            }
        }

        public void UnregiserSystem(EngineSystem system)
        {
            _systems.Remove(system);
        }

        /// <summary>
        /// Performs initial setup for all registered systems.
        /// </summary>
        public void Initialize()
        {
            foreach (EngineSystem system in _systems)
            {
                  system.Initialize();
            }
        }

        /// <summary>
        /// Loads Content for all registered systems.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void LoadContent(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
            foreach (EngineSystem system in _systems)
            {
                system.LoadContent();
            }
        }

        /// <summary>
        /// Updates the game state. Handles fixed-time step logic
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(World currentWorld)
        {
            ITimeManager timeManager = ServiceLocator.GetService<ITimeManager>();
            _accumulator += timeManager.UnscaledElapsed;
            while (_accumulator >= timeManager.FixedStep)
            {
                _accumulator -= timeManager.FixedStep;
                foreach (EngineSystem system in _systems)
                {
                    system.Update(currentWorld);
                }
            }
        }

        /// <summary>
        /// Draws the game.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Draw(GameTime gameTime, World world)
        {
            if (_spriteBatch != null)
            {
                foreach (EngineSystem system in _systems)
                {
                    system.Draw(_spriteBatch, world);
                }
            }
        }

        /// <summary>
        /// Disposes of all registered systems and resources.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Dispose()
        {
            foreach (EngineSystem system in _systems)
            {
                system.Dispose();
            }
            _systems.Clear();
        }
    }
}
