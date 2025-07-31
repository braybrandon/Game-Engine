using Microsoft.Xna.Framework.Graphics;

namespace Common.Interfaces
{
    public interface IDrawSystem
    {
        /// <summary>
        /// Called every fixed game update. Use for game logic, physics, AI, etc.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Draw(SpriteBatch _spriteBatch, IWorld world);
        /// <summary>
        /// Called when the system is disposed. Use for cleaning up resources.
        /// </summary>
        //public  void Dispose();
    }
}
