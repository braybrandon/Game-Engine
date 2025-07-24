using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Core
{
    public class GameLoop : IDisposable
    {

        private Game _game;
        private SpriteBatch _spriteBatch;

        private TimeSpan _accumulator = TimeSpan.Zero;
        private TimeSpan _fixedUpdateStep = TimeSpan.FromSeconds(1.0 / 60.0);

        public GameLoop(Game game)
        {
            _game = game;
        }

        /// <summary>
        /// Performs initial setup for all registered systems.
        /// </summary>
        public void Initialize()
        {

        }

        /// <summary>
        /// Loads Content for all registered systems.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void LoadContent(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
        }

        /// <summary>
        /// Updates the game state. Handles fixed-time step logic
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            _accumulator += gameTime.ElapsedGameTime;
            while (_accumulator >= _fixedUpdateStep)
            {
                _accumulator -= _fixedUpdateStep;
            }
        }

        /// <summary>
        /// Draws the game.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Draw(GameTime gameTime)
        {
            _game.GraphicsDevice.Clear(Color.White);
            if (_spriteBatch != null)
            {

            }
        }

        /// <summary>
        /// Disposes of all registered systems and resources.
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
