using Common.Interfaces;
using GameEngine.Core.Components;
using GameEngine.Core.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine.Core
{
    public class Engine(Game game, IServiceLocator serviceLocator) : IDisposable
    {

        private Game _game = game;
        private List<IUpdateSystem> _updateSystems = new List<IUpdateSystem>();
        private List<IDrawSystem> _drawSystems = new List<IDrawSystem>();
        private SpriteBatch? _spriteBatch;
        private IServiceLocator _serviceLocator = serviceLocator;

        private TimeSpan _accumulator = TimeSpan.Zero;
        private TimeSpan _fixedUpdateStep = TimeSpan.FromSeconds(1.0 / 60.0);

        public void RegisterUpdateSystem(IUpdateSystem system)
        {
            if (!_updateSystems.Contains(system))
            {
                _updateSystems.Add(system);
            }
        }

        public void UnregiserUpdateSystem(IUpdateSystem system)
        {
            _updateSystems.Remove(system);
        }

        public void RegisterDrawSystem(IDrawSystem system)
        {
            if (!_drawSystems.Contains(system))
            {
                _drawSystems.Add(system);
            }
        }

        public void UnregiserDrawSystem(IDrawSystem system)
        {
            _drawSystems.Remove(system);
        }

        /// <summary>
        /// Loads Content for all registered systems.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void LoadContent(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
            //foreach (IDrawSystem system in _drawSystems)
            //{
            //    system.LoadContent();
            //}
        }

        /// <summary>
        /// Updates the game state. Handles fixed-time step logic
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(IWorld currentWorld)
        {
            ITimeManager timeManager = _serviceLocator.GetService<ITimeManager>();
            _accumulator += timeManager.UnscaledElapsed;
            while (_accumulator >= timeManager.FixedStep)
            {
                _accumulator -= timeManager.FixedStep;
                foreach (IUpdateSystem system in _updateSystems)
                {
                    system.Update(currentWorld);
                }
            }
        }

        /// <summary>
        /// Draws the game.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Draw(GameTime gameTime, IWorld world)
        {
            if (_spriteBatch != null)
            {
                foreach (IDrawSystem system in _drawSystems)
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
            //foreach (IUpdateSystem system in _updateSystems)
            //{
            //    system.Dispose();
            //}
            //foreach (IDrawSystem system in _drawSystems)
            //{
            //    system.Dispose();
            //}
            _updateSystems.Clear();
            _drawSystems.Clear();
        }
    }
}
