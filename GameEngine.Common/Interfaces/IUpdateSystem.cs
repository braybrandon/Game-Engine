using Microsoft.Xna.Framework.Graphics;

namespace Common.Interfaces
{
    public interface IUpdateSystem
    {
        /// <summary>
        /// Called every fixed game update. Use for game logic, physics, AI, etc.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(IWorld world);

    }
}
